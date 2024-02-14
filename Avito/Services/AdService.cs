using AutoMapper;
using Avito.Data;
using Avito.Interface;
using Avito.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Collections.Generic;
using System.IO;
using System.Threading.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Avito.Migrations;
using Microsoft.VisualStudio.Web.CodeGeneration.EntityFrameworkCore;

namespace Avito.Services
{
    public enum Sorted 
    {
        Ascending = 1,
        Descending
    }

    [Authorize(AuthenticationSchemes = "Bearer")]
    [Authorize]
    public class AdService : IAdService, ICategoryService
    {

        private readonly DataContext _dataContext;
        private readonly IMapper _mapper;
        private readonly HttpContextAccessor _httpContextAccessor;

        public AdService(DataContext dataContext, IMapper mapper, HttpContextAccessor httpContextAccessor)
        {
            _dataContext = dataContext;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        public string ChoosCategory(int id)
        {
            var result = _dataContext.Category.FirstOrDefault(x => x.Id == id);
            if (result == null)
            {
                throw new Exception("Категория не была найдена");
            }
            string? categoryName = result.Name;

            return categoryName;
        }

        private int GetUserId()
        {
            var requestHeaders = _httpContextAccessor.HttpContext?.Request?.Headers;
            if (requestHeaders != null)
            {
                string jwtToken = _httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
                if (!string.IsNullOrEmpty(jwtToken))
                {
                    JwtSecurityTokenHandler jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
                    JwtSecurityToken jwtSecurityToken = jwtSecurityTokenHandler.ReadJwtToken(jwtToken);
                    Claim? idClaim = jwtSecurityToken?.Claims.FirstOrDefault(claim => claim.Type == "Id");

                    if (idClaim != null && int.TryParse(idClaim.Value, out int userId))
                    {
                        return userId;
                    }
                    else
                    {
                        throw new Exception("Не удалось прочитать Id из токена");
                    }
                }
                else
                {
                    throw new Exception("Токен либо отсутствует, либо неверный формат");
                }
            }
            else
            {
                throw new Exception("HttpContext или заголовки запроса недоступны");
            }
        }

        private AdModels GetAdModelById(int adId)
        {
            AdModels? adModel = _dataContext.AdModels.Include(um => um.Photos).FirstOrDefault(ad => ad.Id == adId);   
            
            if (adModel == null)
            {
                throw new Exception("Объявление не найдено");
            }
            return adModel;
        }

        private void CheckAdOwnership(AdModels adModel, int userId)
        {
            if (adModel.PersonId != userId)
            {
                throw new Exception("Невозможно изменить объявление. Вы не являетесь владельцем объявления.");
            }
        }

        public int CreatePublication(AdModelInput adModelInput)
        {
            try
            {
                int userId = GetUserId();
                AdModels adModel = _mapper.Map<AdModels>(adModelInput);  
                
                adModel.IsActive = true;
                adModel.PersonId = userId;
                adModel.NameCategory = ChoosCategory(adModelInput.CategoryModelId);
                adModel.CreationDate = DateTime.UtcNow;

                _dataContext.AdModels.Add(adModel);
                _dataContext.SaveChanges();

                AdModelOutput adModelOutput = _mapper.Map<AdModelOutput>(adModel);
                int id = adModel.Id;
                
                return id;
            }
            catch (Exception ex)
            {
                throw new Exception("Произошла ошибка при создании объявления.", ex);
            }
        }


        public AdModelOutput AddPhoto(PhotoInput photoInput)
        {
            int userId = GetUserId();
            AdModels? adModel = _dataContext.AdModels.Include(um => um.Photos).FirstOrDefault(ad => ad.Id == photoInput.AdModelId);
            if (adModel == null)
            {
                throw new Exception("Объявление не найдено");
            }
            CheckAdOwnership(adModel, userId);

            if (photoInput.File != null && photoInput.File.Count > 0)
            {
                string filePath = @"D:\Avito";
                string uniqueSubPath = Guid.NewGuid().ToString();
                string fullPath = Path.Combine(filePath, uniqueSubPath);

                adModel.PathDirectory = fullPath;
                _dataContext.SaveChanges();

                DirectoryInfo directoryInfo = new DirectoryInfo(fullPath);
                if (!directoryInfo.Exists)
                {
                    directoryInfo.Create();
                }
                else
                {
                    throw new Exception("Такой каталог уже существует.");
                }

                foreach (IFormFile file in photoInput.File)
                {
                    if (file != null && file.Length > 0)
                    {
                        Photos photo = new Photos();

                        string uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;
                        string fullPathPhoto = Path.Combine(fullPath, uniqueFileName);

                        photo.PathFile = fullPathPhoto;
                        photo.FileName = file.FileName;

                        _dataContext.Photos.Add(photo);
                        _dataContext.SaveChanges();

                        using (FileStream fileStream = new FileStream(fullPathPhoto, FileMode.Create))
                        {
                            file.CopyTo(fileStream);
                        }

                        AdModelPhotoInput adModelPhotoInput = new AdModelPhotoInput()
                        {
                            AdModelId = adModel.Id,
                            PhotoId = photo.Id
                        };
                        AdModelPhoto adModelPhoto = _mapper.Map<AdModelPhoto>(adModelPhotoInput);

                        DbSet<AdModelPhoto> adModelPhotos = _dataContext.Set<AdModelPhoto>();
                        adModelPhotos.Add(adModelPhoto);
                        _dataContext.SaveChanges();
                    }
                    else
                    {
                        throw new Exception("Не удалось загрузить фотографию.");
                    }
                }
                AdModelOutput adModelOutput = _mapper.Map<AdModelOutput>(adModel);
                return adModelOutput;
            }
            else
            {
                throw new Exception("Файлы отсутствуют.");
            }
        }



        public AdModelOutput DeletePublication(int adId)
        {
            try
            {
                int userId = GetUserId();
                AdModels adModel = GetAdModelById(adId);
                CheckAdOwnership(adModel, userId);

                AdModelOutput adModelOutput = _mapper.Map<AdModelOutput>(adModel);

                _dataContext.AdModels.Remove(adModel);
                _dataContext.SaveChanges();

                return adModelOutput;
            }
            catch (Exception ex)
            {
                throw new Exception("Произошла ошибка при удалении объявления.", ex);
            }
        }

        public AdModelOutput GetPublication(int adId)
        {
            IQueryable<AdModels> query = _dataContext.AdModels
                .Include(am => am.Photos)
                .Where(x => x.Id == adId);

            AdModels? AdId = query.FirstOrDefault(x => x.Id == adId);
            if (AdId == null)
            {
                throw new Exception("Объявление не существует");
            }
            else if (!AdId.IsActive)
            {
                throw new Exception("Объявление снято с продажи");
            }

            AdId.NumberOfViews++;
            _dataContext.SaveChanges();

            AdModelOutput adModelOutput = _mapper.Map<AdModelOutput>(AdId);
            return adModelOutput;
        }


        public AdModelOutput UpdatePublication(int adId, AdModelInput request)
        {
            try
            {
                int userId = GetUserId();
                if (userId != 0)
                {
                    AdModels adModel = GetAdModelById(adId);                 
                    CheckAdOwnership(adModel, userId);

                    adModel.Price = request.Price;
                    adModel.Description = request.Description;
                    adModel.Location = request.Location;
                    adModel.CategoryModelId = request.CategoryModelId;
                                                   
                    AdModelOutput adModelOutput = _mapper.Map<AdModelOutput>(adModel);

                    _dataContext.SaveChanges();

                    return adModelOutput;
                }
                throw new Exception("Вы не авторизированный пользователь.");
            }
            catch (Exception ex)
            {
                throw new Exception("Произошла ошибка при изменении объявления.", ex);
            }
        }


        public AdModelOutput UpdatePhotos(int? photosId, PhotoInput? photoInput)
        {
            int userId = GetUserId();
            if (userId != 0)
            {
                AdModels? adModel = _dataContext.AdModels
                    .Include(am => am.Photos)
                    .FirstOrDefault(x => x.Id == photoInput.AdModelId);

                if (adModel != null)
                {
                    // Удаление связи AdModelPhoto
                    AdModelPhoto? adModelPhoto = adModel.Photos.FirstOrDefault(i => i.PhotoId == photosId);
                    if (adModelPhoto != null)
                    {
                        adModel.Photos.Remove(adModelPhoto);
                        _dataContext.SaveChanges();
                    }
                    // Удаление фотографии из таблицы Photos
                    Photos? photoToRemove = _dataContext.Photos.FirstOrDefault(p => p.Id == photosId);
                    string? directory = adModel.PathDirectory;

                    if (photoToRemove != null)
                    {
                        string? pathPhotoToRemove = photoToRemove.PathFile;
                        if (File.Exists(pathPhotoToRemove))
                        {
                            try
                            {
                                File.Delete(pathPhotoToRemove);
                            }
                            catch (Exception)
                            {
                                throw new Exception("Файла не существует");
                            }
                        }
                        _dataContext.Photos.Remove(photoToRemove);
                        _dataContext.SaveChanges();
                    }
                    // Добавление новой фотографии в Photos и на сервер
                    if (photoInput?.File != null)
                    {
                        foreach (IFormFile? file in photoInput.File)
                        {
                            string uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;
                            string fullPath = Path.Combine(directory, uniqueFileName);

                            using (FileStream fileStream = new FileStream(fullPath, FileMode.Create))
                            {
                                file.CopyTo(fileStream);
                            }

                            Photos newPhoto = new Photos
                            {
                                PathFile = fullPath,
                                FileName = file.FileName
                            };
                            _dataContext.Photos.Add(newPhoto);
                            _dataContext.SaveChanges();

                            AdModelPhoto newAdModelPhoto = new AdModelPhoto
                            {
                                AdModelId = adModel.Id,
                                PhotoId = newPhoto.Id
                            };

                            // Добавление связи AdModelPhoto
                            DbSet<AdModelPhoto> adModelPhotos = _dataContext.Set<AdModelPhoto>();
                            adModelPhotos.Add(newAdModelPhoto);                         
                        }
                        _dataContext.SaveChanges();
                        
                    }
                    AdModelOutput adModelOutput = _mapper.Map<AdModelOutput>(adModel);
                    return adModelOutput;

                }
                else
                {
                    throw new Exception("Объявление не найдено.");
                }
            }
            throw new Exception("Вы не являетесь автором объявления.");
        }


        public AdModelOutput LikeAd(int adId)
        {
            int userId = GetUserId();
            if (userId != 0)
            {
                AdModels? adModel = _dataContext.AdModels.Include(am => am.Photos).FirstOrDefault(ad => ad.Id == adId);
                FavoriteModel favoriteModel = _mapper.Map<FavoriteModel>(adModel);

                if (adModel != null && favoriteModel != null)
                {
                    adModel.Likes++;
                    favoriteModel.AdModelId = adId;
                    favoriteModel.UserId = userId;

                    AdModelOutput adModelOutput = _mapper.Map<AdModelOutput>(adModel);

                    _dataContext.Favorites.Add(favoriteModel);
                    _dataContext.SaveChanges();

                    return adModelOutput;
                }
                throw new Exception("Объявления не существует.");
            }
            throw new Exception("Вы не авторизированный пользователь.");
        }

        public List<AdModelOutput> DeleteLikeAd(int adId)
        {
            int userId = GetUserId();
            if (userId != 0)
            {
                AdModels? adModel = _dataContext.AdModels.Include(am => am.Photos).FirstOrDefault(ad => ad.Id == adId);
                FavoriteModel? favoriteModel = _dataContext.Favorites.FirstOrDefault(f => f.AdModelId == adId && f.UserId == userId);
                if (adModel != null && favoriteModel != null)
                {
                    if (favoriteModel.UserId != userId)
                    {
                        throw new Exception("Удаление лайка не возможно.");
                    }
                    else
                    {
                        adModel.Likes--;

                        AdModelOutput adModelOutput = _mapper.Map<AdModelOutput>(adModel);
                        _dataContext.Favorites.Remove(favoriteModel);
                        _dataContext.SaveChanges();


                        List<AdModels?> result = _dataContext.Favorites
                            .Include(ad => ad.AdModel)
                            .ThenInclude(ph => ph.Photos)
                            .Where(x => x.UserId == userId).Select(f => f.AdModel).ToList();


                        List<AdModelOutput> adModelOutputs = _mapper.Map<List<AdModelOutput>>(result);

                        return adModelOutputs;
                    }                    
                }
                throw new Exception("Объявления нет в избранном.");
            }
            throw new Exception("Вы не авторизированный пользователь.");
        }

        public List<AdModelOutput> GetFavoritesModels()
        {
            int userId = GetUserId();
            if (userId != 0)
            {
                List<AdModels?> result = _dataContext.Favorites
                    .Include(ad => ad.AdModel).ThenInclude(add => add!.Photos)
                    .Where(x => x.UserId == userId)
                    .Select(f => f.AdModel).ToList();

                List <AdModelOutput> adModelOutputs = _mapper.Map<List<AdModelOutput>>(result);

                return adModelOutputs;
            }
            return new List<AdModelOutput>();
        }

        public List<AdModelOutput> Filtering(FilterModel filterModel, Sorted sortOrder)
        {
            IEnumerable<AdModels> query = _dataContext.AdModels.Include(am => am.Photos);

            List<AdModels> result = query.ToList();
            List<AdModelOutput> adModelOutputs = _mapper.Map<List<AdModelOutput>>(result);

            if (result != null)
            {
                if (filterModel.MinPrice is not null)
                {
                    adModelOutputs = adModelOutputs.Where(x => x.Price >= filterModel.MinPrice.Value).ToList();
                }

                else
                {
                    adModelOutputs = adModelOutputs.Where(x => x.Price >= 0).ToList();
                }

                if (filterModel.MaxPrice is not null)
                {
                    adModelOutputs = adModelOutputs.Where(x => x.Price <= filterModel.MaxPrice.Value).ToList();
                }

                else 
                {
                    adModelOutputs = adModelOutputs.Where(x => x.Price <= 99999999999).ToList();
                }

                if (filterModel.CategoryModelId is not null)
                {
                    adModelOutputs = adModelOutputs.Where(x => x.CategoryModelId == filterModel.CategoryModelId.Value).ToList();
                }

                if (filterModel.Name is not null)
                {
                    adModelOutputs = adModelOutputs.Where(t => t.Title.Contains(filterModel.Name, StringComparison.OrdinalIgnoreCase)).ToList();
                }

                switch (sortOrder)
                {
                    case Sorted.Ascending:
                        adModelOutputs = adModelOutputs.OrderBy(pr => pr.Price).ToList();
                        break;

                    case Sorted.Descending:
                        adModelOutputs = adModelOutputs.OrderByDescending(pr => pr.Price).ToList();
                        break;

                    default:
                        return adModelOutputs;
                }
                return adModelOutputs;
            }
            throw new Exception("Объявления отсутсвуют");
        }

    }
}

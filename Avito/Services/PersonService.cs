using AutoMapper;
using Avito.Data;
using Avito.Interface;
using Avito.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Npgsql.EntityFrameworkCore.PostgreSQL.Infrastructure.Internal;
using Org.BouncyCastle.Utilities.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using BCryptNet = BCrypt.Net.BCrypt;

namespace Avito.Services
{
    public class PersonService : IPersonService
    {
        private readonly DataContext _dataContext;
        private readonly HttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;
        public PersonService(DataContext dataContext, IMapper mapper, HttpContextAccessor httpContextAccessor)
        {
            _dataContext = dataContext;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
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

        public PersonService(DataContext dataContext, HttpContextAccessor httpContextAccessor)
        {
            _dataContext = dataContext;
            _httpContextAccessor = httpContextAccessor;
        }
        [Authorize(AuthenticationSchemes = "Bearer")]
        [Authorize]
        public PersonOutput UpdatePerson(UserPersonalInformation? userPersonalInformation)
        {
            int userId = GetUserId();
            if (userId != 0)
            {               
                Persons? user = _dataContext.Users.FirstOrDefault(i => i.Id == userId);
                List<Review> reviews = _dataContext.Reviews.Where(i => i.TargetId == userId).ToList();

                if (user != null)
                {

                    if (userPersonalInformation.Name != null)
                    {
                        user.Name = userPersonalInformation.Name;
                    }
                    if (userPersonalInformation.Email != null)
                    {
                        user.Email = userPersonalInformation.Email;
                    }
                    string uniqueFileName = Guid.NewGuid().ToString() + ".png";
                    string pathWithPhoto = "D:\\Avito\\UserPhotos";
                    string fullPath = Path.Combine(pathWithPhoto, uniqueFileName);
                    IFormFile? file = userPersonalInformation.Photo;


                    using (FileStream fileStream = new FileStream(fullPath, FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }

                    user.PathFile = fullPath;
                    user.PhotoId = userId;

                    _dataContext.SaveChanges();

                    PersonOutput personOutput = _mapper.Map<PersonOutput>(user);
                    List<ReviewOutput> reviewOutputs = _mapper.Map<List<ReviewOutput>>(reviews);
                    personOutput.Reviews = reviewOutputs;
                    return personOutput;                                          
                }
                else
                {
                    throw new Exception("Пользователь не найден.");
                }
            }
            else
            {
                throw new Exception("Вы не являетесь владельцем профиля.");
            }
           
        }

        [Authorize(AuthenticationSchemes = "Bearer")]
        [Authorize]
        public bool UpdatePassword(UpdatePersonPassword personPassword)
        {
            int userId = GetUserId();

            Persons? person = _dataContext.Users.FirstOrDefault(x => x.Id == userId);
            bool isValid = BCryptNet.Verify(personPassword.CurrentPassword, person.HashPassword);
            if (isValid)
            {
                string hashPassword = BCryptNet.HashPassword(personPassword.Password);
                person.HashPassword = hashPassword;                
                _dataContext.SaveChanges();

                return true;
            }
            return false;

        }
        public Persons GetPersonByEmail(string Email)
        {
            Persons? result = _dataContext.Users.FirstOrDefault(x => x.Email == Email);
            if (result != null)
            {
                return result;
            }
            throw new Exception("Пользователь не был найден.");
        }

        public ReviewOutput CreateReview(ReviewInput reviewInput)
        {
            int userId = GetUserId();
            Persons? personResult = _dataContext.Users.FirstOrDefault(tp => tp.Id == userId);
            Persons? targetResult = _dataContext.Users.FirstOrDefault(tp => tp.Id == reviewInput.TargetId);
            if (targetResult != null)
            {
                Review review = new Review();
                review.TargetId = reviewInput.TargetId;
                review.PersonId = userId;
                review.PersonName = personResult.Name;
                review.TargetName = targetResult.Name;
                review.Description = reviewInput.Description;
                review.DateTime = DateTime.UtcNow; 
                
                _dataContext.Reviews.Add(review);            
                _dataContext.SaveChanges();

                ReviewOutput reviewOutput = _mapper.Map<ReviewOutput>(review);
                reviewOutput.PersonName = review.PersonName;

                return reviewOutput;
            }
            throw new Exception("Получатель отзыва не найден");
        }
    }
}

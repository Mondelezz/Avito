using Avito.Interface;
using Avito.Models;
using Avito.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Avito.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdController : ControllerBase
    {
        private readonly IAdService _adService;
        public AdController(IAdService adService)
        {
            _adService = adService;
        }

        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPost]
        public ActionResult<AdModelOutput> CreatePublication([FromForm] AdModelInput adModelDto)
        {
            var result = _adService.CreatePublication(adModelDto);
            if (result == null)
            {
                throw new Exception("Не удалось создать объявление");
            }
            return Ok(result);
        }

        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPost("photo")]
        public ActionResult<AdModelOutput> AddPhoto([FromForm] PhotoInput photoDto)
        {
            var result = _adService.AddPhoto(photoDto);
            if (result == null)
            {
                throw new Exception("Не удалось добавить фото");
            }
            return Ok(result);
        }

        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpDelete]
        public ActionResult<AdModelOutput> DeletePublication(int adId)
        {
            var result = _adService.DeletePublication(adId);
            if (result == null)
            {
                throw new Exception("Не удалось удалить объявление");
            }
            return Ok(result);
        }
        [HttpGet("id")]
        public ActionResult GetPublication(int id)
        {
            var result = _adService.GetPublication(id);
            if (result == null)
            {
                throw new Exception("Объявление снято");
            }
            return Ok(result);

        }
        [HttpPut("update")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public ActionResult UpdatePublication(int id, [FromForm] AdModelInput request)
        {
            AdModelOutput? result = _adService.UpdatePublication(id, request);
            if (result == null)
            {
                return BadRequest("Объявление не было изменено.");
            }
            return Ok(result);
        }
        [HttpPut("updatePhotos")]
        [Authorize(AuthenticationSchemes = "Bearer")]

        public ActionResult UpdatePhotos([FromForm] int? photosId, [FromForm] PhotoInput photoInput)
        {
            AdModelOutput result = _adService.UpdatePhotos(photosId, photoInput);

            return Ok(result);

        }
        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPost("{adId}/like")]
        public IActionResult LikeAd(int adId)
        {
            var result = _adService.LikeAd(adId);
            if (result != null)
            {
                return Ok(result);
            }
            return BadRequest("Обновление не было найдено.");
        }
        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPost("{adId}/dellike")]
        public IActionResult DeleteLikeAd(int adId)
        {
            var result = _adService.DeleteLikeAd(adId);
            if (result != null)
            {
                return Ok(result);
            }
            return BadRequest("Обновление не было найдено.");
        }
        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpGet("favorites")]
        public ActionResult GetFavoritesModels()
        {
            var result = _adService.GetFavoritesModels();
            if (result != null)
            {
                return Ok(result);
            }
            return BadRequest("Список избранного пуст.");
        }
        [HttpGet("filtering")]
        public ActionResult Filtering([FromQuery] FilterModel filterModel, Sorted sortOrder)
        {
            List<AdModelOutput> result = _adService.Filtering(filterModel, sortOrder);
            if (result != null)
            {
                return Ok(result);
            }
            return BadRequest("Не удалось отфильтровать объявления");
        }
    }
}

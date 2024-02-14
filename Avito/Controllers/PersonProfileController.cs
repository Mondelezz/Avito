using Avito.Interface;
using Avito.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Avito.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonProfileController : ControllerBase
    {
        private readonly IPersonService _personService;
        public PersonProfileController(IPersonService personService)
        {
            _personService= personService;
        }

        [HttpPut("update")]
        public ActionResult UpdatePerson([FromForm]UserPersonalInformation userPersonalInformation)
        {
            PersonOutput result = _personService.UpdatePerson(userPersonalInformation);
            if (result != null)
            {
                return Ok(result);
            }
            return BadRequest("Не удалось обновить профиль.");
        }
        [HttpPut("password")]
        public ActionResult UpdatePassword([FromForm] UpdatePersonPassword updatePersonPassword)
        {
            bool result = _personService.UpdatePassword(updatePersonPassword);
            if (result)
            {
                return Ok("Пароль успешно изменён.");
            }
            return BadRequest("Не удалось изменить пароль.");

        }
        [HttpPost("review")]
        public ActionResult CreateReview([FromForm] ReviewInput reviewInput)
        {
            ReviewOutput reviewOutput = _personService.CreateReview(reviewInput);
            if (reviewOutput != null)
            {
                return Ok(reviewOutput);
            }
            return BadRequest("Не удалось оставить отзыв.");
        }
    }
}

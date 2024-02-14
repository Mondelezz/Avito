using Avito.Interface;
using Avito.Models;
using FluentEmail.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace Avito.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegistrationController : ControllerBase
    {

        IRegistrationService _registrationService;
        IGenerateCode _generateCode;
        public RegistrationController(IRegistrationService registrationService, IGenerateCode generateCode)       
        {
            _registrationService = registrationService;
            _generateCode = generateCode;
        }
        [HttpPost("reg")]

        public async Task<ActionResult<RegistrationPersonDto>> RegistrationPerson([FromForm] RegistrationPersonDto registrationPersonDto, [FromServices] IFluentEmail fluentEmail)
        {

            int _code = _generateCode.GenerateUnicalCode(registrationPersonDto);

            string emailBody = $@"
        <html>
        <head>
            <style>
                body {{
                    font-family: Arial, sans-serif;
                }}
                .container {{
                    max-width: 600px;
                    margin: 0 auto;
                    padding: 20px;
                    border: 1px solid #ccc;
                    border-radius: 10px;
                }}
                .message {{
                    font-size: 16px;
                }}
            </style>
        </head>
        <body>
            <div class='container'>
                <p class='message'>
                    Здравствуйте,
                    <br /><br />
                    Вы отправили запрос на регистрацию. Для подтверждения Email-почты введите добавочный код: <strong>{_code}</strong>.
                </p>
            </div>
        </body>
        </html>
    ";

            IFluentEmail email = fluentEmail
     .SetFrom("pankov.egor26032005@yandex.ru")
     .To(registrationPersonDto.Email)
     .Subject("Регистрация")
     .Body(emailBody, true);


                await email.SendAsync();
            
            if (ModelState.IsValid)
            {
                _registrationService.RegistrationPerson(registrationPersonDto);
                return Ok("Подтвердите код, который был отправлен на почту. ");
            }
            return BadRequest("Пароли не совпадают!");
        }

        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPost("code")]
        public ActionResult<RegistrationCode> IsValidCode(RegistrationCode registrationCode)
        {
            string enteredCode = registrationCode.Code;

            bool isValid = _generateCode.IsValidCode(enteredCode);
            if (isValid)
            {
                return Ok("Регистрация прошла успешно");            
            }
            return BadRequest("Неверный код подтверждения");
        }
    }
}

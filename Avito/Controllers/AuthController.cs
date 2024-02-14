using Avito.Interface;
using Avito.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Avito.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }
        [AllowAnonymous]
        [HttpPost]
        public ActionResult<AuthPerson> Authorization([FromForm] AuthPerson authPerson)
        {
            var result = _authService.Authentification(authPerson);
            if (result != null)
            {
                return Ok(result);
            }
            return BadRequest("Неверный логин или пароль!");
        }
        [HttpPost("logout")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult Logout()
        {
            if (!User.Identity.IsAuthenticated)
            {
                throw new Exception("Выйти из аккаунта невозможно.");
            }
            return Ok("Вы успешно вышли из аккаунта.");
        }
        [AllowAnonymous]
        [HttpGet("/signin-google")] 
        public IActionResult ExternalLogin(string returnUrl = null)
        {
            return Challenge(new AuthenticationProperties { RedirectUri = returnUrl });
        }
    }
}

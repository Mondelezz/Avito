using Avito.Data;
using Avito.Interface;
using Avito.Models;
using Avito.Options;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using BCryptNet = BCrypt.Net.BCrypt;

namespace Avito.Services
{
    public class AuthorizationService : IAuthService
    {
        private readonly IPersonService _personService;

        public AuthorizationService(IPersonService personService)
        {
            _personService = personService;
        }

        public string Authentification(AuthPerson authPerson)
        {
            Persons result = _personService.GetPersonByEmail(authPerson.Email);
            bool validatePassword = BCryptNet.Verify(authPerson.Password, result.HashPassword);
            if (validatePassword && result != null)
            {
                var identity = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimsIdentity.DefaultNameClaimType, authPerson.Password),
                    new Claim(ClaimsIdentity.DefaultNameClaimType, authPerson.Email),
                    new Claim("Id", result.Id.ToString())
                });
                DateTime now = DateTime.UtcNow;
                var jwt = new JwtSecurityToken(
                    issuer: AuthOptions.ISSUER,
                    audience: AuthOptions.AUDIENCE,
                    notBefore: now,
                    claims: identity.Claims,
                    expires: now.Add(TimeSpan.FromMinutes(AuthOptions.KEYLIFE)),
                    signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
                var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

                return encodedJwt.ToString();
            }
            else
            {
                throw new Exception("Пользователь не найден. Неверный логин или пароль!");
            }
        }
    }
}

using Avito.Data;
using Avito.Interface;
using Avito.Models;
using AutoMapper;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Avito.Services
{
    public class RegistrationService : IRegistrationService, IGenerateCode
    {
        private readonly DataContext _dataContext;
        private readonly IMapper _mapper;
        private readonly HttpContextAccessor _httpContextAccessor;
        public RegistrationService(DataContext dataContext, IMapper mapper, HttpContextAccessor httpContextAccessor)
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

        public RegistrationPersonDto RegistrationPerson(RegistrationPersonDto registrationPersonDto)
        {
            List<ValidationResult> validationResults = new List<ValidationResult>();
            bool isValid = Validator.TryValidateObject(registrationPersonDto, new ValidationContext(registrationPersonDto), validationResults, true);

            if (isValid)
            {
                string hashPassword = BCrypt.Net.BCrypt.HashPassword(registrationPersonDto.Password);
                Persons person = _mapper.Map<Persons>(registrationPersonDto);
                person.HashPassword = hashPassword;
                person.RegistrationDate = DateTime.UtcNow;
                _dataContext.Add(person);
                _dataContext.SaveChanges();

                return registrationPersonDto;
            }
            throw new Exception(validationResults.ToString());
        }
        public int GenerateUnicalCode(RegistrationPersonDto registrationPersonDto)
        {
            Random code = new Random();
            int _code = code.Next(1000, 9999);
            string email = registrationPersonDto.Email;

            CodeDb codeDb = new CodeDb();
            codeDb.Code = _code;
            codeDb.Email = email;

            _dataContext.Codes.Add(codeDb);
            _dataContext.SaveChanges();

            return _code;
        }
        public bool IsValidCode(string enteredCode)
        {
            int userId = GetUserId();

            Persons person = _dataContext.Users.FirstOrDefault(x => x.Id == userId);
            if (int.TryParse(enteredCode, out int enteredCodeInt))
            {
                CodeDb code = _dataContext.Codes.FirstOrDefault(c => c.Code == enteredCodeInt && c.Email == person.Email);
                int originalCode = code.Code;

                if (code != null)
                {
                    return enteredCodeInt == originalCode;
                }
                return false;
            }
            throw new Exception("Не удалось выполнить unbox типов");
        }       
    }
}

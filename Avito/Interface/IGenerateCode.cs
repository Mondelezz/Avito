using Avito.Models;

namespace Avito.Interface
{
    public interface IGenerateCode
    {
        public int GenerateUnicalCode(RegistrationPersonDto registrationPersonDto);
        public bool IsValidCode(string enteredCode);
    }
}

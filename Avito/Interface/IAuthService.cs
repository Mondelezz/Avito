using Avito.Models;

namespace Avito.Interface
{
    public interface IAuthService
    {
        public string Authentification(AuthPerson authPerson);
    }
}
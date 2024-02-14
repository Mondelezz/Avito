using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Avito.Options
{
    public class AuthOptions
    {
        public const string AUDIENCE = "MyClient";
        public const string ISSUER = "MyApp";
        public const string KEY = "TEPERVCEBUDETNORM";
        public const int KEYLIFE = 1000;
        public static SymmetricSecurityKey GetSymmetricSecurityKey()
        {
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(KEY));
        }
    }
}

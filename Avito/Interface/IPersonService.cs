using Avito.Models;

namespace Avito.Interface
{
    public interface IPersonService
    {
        public Persons GetPersonByEmail(string Email);
        public PersonOutput UpdatePerson(UserPersonalInformation userPersonalInformation);
        public bool UpdatePassword(UpdatePersonPassword personPassword);
        public ReviewOutput CreateReview(ReviewInput reviewInput);
    }
}

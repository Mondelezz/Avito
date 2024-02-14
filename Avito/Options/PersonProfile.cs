using AutoMapper;
using Avito.Models;
using Microsoft.EntityFrameworkCore;

namespace Avito.Options
{
    public class PersonProfile : Profile
    {
        public PersonProfile()
        {
            CreateMap<RegistrationPersonDto, Persons>();
            CreateMap<AdModelInput, AdModels>();
            CreateMap<AdModels,FavoriteModel>();
            CreateMap<AdModelPhotoInput, AdModelPhoto>();
            CreateMap<AdModels, AdModelPhoto>();
            CreateMap<AdModels, AdModelOutput>();
            CreateMap<AdModelPhoto, AdModelPhotoOutput>();
            CreateMap<AdModels, AdModelPhotoOutput>();
            CreateMap<FavoriteModel, AdModelOutput>();
            CreateMap<AdModels, Photos>();
            CreateMap<Persons, PersonOutput>();
            CreateMap<Review, ReviewOutput>();
            CreateMap<ReviewOutput, Review>();
        }
    }
}

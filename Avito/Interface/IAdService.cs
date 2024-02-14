using Avito.Models;
using Avito.Services;

namespace Avito.Interface
{
    public interface IAdService
    {
        public int CreatePublication(AdModelInput adModelInput);
        public AdModelOutput DeletePublication(int adId);
        public AdModelOutput UpdatePublication(int adId, AdModelInput request);
        public AdModelOutput GetPublication(int adId);
        public AdModelOutput LikeAd(int adId);
        public List<AdModelOutput> DeleteLikeAd(int adId);
        public List<AdModelOutput> GetFavoritesModels();
        public List<AdModelOutput> Filtering(FilterModel priceFilterModel, Sorted sortOrder);
        public AdModelOutput AddPhoto(PhotoInput photoDto);
        public AdModelOutput UpdatePhotos(int? photosId, PhotoInput photoInput);
    }
}

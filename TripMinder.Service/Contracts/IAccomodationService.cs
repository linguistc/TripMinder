using TripMinder.Data.Entities;

namespace TripMinder.Service.Contracts
{
    public interface IAccomodationService
    {
        public Task<List<Accomodation>> GetAccomodationsListAsync();
        public Task<List<Accomodation>> GetAccomodationsListByZoneIdAsync(int zoneId, CancellationToken cancellationToken = default);
        public Task<List<Accomodation>> GetAccomodationsListByGovernorateIdAsync(int governorateId, CancellationToken cancellationToken = default);
        public Task<List<Accomodation>> GetAccomodationsListByTypeIdAsync(int TypeId, CancellationToken cancellationToken = default);
        public Task<List<Accomodation>> GetAccomodationListByClassIdAsync(int classId, CancellationToken cancellationToken = default);
        
        public Task<List<Accomodation>> GetAccomodationListByRatingAsync(double rating, CancellationToken cancellationToken = default);
        public Task<List<Accomodation>> GetAccommodationListMoreThanPriceAsync(double price, CancellationToken cancellationToken = default);
        public Task<List<Accomodation>> GetAccommodationListLessThanPriceAsync(double price, CancellationToken cancellationToken = default);
        
        public Task<List<Accomodation>> GetAccomodationListByNumberOfBedsAsync(short numberOfBeds, CancellationToken cancellationToken = default);
        
        public Task<List<Accomodation>> GetAccomodationListByNumberOfAdultsAsync(short numberOfRooms, CancellationToken cancellationToken = default);
        
        public Task<Accomodation> GetAccomodationByIdWithIncludeAsync(int id);
        public Task<Accomodation> GetAccomodationByIdAsync(int id);
        public Task<string> CreateAsync(Accomodation newAccomodation);
        public Task<string> UpdateAsync(Accomodation accomodation);
        public Task<string> DeleteAsync(Accomodation accomodation);

        public Task<bool> IsNameArExist(string nameAr);
        public Task<bool> IsNameEnExist(string nameEn);
        public Task<bool> IsNameArExistExcludeSelf(string nameAr, int id);
        public Task<bool> IsNameEnExistExcludeSelf(string nameEn, int id);

        
        
    }
}

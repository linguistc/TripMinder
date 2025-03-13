using TripMinder.Data.Entities;

namespace TripMinder.Service.Contracts
{
    public interface IAccomodationService
    {
        public Task<List<Accomodation>> GetAccomodationsListAsync();
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

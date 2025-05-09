﻿using TripMinder.Data.Entities;

namespace TripMinder.Service.Contracts
{
    public interface IAccomodationService
    {
        public Task<List<Accomodation>> GetAccomodationsListAsync();
        public Task<double?> GetMinimumPriceAsync(CancellationToken cancellationToken = default);
        public Task<List<Accomodation>> GetAccomodationsListByZoneIdAsync(int zoneId, CancellationToken cancellationToken = default);
        public Task<List<Accomodation>> GetAccomodationsListByGovernorateIdAsync(int governorateId, CancellationToken cancellationToken = default);
        public Task<List<Accomodation>> GetAccomodationsListByTypeIdAsync(int TypeId, CancellationToken cancellationToken = default);
        public Task<List<Accomodation>> GetAccomodationsListByClassIdAsync(int classId, CancellationToken cancellationToken = default);
        
        public Task<List<Accomodation>> GetAccomodationsListByRatingAsync(double rating, CancellationToken cancellationToken = default);
        public Task<List<Accomodation>> GetAccomodationsListMoreThanPriceAsync(double price, CancellationToken cancellationToken = default);
        public Task<List<Accomodation>> GetAccomodationsListLessThanPriceAsync(double price, CancellationToken cancellationToken = default);
        
        public Task<List<Accomodation>> GetAccomodationsListByNumberOfBedsAsync(short numberOfBeds, CancellationToken cancellationToken = default);
        
        public Task<List<Accomodation>> GetAccomodationsListByNumberOfAdultsAsync(short numberOfRooms, CancellationToken cancellationToken = default);
        
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

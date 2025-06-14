using Microsoft.EntityFrameworkCore;
using TripMinder.Data.Entities;
using TripMinder.Infrastructure.Contracts;
using TripMinder.Infrastructure.Extentions;
using TripMinder.Service.Contracts;

namespace TripMinder.Service.Implementations
{
    public class AccomodationService : IAccomodationService
    {

        #region Fields
        private readonly IAccomodationRepository _repository;

        #endregion

        #region Constructors
        public AccomodationService(IAccomodationRepository repository)
        {
            this._repository = repository;
        }
        #endregion

        #region Functions
        public async Task<List<Accomodation>> GetAccomodationsListAsync()
        {
            return await this._repository.GetAccomodationsListAsync();
        }

        public async Task<double?> GetMinimumPriceAsync(CancellationToken cancellationToken = default)
        {
            return await this._repository.GetMinimumPriceAsync(cancellationToken);
        }

        public async Task<List<Accomodation>> GetAccomodationsListByZoneIdAsync(int zoneId, CancellationToken cancellationToken = default)
        {
            return await this._repository.GetAccomodationsListByZoneIdAsync(zoneId, cancellationToken);
        }

        public async Task<List<Accomodation>> GetAccomodationsListByGovernorateIdAsync(int governorateId, CancellationToken cancellationToken = default)
        {
            return await this._repository.GetAccomodationsListByGovernorateIdAsync(governorateId, cancellationToken);
        }

        public async Task<List<Accomodation>> GetAccomodationsListByTypeIdAsync(int TypeId, CancellationToken cancellationToken = default)
        {
            return await this._repository.GetAccomodationsListByTypeIdAsync(TypeId, cancellationToken);
        }

        public async Task<List<Accomodation>> GetAccomodationsListByClassIdAsync(int classId, CancellationToken cancellationToken = default)
        {
            
            return await this._repository.GetAccomodationsListByClassIdAsync(classId, cancellationToken);
            
        }

        public async Task<List<Accomodation>> GetAccomodationsListByRatingAsync(double rating, CancellationToken cancellationToken = default)
        {
            return await this._repository.GetAccomodationsListByRatingAsync(rating, cancellationToken);
        }

        public async Task<List<Accomodation>> GetAccomodationsListMoreThanPriceAsync(double price, CancellationToken cancellationToken = default)
        {
            return await this._repository.GetAccomodationsListMoreThanPriceAsync(price, cancellationToken);
        }

        public async Task<List<Accomodation>> GetAccomodationsListLessThanPriceAsync(double price, CancellationToken cancellationToken = default)
        {
            return await this._repository.GetAccomodationsListLessThanPriceAsync(price, cancellationToken);
        }

        public async Task<List<Accomodation>> GetAccomodationsListByNumberOfBedsAsync(short numberOfBeds, CancellationToken cancellationToken = default)
        {
            return await this._repository.GetAccomodationsListByNumOfBedsAsync(numberOfBeds, cancellationToken);
        }

        public async Task<List<Accomodation>> GetAccomodationsListByNumberOfAdultsAsync(short numberOfRooms, CancellationToken cancellationToken = default)
        {
            return await this._repository.GetAccomodationsListByNumOfAdultsAsync(numberOfRooms, cancellationToken);
        }

        public async Task<Accomodation> GetAccomodationByIdWithIncludeAsync(int id)
        {
            var accomodation = this._repository.GetTableNoTracking()
                                        .Include(a => a.PlaceType)
                                        .Include(a => a.Class)
                                        .Include(a => a.Zone)
                                        .Include(a => a.Zone.Governorate)
                                        .FirstOrDefault(a => a.Id == id);

            return accomodation;
        }

        public async Task<Accomodation> GetAccomodationByIdAsync(int id)
        {
            var accomodation = await this._repository.GetByIdAsync(id);
            return accomodation;
        }

        public async Task<string> CreateAsync(Accomodation newAccomodation)
        {
            await this._repository.CreateAsync(newAccomodation);
            return "Created";
        }

        public Task<bool> IsNameArExist(string nameAr)
        {
            throw new NotImplementedException();
        }

        public Task<bool> IsNameEnExist(string nameEn)
        {
            throw new NotImplementedException();
        }

        public Task<bool> IsNameArExistExcludeSelf(string nameAr, int id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> IsNameEnExistExcludeSelf(string nameEn, int id)
        {
            throw new NotImplementedException();
        }

        public async Task<string> UpdateAsync(Accomodation accomodation)
        {
            await this._repository.UpdateAsync(accomodation);
            return "Updated";
        }

        public async Task<string> DeleteAsync(Accomodation accomodation)
        {
            var trans = this._repository.BeginTransaction();

            try
            {
                await this._repository.DeleteAsync(accomodation);
                await trans.CommitAsync();
                return "Deleted";
            }
            catch
            {
                await trans.RollbackAsync();
                return "Failed";
            }
        }

        #endregion

    }
}

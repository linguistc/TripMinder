using Microsoft.EntityFrameworkCore;
using TripMinder.Data.Entities;
using TripMinder.Infrastructure.Contracts;
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
        public Task<List<Accomodation>> GetAccomodationsListAsync()
        {
            return this._repository.GetAccomodationsListAsync();
        }

        public Task<List<Accomodation>> GetAccomodationsListByZoneIdAsync(int zoneId, CancellationToken cancellationToken = default)
        {
            return this._repository.GetAccomodationsListByZoneIdAsync(zoneId, cancellationToken);
        }

        public Task<List<Accomodation>> GetAccomodationsListByGovernorateIdAsync(int governorateId, CancellationToken cancellationToken = default)
        {
            return this._repository.GetAccomodationsListByGovernorateIdAsync(governorateId, cancellationToken);
        }
        
        public async Task<Accomodation> GetAccomodationByIdWithIncludeAsync(int id)
        {
            var accomodation = this._repository.GetTableNoTracking()
                                        .Include(a => a.PlaceType)
                                        .Include(a => a.Class)
                                        .Include(a => a.Zone)
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

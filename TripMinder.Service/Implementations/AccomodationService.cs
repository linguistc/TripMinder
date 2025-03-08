using Microsoft.EntityFrameworkCore;
using TripMinder.Data.Entities;
using TripMinder.Infrastructure.Contracts;
using TripMinder.Service.Contracts;

namespace TripMinder.Service.Implementations
{
    public class AccomodationService : IAccomodationService
    {

        #region Fields
        private readonly IAccomodationRepository repository;

        #endregion

        #region Constructors
        public AccomodationService(IAccomodationRepository repository)
        {
            this.repository = repository;
        }
        #endregion

        #region Functions
        public Task<List<Accomodation>> GetAllAccomodationsAsync()
        {
            return this.repository.GetAllAccomodationsAsync();
        }

        public async Task<Accomodation> GetAccomodationByIdWithIncludeAsync(int id)
        {
            var accomodation = this.repository.GetTableNoTracking()
                                        .Include(a => a.Description)
                                        .Include(a => a.PlaceCategory)
                                        .Include(a => a.Class)
                                        .Include(a => a.Zone)
                                        .Include(a => a.Location)
                                        .FirstOrDefault(a => a.Id == id);

            return accomodation;
        }

        public async Task<Accomodation> GetAccomodationByIdAsync(int id)
        {
            var accomodation = await this.repository.GetByIdAsync(id);
            return accomodation;
        }

        public async Task<string> CreateAsync(Accomodation newAccomodation)
        {
            await this.repository.CreateAsync(newAccomodation);
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
            await this.repository.UpdateAsync(accomodation);
            return "Updated";
        }

        public async Task<string> DeleteAsync(Accomodation accomodation)
        {
            var trans = this.repository.BeginTransaction();

            try
            {
                await this.repository.DeleteAsync(accomodation);
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

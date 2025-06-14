using Microsoft.EntityFrameworkCore;
using TripMinder.Data.Entities;
using TripMinder.Infrastructure.Contracts;
using TripMinder.Infrastructure.Extentions;
using TripMinder.Service.Contracts;

namespace TripMinder.Service.Implementations
{
    public class EntertainmentService : IEntertainmentService
    {
        #region Fields
        private readonly IEntertainmentRepository _repository;

        #endregion

        #region Constructors
        public EntertainmentService(IEntertainmentRepository repository)
        {
            this._repository = repository;
        }

        #endregion

        #region Functions
        public async Task<double?> GetMinimumPriceAsync(CancellationToken cancellationToken = default)
        {
            return await this._repository.GetMinimumPriceAsync(cancellationToken);
        }
        public async Task<List<Entertainment>> GetEntertainmentsListAsync()
        {
            return await this._repository.GetEntertainmentsListAsync();
        }

        public async Task<List<Entertainment>> GetEntertainmentsListByZoneIdAsync(int zoneId, CancellationToken cancellationToken = default)
        {
            return await this._repository.GetEntertainmentsListByZoneIdAsync(zoneId, cancellationToken);
        }

        public async Task<List<Entertainment>> GetEntertainmentsListByGovernorateIdAsync(int governorateId, CancellationToken cancellationToken = default)
        {
            return await this._repository.GetEntertainmentsListByGovernorateIdAsync(governorateId, cancellationToken);

        }

        public async Task<Entertainment> GetEntertainmentByIdWithIncludeAsync(int id)
        {
            var entertainment = await this._repository.GetTableNoTracking()
                                            .Include(e => e.EntertainmentType)
                                            .Include(e => e.PlaceType)
                                            .Include(e => e.Class)
                                            .Include(e => e.Zone)
                                            .Include(e => e.Zone.Governorate)
                                            .FirstOrDefaultAsync(e => e.Id == id);

            return entertainment;
        }

        public async Task<List<Entertainment>> GetEntertainmentsListByClassIdAsync(int classId, CancellationToken cancellationToken = default)
        {
            return await this._repository.GetEntertainmentsListByClassIdAsync(classId, cancellationToken);
        }

        public async Task<List<Entertainment>> GetEntertainmentsListByTypeIdAsync(int TypeId, CancellationToken cancellationToken = default)
        {
            return await this._repository.GetEntertainmentsListByTypeIdAsync(TypeId, cancellationToken);
        }

        public async Task<List<Entertainment>> GetEntertainmentsListMoreThanPriceAsync(double price, CancellationToken cancellationToken = default)
        {
            return await this._repository.GetEntertainmentsListMoreThanPriceAsync(price, cancellationToken);
        }

        public async Task<List<Entertainment>> GetEntertainmentsListLessThanPriceAsync(double price, CancellationToken cancellationToken = default)
        {
            return await this._repository.GetEntertainmentsListLessThanPriceAsync(price, cancellationToken);
        }

        public async Task<List<Entertainment>> GetEntertainmentsListByRatingAsync(double rating, CancellationToken cancellationToken = default)
        {
            return await this._repository.GetEntertainmentsListByRatingAsync(rating, cancellationToken);
        }

        public async Task<Entertainment> GetEntertainmentByIdAsync(int id)
        {
            var entertainment = await this._repository.GetByIdAsync(id);

            return entertainment;
        }

        public async Task<string> CreateAsync(Entertainment newEntertainment)
        {
            await this._repository.CreateAsync(newEntertainment);
            return "Created";
        }

        public async Task<bool> IsNameArExist(string nameAr)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> IsNameEnExist(string nameEn)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> IsNameArExistExcludeSelf(string nameAr, int id)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> IsNameEnExistExcludeSelf(string nameEn, int id)
        {
            throw new NotImplementedException();
        }

        public async Task<string> UpdateAsync(Entertainment entertainment)
        {
            await this._repository.UpdateAsync(entertainment);
            return "Updated";
        }

        public async Task<string> DeleteAsync(Entertainment entertainment)
        {
            var trans = this._repository.BeginTransaction();

            try
            {
                await this._repository.DeleteAsync(entertainment);
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

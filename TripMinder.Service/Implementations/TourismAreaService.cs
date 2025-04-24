using Microsoft.EntityFrameworkCore;
using TripMinder.Data.Entities;
using TripMinder.Infrastructure.Contracts;
using TripMinder.Service.Contracts;

namespace TripMinder.Service.Implementations
{
    public class TourismAreaService : ITourismAreaService
    {
        #region Fields
        private readonly ITourismAreaRepository repository;

        #endregion

        #region Constructors
        public TourismAreaService(ITourismAreaRepository repository)
        {
            this.repository = repository;
        }

        #endregion

        #region Functions
        public async Task<List<TourismArea>> GetTourismAreasListAsync()
        {
            return await this.repository.GetAllTourismAreasAsync();
        }
        
        public async Task<List<TourismArea>> GetTourismAreasListByZoneIdAsync(int zoneId, CancellationToken cancellationToken = default)
        {
            return await this.repository.GetTourismAreasListByZoneIdAsync(zoneId, cancellationToken);
        }

        public async Task<List<TourismArea>> GetTourismAreasListByGovernorateIdAsync(int governorateId, CancellationToken cancellationToken = default)
        {
            return await this.repository.GetTourismAreasListByZoneIdAsync(governorateId, cancellationToken);
        }

        public async Task<TourismArea> GetTourismAreaByIdWithIncludeAsync(int id)
        {
            var tourism = this.repository.GetTableNoTracking()
                                    .Include(t => t.TourismType)
                                    .Include(t => t.PlaceType)
                                    .Include(t => t.Class)
                                    .Include(t => t.Zone)
                                    .FirstOrDefault(t => t.Id == id);

            return tourism;
        }

        public async Task<List<TourismArea>> GetTourismAreasByTypeIdAsync(int TypeId, CancellationToken cancellationToken = default)
        {
            return await this.repository.GetTourismAreasListByTypeIdAsync(TypeId, cancellationToken);
        }

        public async Task<List<TourismArea>> GetTourismAreasListByClassIdAsync(int classId, CancellationToken cancellationToken = default)
        {
            return await this.repository.GetTourismAreasListByClassIdAsync(classId, cancellationToken);
        }

        public async Task<List<TourismArea>> GetTourismAreasByRatingAsync(double rating, CancellationToken cancellationToken = default)
        {
            return await this.repository.GetTourismAreasListByRatingAsync(rating, cancellationToken);
        }

        public async Task<List<TourismArea>> GetTourismAreasMoreThanPriceAsync(double price, CancellationToken cancellationToken = default)
        {
            return await this.repository.GetTourismAreasListMoreThanPriceAsync(price, cancellationToken);
        }

        public async Task<List<TourismArea>> GetTourismAreasLessThanPriceAsync(double price, CancellationToken cancellationToken = default)
        {
            return await this.repository.GetTourismAreasListLessThanPriceAsync(price, cancellationToken);
        }

        public async Task<TourismArea> GetTourismAreaByIdAsync(int id)
        {
            var tourismArea = await this.repository.GetByIdAsync(id);

            return tourismArea;
        }

        public async Task<string> CreateAsync(TourismArea newTourismArea)
        {
            await this.repository.CreateAsync(newTourismArea);
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

        public async Task<string> UpdateAsync(TourismArea tourismArea)
        {
            await this.repository.UpdateAsync(tourismArea);
            return "Updated";
        }

        public async Task<string> DeleteAsync(TourismArea tourismArea)
        {
            var trans = this.repository.BeginTransaction();

            try
            {
                await this.repository.DeleteAsync(tourismArea);
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

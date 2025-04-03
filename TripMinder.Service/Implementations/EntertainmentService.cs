using Microsoft.EntityFrameworkCore;
using TripMinder.Data.Entities;
using TripMinder.Infrastructure.Contracts;
using TripMinder.Service.Contracts;

namespace TripMinder.Service.Implementations
{
    public class EntertainmentService : IEntertainmentService
    {
        #region Fields
        private readonly IEntertainmentRepository repository;

        #endregion

        #region Constructors
        public EntertainmentService(IEntertainmentRepository repository)
        {
            this.repository = repository;
        }

        #endregion

        #region Functions
        public async Task<List<Entertainment>> GetAllEntertainmentsAsync()
        {
            return await this.repository.GetAllEntertainmentsAsync();
        }

        public async Task<Entertainment> GetEntertainmentByIdWithIncludeAsync(int id)
        {
            var entertainment = this.repository.GetTableNoTracking()
                                            .Include(e => e.EntertainmentType)
                                            .Include(e => e.PlaceType)
                                            .Include(e => e.Class)
                                            .Include(e => e.Zone)
                                            .FirstOrDefault(e => e.Id == id);

            return entertainment;
        }

        public async Task<Entertainment> GetEntertainmentByIdAsync(int id)
        {
            var entertainment = await this.repository.GetByIdAsync(id);

            return entertainment;
        }

        public async Task<string> CreateAsync(Entertainment newEntertainment)
        {
            await this.repository.CreateAsync(newEntertainment);
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
            await this.repository.UpdateAsync(entertainment);
            return "Updated";
        }

        public async Task<string> DeleteAsync(Entertainment entertainment)
        {
            var trans = this.repository.BeginTransaction();

            try
            {
                await this.repository.DeleteAsync(entertainment);
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

using CompanyDealer.DAL.Data;
using CompanyDealer.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompanyDealer.DAL.Repository.RestockRepo
{
    public class RestockRequestRepository : IRestockRequestRepository
    {
        private readonly ApplicationDbContext _db;
        public RestockRequestRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<List<RestockRequest>> GetAllAsync()
        {
            return await _db.RestockRequests.AsNoTracking().ToListAsync();
        }

        public async Task<RestockRequest?> GetByIdAsync(Guid id)
        {
            return await _db.RestockRequests.FindAsync(id);
        }

        public async Task CreateAsync(RestockRequest entity)
        {
            _db.RestockRequests.Add(entity);
            await _db.SaveChangesAsync();
        }

        public async Task UpdateAsync(RestockRequest entity)
        {
            _db.RestockRequests.Update(entity);
            await _db.SaveChangesAsync();
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var entity = await _db.RestockRequests.FindAsync(id);
            if (entity == null) return false;
            _db.RestockRequests.Remove(entity);
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<List<RestockRequest>> GetByDealerIdAsync(Guid dealerId)
        {
            return await _db.RestockRequests
                .Where(r => r.DealerId == dealerId)
                .AsNoTracking()
                .ToListAsync();
        }
        public async Task<List<RestockRequest>> GetRequestsForAcceptenceLevelAsync(string acceptenceLevel)
        {
            return await _db.RestockRequests
                .Where(r => r.AcceptenceLevel == acceptenceLevel)
                .AsNoTracking()
                .ToListAsync();
        }
    }
}

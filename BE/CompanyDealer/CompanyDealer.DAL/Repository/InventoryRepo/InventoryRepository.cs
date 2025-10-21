using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CompanyDealer.DAL.Data;
using CompanyDealer.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace CompanyDealer.DAL.Repository.InventoryRepo
{
    public class InventoryRepository : GenericRepository<Inventory>, IInventoryRepository
    {
        private readonly ApplicationDbContext _db;

        public InventoryRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public async Task<List<Inventory>> GetAllAsync()
        {
            return await _db.Inventories
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Inventory?> GetByIdAsync(Guid id)
        {
            return await _db.Inventories
                .Include(i => i.InventoryVehicles)
                .ThenInclude(iv => iv.Vehicle)
                .AsNoTracking()
                .FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task<Inventory> CreateAsync(Inventory entity)
        {
            _db.Inventories.Add(entity);
            await _db.SaveChangesAsync();
            return entity;
        }

        public async Task<Inventory?> UpdateAsync(Inventory entity)
        {
            var existing = await _db.Inventories.FindAsync(entity.Id);
            if (existing == null) return null;

            existing.LastUpdated = entity.LastUpdated;
            existing.DealerId = entity.DealerId;
            // note: inventory-vehicle relation updates should be handled explicitly in a dedicated method

            _db.Inventories.Update(existing);
            await _db.SaveChangesAsync();
            return existing;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var existing = await _db.Inventories.FindAsync(id);
            if (existing == null) return false;

            _db.Inventories.Remove(existing);
            await _db.SaveChangesAsync();
            return true;
        }
    }
}
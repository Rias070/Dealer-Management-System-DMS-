using System;
using System.Collections.Generic;
using System.Linq;
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

        public async Task<List<Vehicle>> GetVehicleInInventory(Guid dealerId)
        {
            var vehicles = await _db.Inventories
                .Include(i => i.InventoryVehicles)
                .ThenInclude(iv => iv.Vehicle)
                .Where(i => i.DealerId == dealerId)
                .SelectMany(i => i.InventoryVehicles.Select(iv => iv.Vehicle))
                .Distinct()
                .ToListAsync();

            return vehicles;
        }

        public async Task<bool> ReduceQuantityIfEnough(Guid vehicleId, int quantity)
        {
            var inventoryId = await GetIdInventory("Company");
            var inventoryVehicle = await _db.InventoryVehicles
                .Include(iv => iv.Inventory)
                .FirstOrDefaultAsync(iv =>
                    iv.VehicleId == vehicleId &&
                    iv.InventoryId == inventoryId);

            if (inventoryVehicle == null || inventoryVehicle.Quantity < quantity)
                return false;

            inventoryVehicle.Quantity -= quantity;

            await _db.SaveChangesAsync();
            return true;
        }
        public async Task<bool> IncreaseQuantity(Guid vehicleId, int quantity, Guid dealerId)
        {
            var inventoryId = await GetIdInventory(dealerId);
            if (!inventoryId.HasValue) return false;

            var inventoryVehicle = await _db.InventoryVehicles
                .FirstOrDefaultAsync(iv =>
                    iv.VehicleId == vehicleId &&
                    iv.InventoryId == inventoryId.Value);

            if (inventoryVehicle == null)
            {
                inventoryVehicle = new InventoryVehicle
                {
                    InventoryId = inventoryId.Value,
                    VehicleId = vehicleId,
                    Quantity = quantity
                };
                _db.InventoryVehicles.Add(inventoryVehicle);
            }
            else
            {
                inventoryVehicle.Quantity += quantity;
                _db.InventoryVehicles.Update(inventoryVehicle);
            }

            await _db.SaveChangesAsync();
            return true;
        }
        private async Task<Guid?>  GetIdInventory(Guid dealerId)
        {
            var inventory = await _db.Inventories
                .AsNoTracking()
                .FirstOrDefaultAsync(i => i.DealerId == dealerId);
            return inventory?.Id;
        }
        private async Task<Guid?> GetIdInventory(string DealerName)
        {
            var dealer = await _db.Dealers
                .AsNoTracking()
                .FirstOrDefaultAsync(i => i.Name == DealerName);
            var inventory = await _db.Inventories
                .AsNoTracking()
                .FirstOrDefaultAsync(i => i.DealerId == dealer.Id);
            return inventory?.Id;
        }

        public async Task<List<object>> GetVehicleWithQuantityByDealer(Guid dealerId)
        {
            var inventoryVehicles = await _db.InventoryVehicles
                .Include(iv => iv.Vehicle)
                .Where(iv => iv.Inventory.DealerId == dealerId)
                .ToListAsync();

            var result = inventoryVehicles.Select(iv => new
            {
                Vehicle = iv.Vehicle,
                Quantity = iv.Quantity
            }).ToList<object>();

            return result;
        }
    }
}
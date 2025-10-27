using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CompanyDealer.DAL.Models;

namespace CompanyDealer.DAL.Repository.InventoryRepo
{
    public interface IInventoryRepository
    {
        Task<List<Inventory>> GetAllAsync();
        Task<Inventory?> GetByIdAsync(Guid id);
        Task<Inventory> CreateAsync(Inventory entity);
        Task<Inventory?> UpdateAsync(Inventory entity);
        Task<bool> DeleteAsync(Guid id);
    
        Task<List<Vehicle>> GetVehicleInInventory(Guid dealerId);
        Task<List<object>> GetVehicleWithQuantityByDealer(Guid dealerId);
        Task<bool> ReduceQuantityIfEnough(Guid vehicleId, int quantity);
    }
}
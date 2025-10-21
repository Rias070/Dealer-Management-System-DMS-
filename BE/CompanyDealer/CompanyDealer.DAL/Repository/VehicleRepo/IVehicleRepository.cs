using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CompanyDealer.DAL.Models;

namespace CompanyDealer.DAL.Repository.VehicleRepo
{
    /// <summary>
    /// Repository abstraction for Vehicle entity.
    /// </summary>
    public interface IVehicleRepository : IGenericRepository<Vehicle>
    {
        Task<Vehicle> CreateAsync(Vehicle vehicle);

        Task<Vehicle?> GetByIdAsync(Guid id);

        Task<List<Vehicle>> GetAllAsync();

        Task<Vehicle?> UpdateAsync(Vehicle vehicle);

        Task<bool> DeleteAsync(Guid id);
    }
}
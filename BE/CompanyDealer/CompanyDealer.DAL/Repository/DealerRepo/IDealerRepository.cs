using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CompanyDealer.DAL.Models;

namespace CompanyDealer.DAL.Repository.DealerRepo
{
    /// <summary>
    /// Repository abstraction for Dealer entity.
    /// </summary>
    public interface IDealerRepository : IGenericRepository<Dealer>
    {
        Task<Dealer> CreateAsync(Dealer dealer);
        Task<Dealer?> GetByIdAsync(Guid id);
        Task<List<Dealer>> GetAllAsync();
        Task<List<Dealer>> GetActiveDealersAsync();
        Task<Dealer?> UpdateAsync(Dealer dealer);
        Task<bool> DeleteAsync(Guid id);
        Task<bool> ExistsAsync(Guid id);
    }
}

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CompanyDealer.DAL.Models;

namespace CompanyDealer.DAL.Repository.SaleContractRepo
{
    public interface ISaleContractRepository
    {
        Task<IEnumerable<SaleContract>> GetAllAsync();
        Task<SaleContract?> GetByIdAsync(Guid id);
        Task<SaleContract?> GetByContractNumberAsync(string contractNumber);
        Task<SaleContract> CreateAsync(SaleContract contract);
        Task<SaleContract> UpdateAsync(SaleContract contract);
        Task<bool> DeleteAsync(Guid id);
        Task<bool> ExistsAsync(Guid id);
    }
}

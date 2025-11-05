using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CompanyDealer.DAL.Data;
using CompanyDealer.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace CompanyDealer.DAL.Repository.SaleContractRepo
{
    public class SaleContractRepository : ISaleContractRepository
    {
        private readonly ApplicationDbContext _context;

        public SaleContractRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<SaleContract>> GetAllAsync()
        {
            return await _context.SaleContracts
                .Include(c => c.Order)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<SaleContract?> GetByIdAsync(Guid id)
        {
            return await _context.SaleContracts
                .Include(c => c.Order)
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<SaleContract?> GetByContractNumberAsync(string contractNumber)
        {
            return await _context.SaleContracts
                .Include(c => c.Order)
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.ContractNumber == contractNumber);
        }

        public async Task<SaleContract> CreateAsync(SaleContract contract)
        {
            await _context.SaleContracts.AddAsync(contract);
            await _context.SaveChangesAsync();
            return contract;
        }

        public async Task<SaleContract> UpdateAsync(SaleContract contract)
        {
            _context.SaleContracts.Update(contract);
            await _context.SaveChangesAsync();
            return contract;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var contract = await _context.SaleContracts.FindAsync(id);
            if (contract == null)
                return false;

            _context.SaleContracts.Remove(contract);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ExistsAsync(Guid id)
        {
            return await _context.SaleContracts.AnyAsync(c => c.Id == id);
        }
    }
}

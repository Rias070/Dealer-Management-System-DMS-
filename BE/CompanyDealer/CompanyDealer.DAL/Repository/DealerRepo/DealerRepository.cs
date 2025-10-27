using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CompanyDealer.DAL.Data;
using CompanyDealer.DAL.Models;
using CompanyDealer.DAL.Repository;
using Microsoft.EntityFrameworkCore;

namespace CompanyDealer.DAL.Repository.DealerRepo
{
    public class DealerRepository : GenericRepository<Dealer>, IDealerRepository
    {
        private readonly ApplicationDbContext _context;

        public DealerRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Dealer> CreateAsync(Dealer dealer)
        {
            await _context.Dealers.AddAsync(dealer);
            await _context.SaveChangesAsync();
            return dealer;
        }

        public async Task<Dealer?> GetByIdAsync(Guid id)
        {
            return await _context.Dealers
                .Include(d => d.Accounts)
                .Include(d => d.DealerContracts)
                .FirstOrDefaultAsync(d => d.Id == id);
        }

        public async Task<List<Dealer>> GetAllAsync()
        {
            return await _context.Dealers
                .Include(d => d.Accounts)
                .Include(d => d.DealerContracts)
                .OrderBy(d => d.Name)
                .ToListAsync();
        }

        public async Task<List<Dealer>> GetActiveDealersAsync()
        {
            return await _context.Dealers
                .Include(d => d.Accounts)
                .Include(d => d.DealerContracts)
                .Where(d => d.IsActive)
                .OrderBy(d => d.Name)
                .ToListAsync();
        }

        public async Task<Dealer?> UpdateAsync(Dealer dealer)
        {
            var existingDealer = await _context.Dealers.FindAsync(dealer.Id);
            if (existingDealer == null)
                return null;

            existingDealer.Name = dealer.Name;
            existingDealer.Location = dealer.Location;
            existingDealer.ContactInfo = dealer.ContactInfo;
            existingDealer.IsActive = dealer.IsActive;

            _context.Dealers.Update(existingDealer);
            await _context.SaveChangesAsync();
            return existingDealer;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var dealer = await _context.Dealers.FindAsync(id);
            if (dealer == null)
                return false;

            _context.Dealers.Remove(dealer);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ExistsAsync(Guid id)
        {
            return await _context.Dealers.AnyAsync(d => d.Id == id);
        }
    }
}

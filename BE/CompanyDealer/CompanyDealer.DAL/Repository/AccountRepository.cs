using System;
using System.Threading.Tasks;
using CompanyDealer.DAL.Data;
using CompanyDealer.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace CompanyDealer.DAL.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly ApplicationDbContext _context;

        public AccountRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Account?> GetByUsernameAndPasswordAsync(string username, string password)
        {
            return await _context.Accounts.AsNoTracking()
                .FirstOrDefaultAsync(a => a.Username == username && a.Password == password);
        }

        public async Task<Account?> GetByIdAsync(Guid id)
        {
            return await _context.Accounts.AsNoTracking().FirstOrDefaultAsync(a => a.Id == id);
        }
    }
}



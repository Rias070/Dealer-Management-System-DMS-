using CompanyDealer.DAL.Data;
using CompanyDealer.DAL.Models;
using CompanyDealer.DAL.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace CompanyDealer.DAL.Repository.UserRepo
{
    public class AccountRepository : GenericRepository<Account>, IAccountRepository
    {
        public AccountRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<Account> GetByUserNameAndPasswordAsync(string userName, string password)
        {
            return await _context.Set<Account>()
                .FirstOrDefaultAsync(u => u.Username == userName && u.Password == password);
        }

        public async Task<Account> GetByIdWithRolesAsync(Guid userId)
        {
            return await _context.Set<Account>()
                .Include(u => u.Roles)
                .FirstOrDefaultAsync(u => u.Id == userId);
        }

        public async Task<Account> GetByUserNameWithRolesAsync(string userName)
        {
            return await _context.Set<Account>()
                .Include(u => u.Roles)
                .FirstOrDefaultAsync(u => u.Username == userName);
        }

        public async Task AssignRoleToUserAsync(Guid userId, Guid roleId)
        {
            var user = await _context.Set<Account>()
                .Include(u => u.Roles)
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
                throw new ArgumentException($"User with ID {userId} not found");

            var role = await _context.Set<Role>()
                .FirstOrDefaultAsync(r => r.Id == roleId && !r.IsDeleted);

            if (role == null)
                throw new ArgumentException($"Role does not exist");

            if (user.Roles == null)
                user.Roles = new List<Role>();

            // Check if user already has this role
            if (!user.Roles.Any(r => r.Id == role.Id))
            {
                user.Roles.Add(role);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<Account> GetUserByUserIdAsync(Guid userId)
        {
            return await _dbSet.FirstOrDefaultAsync(u => u.Id == userId);
        }

        public async Task<Guid?> GetDealerIdByNameAsync(string dealerName)
        {
            var dealer = await _context.Set<Dealer>().FirstOrDefaultAsync(d => d.Name == dealerName);
            return dealer?.Id;
        }

        public async Task<Guid?> GetDealerByAccount(Guid userId)
        {
            var dealerId = await (from account in _context.Set<Account>()
                                    join d in _context.Set<Dealer>()
                                    on account.DealerId equals d.Id
                                    where account.Id == userId
                                    select d.Id).FirstOrDefaultAsync();
            return dealerId;
        }

        public async Task<IEnumerable<Account>> GetAccountsByDealerIdAsync(Guid dealerId)
        {
            return await _dbSet.Where(a => a.DealerId == dealerId).ToListAsync();
        }
    }
}



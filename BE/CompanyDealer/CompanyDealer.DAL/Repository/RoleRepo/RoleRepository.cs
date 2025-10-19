using CompanyDealer.DAL.Data;
using CompanyDealer.DAL.Models;
using CompanyDealer.DAL.Repository;
using System.Threading.Tasks;
using System.Linq; // Add this using directive
using Microsoft.EntityFrameworkCore; // Add this using directive

namespace CompanyDealer.DAL.Repository.RoleRepo
{
    public class RoleRepository : GenericRepository<Role>, IRoleRepository
    {
        public RoleRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<Role> GetByNameAsync(string roleName)
        {
            return await _context.Set<Role>()
                .FirstOrDefaultAsync(r => r.RoleName == roleName && !r.IsDeleted);
        }

        public async Task<Role> EnsureRoleExistsAsync(string roleName)
        {
            var role = await GetByNameAsync(roleName);

            if (role == null)
            {
                role = new Role
                {
                    Id = Guid.NewGuid(),
                    RoleName = roleName,
                    IsDeleted = false
                };
                await _context.Set<Role>().AddAsync(role);
                await _context.SaveChangesAsync();
            }

            return role;
        }
    }
}
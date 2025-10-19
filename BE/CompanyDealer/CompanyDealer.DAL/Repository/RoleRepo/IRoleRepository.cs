using CompanyDealer.DAL.Data;
using CompanyDealer.DAL.Models;
using CompanyDealer.DAL.Repository;
using System.Threading.Tasks;

namespace CompanyDealer.DAL.Repository.RoleRepo
{
    public interface IRoleRepository : IGenericRepository<Role>
    {
        Task<Role> GetByNameAsync(string roleName);
        Task<Role> EnsureRoleExistsAsync(string roleName);
    }
}
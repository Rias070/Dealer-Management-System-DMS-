using System;
using System.Threading.Tasks;
using CompanyDealer.DAL.Models;

namespace CompanyDealer.DAL.Repository.UserRepo
{
    public interface IAccountRepository : IGenericRepository<Account>
    {
        Task<Account> GetByUserNameAndPasswordAsync(string userName, string password);
        Task<Account> GetByIdWithRolesAsync(Guid userId);
        Task<Account> GetByUserNameWithRolesAsync(string userName);
        Task AssignRoleToUserAsync(Guid userId, Guid roleId);
        Task<Account> GetUserByUserIdAsync(Guid userId);
        Task<Guid?> GetDealerIdByNameAsync(string dealerName);
        Task<Guid?> GetDealerByAccount(Guid userId);
    }
}



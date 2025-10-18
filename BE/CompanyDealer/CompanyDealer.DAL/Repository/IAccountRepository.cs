using System;
using System.Threading.Tasks;
using CompanyDealer.DAL.Models;

namespace CompanyDealer.DAL.Repositories
{
    public interface IAccountRepository
    {
        Task<Account?> GetByUsernameAndPasswordAsync(string username, string password);
        Task<Account?> GetByIdAsync(Guid id);
    }
}



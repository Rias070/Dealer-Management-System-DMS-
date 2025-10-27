using CompanyDealer.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompanyDealer.DAL.Repository.RestockRepo
{
    public interface IRestockRequestRepository
    {
        Task<List<RestockRequest>> GetAllAsync();
        Task<RestockRequest?> GetByIdAsync(Guid id);
        Task CreateAsync(RestockRequest entity);
        Task UpdateAsync(RestockRequest entity);
        Task<bool> DeleteAsync(Guid id);
        Task<List<RestockRequest>> GetByDealerIdAsync(Guid dealerId);
        Task<List<RestockRequest>> GetRequestsForAcceptenceLevelAsync(string acceptenceLevel);
    }
}

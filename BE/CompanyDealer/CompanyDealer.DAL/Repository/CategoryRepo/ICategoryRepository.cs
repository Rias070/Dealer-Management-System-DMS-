using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompanyDealer.DAL.Repository.CategoryRepo
{
    public interface ICategoryRepository : IGenericRepository<Models.Category>
    {
        Task<Models.Category> CreateAsync(Models.Category category);
        Task<bool> DeleteAsync(Guid id);
    }
}

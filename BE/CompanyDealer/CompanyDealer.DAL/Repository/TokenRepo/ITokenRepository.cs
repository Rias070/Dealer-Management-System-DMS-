using CompanyDealer.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompanyDealer.DAL.Repository.TokenRepo
{
    public interface ITokenRepository : IGenericRepository<Token>
    {
        Task<Token> GetByRefreshTokenAsync(string refreshToken);
        Task<Token> GetByUserIdAsync(Guid userId);
    }
}

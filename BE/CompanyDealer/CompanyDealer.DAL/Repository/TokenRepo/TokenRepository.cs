using CompanyDealer.DAL.Data;
using CompanyDealer.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompanyDealer.DAL.Repository.TokenRepo
{
    public class TokenRepository : GenericRepository<Token>, ITokenRepository
    {
        public TokenRepository(ApplicationDbContext context) : base(context) { }

        public async Task<Token> GetByRefreshTokenAsync(string refreshToken)
        {
            return await _dbSet
                .FirstOrDefaultAsync(t => t.RefreshToken == refreshToken);
        }

        public async Task<Token> GetByUserIdAsync(Guid userId)
        {
            return await _dbSet
                .FirstOrDefaultAsync(t => t.AccountId == userId);
        }
    }
}

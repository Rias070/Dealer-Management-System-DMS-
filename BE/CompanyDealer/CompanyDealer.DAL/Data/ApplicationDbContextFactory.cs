using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace CompanyDealer.DAL.Data
{
    public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();

            // 🔧 Update this connection string to match your environment
            optionsBuilder.UseNpgsql("Host=localhost;Database=CompanyDealerDB;Username=postgres;Password=12345");

            return new ApplicationDbContext(optionsBuilder.Options);
        }
    }
}

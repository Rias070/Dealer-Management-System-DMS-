using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using CompanyDealer.DAL.Data;
using CompanyDealer.DAL.Models;
using BCrypt.Net;

namespace CompanyDealer.DataInitalizer
{
    public static class DataInitializer
    {
        /// <summary>
        /// Seed demo data that matches the actual model properties present in the project.
        /// This avoids referencing properties/types that may not exist and keeps the seeder compilable.
        /// Call after migrations during app startup.
        /// </summary>
        public static async Task SeedAsync(ApplicationDbContext context)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));
            if (await context.Dealers.AnyAsync()) return;

            // Seed Roles
            var companyAdminRole = new Role { Id = Guid.NewGuid(), RoleName = "CompanyAdmin" };
            var companyStaffRole = new Role { Id = Guid.NewGuid(), RoleName = "CompanyStaff" };
            var dealerAdminRole = new Role { Id = Guid.NewGuid(), RoleName = "DealerAdmin" };
            var dealerStaffRole = new Role { Id = Guid.NewGuid(), RoleName = "DealerStaff" };
            await context.Set<Role>().AddRangeAsync(companyAdminRole, companyStaffRole, dealerAdminRole, dealerStaffRole);

            // Seed Dealers
            var dealer1Id = Guid.NewGuid();
            var dealer2Id = Guid.NewGuid();
            var dealer3Id = Guid.NewGuid();

            var dealer1 = new Dealer
            {
                Id = dealer1Id,
                Name = "Demo Dealer 1",
                Location = "Hanoi, Vietnam",
                ContactInfo = "demo1@dealer.local | +84 912 345 678",
                RegistrationDate = DateTime.UtcNow.AddYears(-1),
                IsActive = true
            };

            var dealer2 = new Dealer
            {
                Id = dealer2Id,
                Name = "Demo Dealer 2",
                Location = "Ho Chi Minh City, Vietnam",
                ContactInfo = "demo2@dealer.local | +84 912 111 222",
                RegistrationDate = DateTime.UtcNow.AddYears(-2),
                IsActive = true
            };

            var dealer3 = new Dealer
            {
                Id = dealer3Id,
                Name = "Demo Dealer 3",
                Location = "Da Nang, Vietnam",
                ContactInfo = "demo3@dealer.local | +84 912 333 444",
                RegistrationDate = DateTime.UtcNow.AddYears(-3),
                IsActive = true
            };

            // Accounts for each dealer
            var adminAccount1 = new Account
            {
                Id = Guid.NewGuid(),
                Name = "Company Admin 1",
                ContactPerson = "Admin Person 1",
                Email = "admin1@dealer.local",
                Phone = "+84 912 345 678",
                Address = "123 Demo Street",
                CreatedAt = DateTime.UtcNow,
                IsActive = true,
                Username = "admin1",
                Password = BCrypt.Net.BCrypt.HashPassword("admin123"),
                DealerId = dealer1Id,
                Roles = new[] { companyAdminRole }
            };

            var adminAccount2 = new Account
            {
                Id = Guid.NewGuid(),
                Name = "Company Admin 2",
                ContactPerson = "Admin Person 2",
                Email = "admin2@dealer.local",
                Phone = "+84 912 111 222",
                Address = "456 Demo Street",
                CreatedAt = DateTime.UtcNow,
                IsActive = true,
                Username = "admin2",
                Password = BCrypt.Net.BCrypt.HashPassword("admin123"),
                DealerId = dealer2Id,
                Roles = new[] { companyAdminRole }
            };

            var adminAccount3 = new Account
            {
                Id = Guid.NewGuid(),
                Name = "Company Admin 3",
                ContactPerson = "Admin Person 3",
                Email = "admin3@dealer.local",
                Phone = "+84 912 333 444",
                Address = "789 Demo Street",
                CreatedAt = DateTime.UtcNow,
                IsActive = true,
                Username = "admin3",
                Password = BCrypt.Net.BCrypt.HashPassword("admin123"),
                DealerId = dealer3Id,
                Roles = new[] { companyAdminRole }
            };

            // Add in proper order to satisfy FKs
            await context.Set<Role>().AddRangeAsync(companyAdminRole, companyStaffRole, dealerAdminRole, dealerStaffRole);
            await context.Dealers.AddRangeAsync(dealer1, dealer2, dealer3);
            await context.Accounts.AddRangeAsync(adminAccount1, adminAccount2, adminAccount3);

            await context.SaveChangesAsync();
        }
    }
}

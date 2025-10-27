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
            var dealerManagerRole = new Role { Id = Guid.NewGuid(), RoleName = "DealerManager" };
            var companyManagerRole = new Role { Id = Guid.NewGuid(), RoleName = "CompanyManager" };

            await context.Set<Role>().AddRangeAsync(
                companyAdminRole, companyStaffRole, dealerAdminRole, dealerStaffRole, dealerManagerRole, companyManagerRole
            );

            // Seed Dealers
            var dealer1Id = Guid.NewGuid();
            var dealer2Id = Guid.NewGuid();
            var dealer3Id = Guid.NewGuid();

            // đại diện cho company
            var dealer1 = new Dealer
            {
                Id = dealer1Id,
                Name = "Company",
                Location = "Hanoi, Vietnam",
                ContactInfo = "CompanyGmail@.local | +84 912 345 678",
                RegistrationDate = DateTime.UtcNow.AddYears(-1),
                IsActive = true
            };


            // các dealer
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

            // Seed Categories (add more if needed)
            var sedanCategory = new Category { Id = Guid.NewGuid(), Name = "Sedan", Description = "Sedan cars" };
            var suvCategory = new Category { Id = Guid.NewGuid(), Name = "SUV", Description = "SUV cars" };
            var hatchbackCategory = new Category { Id = Guid.NewGuid(), Name = "Hatchback", Description = "Hatchback cars" };
            var luxuryCategory = new Category { Id = Guid.NewGuid(), Name = "Luxury", Description = "Luxury cars" };
            await context.Categories.AddRangeAsync(sedanCategory, suvCategory, hatchbackCategory, luxuryCategory);

            // Seed Vehicles (VinFast electric and gasoline)
            var vinfastE34 = new Vehicle
            {
                Id = Guid.NewGuid(),
                Make = "VinFast",
                Model = "VF e34",
                Year = 2023,
                VIN = "VF-E34-001",
                Color = "White",
                Price = 690000000,
                Description = "VinFast VF e34 - Electric SUV",
                IsAvailable = true,
                CategoryId = suvCategory.Id
            };
            var vinfastVf5 = new Vehicle
            {
                Id = Guid.NewGuid(),
                Make = "VinFast",
                Model = "VF 5",
                Year = 2023,
                VIN = "VF-5-001",
                Color = "Yellow",
                Price = 458000000,
                Description = "VinFast VF 5 - Electric Crossover",
                IsAvailable = true,
                CategoryId = suvCategory.Id
            };
            var vinfastVf6 = new Vehicle
            {
                Id = Guid.NewGuid(),
                Make = "VinFast",
                Model = "VF 6",
                Year = 2023,
                VIN = "VF-6-001",
                Color = "Blue",
                Price = 595000000,
                Description = "VinFast VF 6 - Electric SUV",
                IsAvailable = true,
                CategoryId = suvCategory.Id
            };
            var vinfastVf7 = new Vehicle
            {
                Id = Guid.NewGuid(),
                Make = "VinFast",
                Model = "VF 7",
                Year = 2023,
                VIN = "VF-7-001",
                Color = "Red",
                Price = 800000000,
                Description = "VinFast VF 7 - Electric SUV",
                IsAvailable = true,
                CategoryId = suvCategory.Id
            };
            var vinfastVf8 = new Vehicle
            {
                Id = Guid.NewGuid(),
                Make = "VinFast",
                Model = "VF 8",
                Year = 2023,
                VIN = "VF-8-001",
                Color = "Silver",
                Price = 1090000000,
                Description = "VinFast VF 8 - Electric SUV",
                IsAvailable = true,
                CategoryId = suvCategory.Id
            };
            var vinfastVf9 = new Vehicle
            {
                Id = Guid.NewGuid(),
                Make = "VinFast",
                Model = "VF 9",
                Year = 2023,
                VIN = "VF-9-001",
                Color = "Black",
                Price = 1500000000,
                Description = "VinFast VF 9 - Electric SUV",
                IsAvailable = true,
                CategoryId = suvCategory.Id
            };
            // Gasoline models
            var vinfastLuxA20 = new Vehicle
            {
                Id = Guid.NewGuid(),
                Make = "VinFast",
                Model = "Lux A2.0",
                Year = 2021,
                VIN = "VF-LUXA20-001",
                Color = "Gray",
                Price = 1100000000,
                Description = "VinFast Lux A2.0 - Gasoline Sedan",
                IsAvailable = true,
                CategoryId = sedanCategory.Id
            };
            var vinfastLuxSA20 = new Vehicle
            {
                Id = Guid.NewGuid(),
                Make = "VinFast",
                Model = "Lux SA2.0",
                Year = 2021,
                VIN = "VF-LUXSA20-001",
                Color = "White",
                Price = 1500000000,
                Description = "VinFast Lux SA2.0 - Gasoline SUV",
                IsAvailable = true,
                CategoryId = suvCategory.Id
            };
            var vinfastFadil = new Vehicle
            {
                Id = Guid.NewGuid(),
                Make = "VinFast",
                Model = "Fadil",
                Year = 2021,
                VIN = "VF-FADIL-001",
                Color = "Red",
                Price = 425000000,
                Description = "VinFast Fadil - Gasoline Hatchback",
                IsAvailable = true,
                CategoryId = hatchbackCategory.Id
            };
            var vinfastPresident = new Vehicle
            {
                Id = Guid.NewGuid(),
                Make = "VinFast",
                Model = "President",
                Year = 2021,
                VIN = "VF-PRESIDENT-001",
                Color = "Black",
                Price = 4600000000,
                Description = "VinFast President - Luxury Gasoline SUV",
                IsAvailable = true,
                CategoryId = luxuryCategory.Id
            };

            // Add all VinFast vehicles to the context
            await context.Vehicles.AddRangeAsync(
                vinfastE34, vinfastVf5, vinfastVf6, vinfastVf7, vinfastVf8, vinfastVf9,
                vinfastLuxA20, vinfastLuxSA20, vinfastFadil, vinfastPresident
            );

            // Seed Inventories
            var inventory1 = new Inventory
            {
                Id = Guid.NewGuid(),
                DealerId = dealer1Id,
                LastUpdated = DateTime.UtcNow
            };
            var inventory2 = new Inventory
            {
                Id = Guid.NewGuid(),
                DealerId = dealer2Id,
                LastUpdated = DateTime.UtcNow
            };
            var inventory3 = new Inventory
            {
                Id = Guid.NewGuid(),
                DealerId = dealer3Id,
                LastUpdated = DateTime.UtcNow
            };
            await context.Inventories.AddRangeAsync(inventory1, inventory2, inventory3);

            // Link Vehicles to Inventories via InventoryVehicle
            var inventoryVehicle1 = new InventoryVehicle
            {
                InventoryId = inventory1.Id,
                VehicleId = vinfastE34.Id,
                Quantity = 5
            };
            var inventoryVehicle2 = new InventoryVehicle
            {
                InventoryId = inventory1.Id,
                VehicleId = vinfastVf5.Id,
                Quantity = 2
            };
            var inventoryVehicle3 = new InventoryVehicle
            {
                InventoryId = inventory2.Id,
                VehicleId = vinfastVf6.Id,
                Quantity = 3
            };
            var inventoryVehicle4 = new InventoryVehicle
            {
                InventoryId = inventory3.Id,
                VehicleId = vinfastE34.Id,
                Quantity = 1
            };
            await context.InventoryVehicles.AddRangeAsync(inventoryVehicle1, inventoryVehicle2, inventoryVehicle3, inventoryVehicle4);

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

            var companyStaff1 = new Account
            {
                Id = Guid.NewGuid(),
                Name = "Company Staff 1",
                ContactPerson = "Admin Person 3",
                Email = "asdf@dealer.local",
                Phone = "+84 912 333 444",
                Address = "789 Demo Street",
                CreatedAt = DateTime.UtcNow,
                IsActive = true,
                Username = "staff1",
                Password = BCrypt.Net.BCrypt.HashPassword("staff123"),
                DealerId = dealer3Id,
                Roles = new[] { companyStaffRole }
            };
            var companyStaff2 = new Account
            {
                Id = Guid.NewGuid(),
                Name = "Company Staff 2",
                ContactPerson = "Admin Person 2",
                Email = "asdf@dealer.local",
                Phone = "+84 912 333 444",
                Address = "789 Demo Street",
                CreatedAt = DateTime.UtcNow,
                IsActive = true,
                Username = "staff2",
                Password = BCrypt.Net.BCrypt.HashPassword("staff123"),
                DealerId = dealer3Id,
                Roles = new[] { companyStaffRole }
            };

            // Seed DealerManager account
            var dealerManagerAccount = new Account
            {
                Id = Guid.NewGuid(),
                Name = "Dealer Manager 1",
                ContactPerson = "Manager Person 1",
                Email = "manager1@dealer.local",
                Phone = "+84 912 555 666",
                Address = "321 Manager Street",
                CreatedAt = DateTime.UtcNow,
                IsActive = true,
                Username = "manager1",
                Password = BCrypt.Net.BCrypt.HashPassword("manager123"),
                DealerId = dealer2Id, // or assign to another dealer as needed
                Roles = new[] { dealerManagerRole }
            };

            // DealerAdmin account
            var dealerAdminAccount = new Account
            {
                Id = Guid.NewGuid(),
                Name = "Dealer Admin 1",
                ContactPerson = "Dealer Admin Person",
                Email = "dealeradmin1@dealer.local",
                Phone = "+84 912 777 888",
                Address = "111 Dealer Admin Street",
                CreatedAt = DateTime.UtcNow,
                IsActive = true,
                Username = "dealeradmin1",
                Password = BCrypt.Net.BCrypt.HashPassword("dealeradmin123"),
                DealerId = dealer1Id,
                Roles = new[] { dealerAdminRole }
            };

            // DealerStaff account
            var dealerStaffAccount = new Account
            {
                Id = Guid.NewGuid(),
                Name = "Dealer Staff 1",
                ContactPerson = "Dealer Staff Person",
                Email = "dealerstaff1@dealer.local",
                Phone = "+84 912 888 999",
                Address = "222 Dealer Staff Street",
                CreatedAt = DateTime.UtcNow,
                IsActive = true,
                Username = "dealerstaff1",
                Password = BCrypt.Net.BCrypt.HashPassword("dealerstaff123"),
                DealerId = dealer2Id,
                Roles = new[] { dealerStaffRole }
            };

            // CompanyManager account
            var companyManagerAccount = new Account
            {
                Id = Guid.NewGuid(),
                Name = "Company Manager 1",
                ContactPerson = "Company Manager Person",
                Email = "companymanager1@dealer.local",
                Phone = "+84 912 999 000",
                Address = "333 Company Manager Street",
                CreatedAt = DateTime.UtcNow,
                IsActive = true,
                Username = "companymanager1",
                Password = BCrypt.Net.BCrypt.HashPassword("companymanager123"),
                DealerId = dealer1Id,
                Roles = new[] { companyManagerRole }
            };

            await context.Dealers.AddRangeAsync(dealer1, dealer2, dealer3);
            await context.Accounts.AddRangeAsync(
                adminAccount1, adminAccount2, adminAccount3,
                companyStaff1, companyStaff2,
                dealerManagerAccount,
                dealerAdminAccount,
                dealerStaffAccount,
                companyManagerAccount
            );

            await context.SaveChangesAsync();
        }
    }
}

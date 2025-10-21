using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using CompanyDealer.DAL.Models;
using CompanyDealer.DAL.Data;

namespace CompanyDealer.DAL.Repository.VehicleRepo
{
    public class VehicleRepository : GenericRepository<Vehicle>, IVehicleRepository
    {
        private readonly ApplicationDbContext _context;

        public VehicleRepository(ApplicationDbContext context)
            : base(context) 
        {
            _context = context;
        }

        // Create
        public async Task<Vehicle> CreateAsync(Vehicle vehicle)
        {
            _context.Set<Vehicle>().Add(vehicle);
            await _context.SaveChangesAsync();
            return vehicle;
        }

        // Read (Get by Id)
        public async Task<Vehicle?> GetByIdAsync(Guid id)
        {
            return await _context.Set<Vehicle>()
                .Include(v => v.Category)
                .Include(v => v.TestDriveRecords)
                .FirstOrDefaultAsync(v => v.Id == id);
        }

        // Read (Get all)
        public async Task<List<Vehicle>> GetAllAsync()
        {
            return await _context.Set<Vehicle>()
         
                .Include(v => v.Category)
                .Include(v => v.TestDriveRecords)
                .ToListAsync();
        }

        // Update
        public async Task<Vehicle?> UpdateAsync(Vehicle vehicle)
        {
            var existingVehicle = await _context.Set<Vehicle>().FindAsync(vehicle.Id);
            if (existingVehicle == null)
            {
                return null;
            }

            _context.Entry(existingVehicle).CurrentValues.SetValues(vehicle);
            await _context.SaveChangesAsync();
            return existingVehicle;
        }

        // Delete
        public async Task<bool> DeleteAsync(Guid id)
        {
            var vehicle = await _context.Set<Vehicle>().FindAsync(id);
            if (vehicle == null)
            {
                return false;
            }

            _context.Set<Vehicle>().Remove(vehicle);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}

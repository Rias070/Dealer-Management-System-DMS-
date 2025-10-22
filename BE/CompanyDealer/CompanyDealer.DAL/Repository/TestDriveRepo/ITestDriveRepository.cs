using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CompanyDealer.DAL.Models;

namespace CompanyDealer.DAL.Repository.TestDriveRepo
{
    public interface ITestDriveRepository
    {
        Task<IEnumerable<TestDriveRecord>> GetAllAsync();
        Task<IEnumerable<TestDriveRecord>> GetByDealerIdAsync(Guid dealerId);
        Task<IEnumerable<TestDriveRecord>> GetByVehicleIdAsync(Guid vehicleId);
        Task<IEnumerable<TestDriveRecord>> GetByStatusAsync(TestDriveStatus status);
        Task<TestDriveRecord?> GetByIdAsync(Guid id);
        Task<TestDriveRecord> CreateAsync(TestDriveRecord testDrive);
        Task<TestDriveRecord> UpdateAsync(TestDriveRecord testDrive);
        Task<bool> DeleteAsync(Guid id);
        Task<bool> ExistsAsync(Guid id);
        Task<bool> HasScheduleConflictAsync(Guid vehicleId, DateTime testDate, Guid? excludeTestDriveId = null);
        Task<IEnumerable<TestDriveRecord>> GetByDateRangeAsync(DateTime fromDate, DateTime toDate);
    }
}

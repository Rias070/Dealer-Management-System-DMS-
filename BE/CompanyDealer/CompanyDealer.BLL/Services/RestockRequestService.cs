using CompanyDealer.DAL.Models;
using CompanyDealer.DAL.Repository.RestockRepo;
using CompanyDealer.BLL.DTOs.RestockRequestDTOs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CompanyDealer.DAL.Repository.InventoryRepo;
using CompanyDealer.DAL.Repository.VehicleRepo;
using CompanyDealer.DAL.Repository.ContractRepo;

public class RestockRequestService
{
    private readonly IRestockRequestRepository _repo;
    private readonly IInventoryRepository _inventoryRepo;
    private readonly IVehicleRepository _vehicleRepo;
    private readonly IContractRepository _contractRepo;

    public RestockRequestService(IRestockRequestRepository repo, IInventoryRepository inventoryRepo, IVehicleRepository vehicleRepository, IContractRepository contractRepository)
    {
        _repo = repo;
        _inventoryRepo = inventoryRepo;
        _vehicleRepo = vehicleRepository;
        _contractRepo = contractRepository;
    }

    public async Task<List<RestockRequestDto>> GetAllAsync()
    {
        var requests = await _repo.GetAllAsync();
        return requests.Select(r => new RestockRequestDto
        {
            Id = r.id,
            AccountId = r.AccountId,
            DealerId = r.DealerId,
            VehicleId = r.VehicleId,
            VehicleName = r.VehicleName,
            Quantity = r.Quantity,
            RequestDate = r.RequestDate,
            ResponseDate = r.ResponseDate,
            AcceptenceLevel = r.AcceptenceLevel,
            AcceptedBy = r.AcceptedBy,
            Status = r.Status,
            Description = r.Description
        }).ToList();
    }

    

    public async Task<RestockRequestDto?> GetByIdAsync(Guid id)
    {
        var r = await _repo.GetByIdAsync(id);
        if (r == null) return null;
        return new RestockRequestDto
        {
            Id = r.id,
            AccountId = r.AccountId,
            DealerId = r.DealerId,
            VehicleId = r.VehicleId,
            VehicleName = r.VehicleName,
            Quantity = r.Quantity,
            RequestDate = r.RequestDate,
            ResponseDate = r.ResponseDate,
            AcceptenceLevel = r.AcceptenceLevel,
            AcceptedBy = r.AcceptedBy,
            Status = r.Status,
            Description = r.Description
        };
    }

    public async Task<RestockRequestDto> CreateAsync(CreateRestockRequestDto dto)
    {
        var Vehice = await _vehicleRepo.GetByIdAsync(dto.VehicleId);
        if (Vehice == null)
        {
            throw new ArgumentException($"Vehicle with ID {dto.VehicleId} not found in inventory", nameof(dto.VehicleId));
        }
        var totalPrice = Vehice.Price * dto.Quantity;
        var entity = new RestockRequest
        {
            id = Guid.NewGuid(),
            AccountId = dto.AccountId,
            DealerId = dto.DealerId,
            VehicleId = dto.VehicleId,
            VehicleName = dto.VehicleName,
            Quantity = dto.Quantity,
            TotalPrice = totalPrice,
            RequestDate = DateTime.UtcNow,
            Status = "Pending",
            AcceptenceLevel = "Dealer",
            AcceptedBy = "",
            ReasonRejected = "",
            Description = dto.Description
        };
        await _repo.CreateAsync(entity);
        return await GetByIdAsync(entity.id);
    }

    public async Task<bool> UpdateAsync(Guid id, RestockRequestDto dto)
    {
        var entity = await _repo.GetByIdAsync(id);
        if (entity == null) return false;
        entity.Quantity = dto.Quantity;
        entity.Description = dto.Description;
        entity.Status = dto.Status;
        entity.AcceptenceLevel = dto.AcceptenceLevel;
        entity.AcceptedBy = dto.AcceptedBy;
        entity.ReasonRejected = dto.ReasonRejected;
        entity.ResponseDate = dto.ResponseDate ?? entity.ResponseDate;
        await _repo.UpdateAsync(entity);
        return true;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        return await _repo.DeleteAsync(id);
    }

    public async Task<bool> AcceptAndEscalateAsync(Guid id, Guid managerAccountId)
    {
        var entity = await _repo.GetByIdAsync(id);
        if (entity == null || entity.Status != "Pending" || entity.AcceptenceLevel != "Dealer")
            return false;
        entity.Status = "Pending";
        entity.AcceptenceLevel = "Company";
        entity.AcceptedBy = managerAccountId.ToString();
        entity.ResponseDate = DateTime.UtcNow;
        await _repo.UpdateAsync(entity);
        return true;
    }
    public async Task<bool> RejectAsync(Guid id, Guid managerAccountId, string rejectReason)
    {
        var entity = await _repo.GetByIdAsync(id);
        if (entity == null || entity.Status != "Pending" || entity.AcceptenceLevel != "Dealer")
            return false;
        entity.Status = "Rejected";
        entity.ReasonRejected = rejectReason;
        entity.AcceptedBy = managerAccountId.ToString();
        entity.ResponseDate = DateTime.UtcNow;
        await _repo.UpdateAsync(entity);
        return true;
    }

    public async Task<bool> CompanyAcceptAsync(Guid id, Guid managerAccountId)
    {
        var entity = await _repo.GetByIdAsync(id);
        if (entity == null || entity.Status != "Pending" || entity.AcceptenceLevel != "Company")
            return false;
        bool result = await _inventoryRepo.ReduceQuantityIfEnough(entity.VehicleId, entity.Quantity);
        if (!result)
            return false;
        entity.Status = "Accept";
        entity.AcceptenceLevel = "Company";
        entity.AcceptedBy = managerAccountId.ToString();
        entity.ResponseDate = DateTime.UtcNow;
        // ✅ Generate Contract

        var contract = new Contract
        {
            RestockRequestId = id,
            TotalAmount = entity.TotalPrice, // hoặc tính lại nếu có
            ExpirationDate = DateTime.UtcNow.AddMonths(6), // ví dụ hợp đồng có hạn 6 tháng
            Notes = "Automatically generated when company accepted the restock request."
        };
        await _contractRepo.AddAsync(contract);

        await _repo.UpdateAsync(entity);
        return true;
    }

    public async Task<bool> CompanyRejectAsync(Guid id, string rejectReason)
    {
        var entity = await _repo.GetByIdAsync(id);
        if (entity == null || entity.Status != "Pending" || entity.AcceptenceLevel != "Company")
            return false;
        entity.Status = "Rejected";
        entity.ReasonRejected = rejectReason;
        entity.ResponseDate = DateTime.UtcNow;
        await _repo.UpdateAsync(entity);
        return true;
    }



    public async Task<List<RestockRequestDto>> GetRequestsByDealerManager(Guid dealerId)
    {
        var requests = await _repo.GetByDealerIdAsync(dealerId);
        return requests.Select(r => new RestockRequestDto
        {
            Id = r.id,
            AccountId = r.AccountId,
            DealerId = r.DealerId,
            VehicleId = r.VehicleId,
            VehicleName = r.VehicleName,
            Quantity = r.Quantity,
            RequestDate = r.RequestDate,
            ResponseDate = r.ResponseDate,
            AcceptenceLevel = r.AcceptenceLevel,
            AcceptedBy = r.AcceptedBy,
            ReasonRejected = r.ReasonRejected,
            Status = r.Status,
            Description = r.Description
        }).ToList();
    }
    public async Task<List<RestockRequestDto>> GetRestockRequestFor(string acceptenceLevel)
    {
        var requests = await _repo.GetRequestsForAcceptenceLevelAsync(acceptenceLevel);
        return requests.Select(r => new RestockRequestDto
        {
            Id = r.id,
            AccountId = r.AccountId,
            DealerId = r.DealerId,
            VehicleId = r.VehicleId,
            VehicleName = r.VehicleName,
            Quantity = r.Quantity,
            RequestDate = r.RequestDate,
            ResponseDate = r.ResponseDate,
            AcceptenceLevel = r.AcceptenceLevel,
            AcceptedBy = r.AcceptedBy,
            ReasonRejected = r.ReasonRejected,
            Status = r.Status,
            Description = r.Description
        }).ToList();
    }
}
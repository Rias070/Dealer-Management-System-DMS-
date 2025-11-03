using CompanyDealer.DAL.Repository.ContractRepo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CompanyDealer.DAL.Models;
using CompanyDealer.DAL.Repository.InventoryRepo;
using CompanyDealer.DAL.Repository.VehicleRepo;
using CompanyDealer.DAL.Repository.RestockRepo;

namespace CompanyDealer.BLL.Services
{
    public class ContractService
    {
        private IContractRepository _repo;
        private IInventoryRepository _inventoryRepo;
        private IRestockRequestRepository _restockRepo;
        public ContractService(IContractRepository repository, IInventoryRepository inventoryRepository, IRestockRequestRepository restockRequestRepository)
        {
            _repo = repository;
            _inventoryRepo = inventoryRepository;
            _restockRepo = restockRequestRepository;
        }
        public async Task<List<Contract>> GetByDealerId(Guid dealerId)
        {
            var result = await _repo.GetByDealerId(dealerId);
            return result.ToList();
        }
        public async Task<Contract> ConfirmContract(Guid contractId)
        {
            var contract = await _repo.GetByIdAsync(contractId);
            if (contract == null)
                throw new ArgumentException($"Contract with ID {contractId} not found");
            contract.Status = "Confirmed";
            await _repo.UpdateAsync(contract);
            var restockRequest = _restockRepo.GetByIdAsync(contract.RestockRequestId).Result;
            await _inventoryRepo.IncreaseQuantity(restockRequest.VehicleId, contract.RestockRequest.Quantity, contract.DealerId);
            return contract;
        }
    }
}

using CompanyDealer.DAL.Repository.ContractRepo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CompanyDealer.DAL.Models;
using CompanyDealer.DAL.Repository.InventoryRepo;

namespace CompanyDealer.BLL.Services
{
    public class ContractService
    {
        private IContractRepository _repo;
        private IInventoryRepository _inventoryRepo;
        public ContractService(IContractRepository repository, IInventoryRepository inventoryRepository)
        {
            _repo = repository;
            _inventoryRepo = inventoryRepository;
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
            await _inventoryRepo.IncreaseQuantity(contract.RestockRequest.VehicleId, contract.RestockRequest.Quantity, contract.DealerId);
            return contract;
        }
    }
}

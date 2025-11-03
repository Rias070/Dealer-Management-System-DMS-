using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CompanyDealer.DAL.Repository.ContractRepo;
using CompanyDealer.BLL.Services;

namespace CompanyDealer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RestockContractController : ControllerBase
    {
        private readonly ContractService _contractService;

        public RestockContractController(ContractService contractService)
        {
            _contractService = contractService;
        }

        // GET: api/RestockContract/dealer/{dealerId}
        [HttpGet("dealer/{dealerId:guid}")]
        public async Task<IActionResult> GetByDealerId(Guid dealerId)
        {
            var contracts = await _contractService.GetByDealerId(dealerId);
            if (contracts == null || !contracts.Any())
                return NotFound();

            return Ok(contracts);
        }
        [HttpPost("confirm/{contractId:guid}")]
        public async Task<IActionResult> ConfirmContract(Guid contractId)
        {
            var contract = await _contractService.ConfirmContract(contractId);
            if (contract == null)
                return NotFound();
            return Ok(contract);
        }

    }
}

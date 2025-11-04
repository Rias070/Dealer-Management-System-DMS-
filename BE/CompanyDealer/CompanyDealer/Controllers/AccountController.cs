using CompanyDealer.BLL.Services;
using CompanyDealer.DAL.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CompanyDealer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        // GET: api/Account
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Account>>> GetAll()
        {
            var accounts = await _accountService.GetAllAccountsAsync();
            return Ok(accounts);
        }

        // GET: api/Account/{id}
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<Account>> GetById(Guid id)
        {
            var account = await _accountService.GetAccountByIdAsync(id);
            if (account == null) return NotFound();
            return Ok(account);
        }

        // PUT: api/Account/{id}
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] Account account)
        {
            if (id != account.Id) return BadRequest();
            var updated = await _accountService.UpdateAccountAsync(account);
            if (updated == null) return NotFound();
            return Ok(updated);
        }

        // DELETE: api/Account/{id}
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _accountService.DeleteAccountByIdAsync(id);
            return NoContent();
        }

        // POST: api/Account/{id}/activate
        [HttpPost("{id:guid}/activate")]
        public async Task<IActionResult> Activate(Guid id)
        {
            await _accountService.ActivateAccountAsync(id);
            return NoContent();
        }

        // POST: api/Account/{id}/deactivate
        [HttpPost("{id:guid}/deactivate")]
        public async Task<IActionResult> Deactivate(Guid id)
        {
            await _accountService.DeactivateAccountAsync(id);
            return NoContent();
        }

        // GET: api/Account/dealer/{dealerId}
        [HttpGet("dealer/{dealerId:guid}")]
        public async Task<ActionResult<IEnumerable<Account>>> GetByDealerId(Guid dealerId)
        {
            var accounts = await _accountService.GetAccountsByDealerIdAsync(dealerId);
            return Ok(accounts);
        }
    }
}

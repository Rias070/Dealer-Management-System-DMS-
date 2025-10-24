using System;
using System.Threading.Tasks;
using CompanyDealer.BLL.DTOs.RoleDTOs;
using CompanyDealer.BLL.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CompanyDealer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RoleController : ControllerBase
    {
        private readonly IRoleService _roleService;

        public RoleController(IRoleService roleService)
        {
            _roleService = roleService ?? throw new ArgumentNullException(nameof(roleService));
        }

        // GET: api/Role
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var roles = await _roleService.GetAllAsync();
            return Ok(roles);
        }

        // GET: api/Role/{id}
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            if (id == Guid.Empty) return BadRequest();
            var role = await _roleService.GetByIdAsync(id);
            if (role == null) return NotFound();
            return Ok(role);
        }

        // POST: api/Role
        // Consider protecting this endpoint with a policy/role, e.g. [Authorize(Policy = "RequireCompanyAdminRole")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateRoleDto request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.RoleName)) return BadRequest();
            var created = await _roleService.CreateAsync(request);
            if (created == null) return BadRequest("Role already exists or invalid data.");
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        // PUT: api/Role/{id}
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateRoleDto request)
        {
            if (request == null || id == Guid.Empty || id != request.Id) return BadRequest();
            var updated = await _roleService.UpdateAsync(request);
            if (updated == null) return NotFound();
            return Ok(updated);
        }

        // DELETE: api/Role/{id}
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            if (id == Guid.Empty) return BadRequest();
            var deleted = await _roleService.DeleteAsync(id);
            if (!deleted) return NotFound();
            return NoContent();
        }
    }
}
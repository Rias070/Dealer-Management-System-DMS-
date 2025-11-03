using CompanyDealer.BLL.DTOs.CustomerDTOs;
using CompanyDealer.BLL.Services;
using CompanyDealer.DAL.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using CompanyDealer.BLL.ExceptionHandle;

[Route("api/[controller]")]
[ApiController]
public class CustomerController : ControllerBase
{
    private readonly CustomerService _customerService;

    public CustomerController(CustomerService customerService)
    {
        _customerService = customerService;
    }

    // GET: api/customer
    [HttpGet]
    public async Task<ActionResult<List<Customer>>> GetAll()
    {
        var customers = await _customerService.GetAll();
        return Ok(customers);
    }

    // GET: api/customer/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<Customer>> GetById(Guid id)
    {
        var customer = await _customerService.GetById(id);
        if (customer == null) return NotFound();
        return Ok(customer);
    }

    // POST: api/customer
    [HttpPost]
    public async Task<ActionResult<Customer>> Create([FromBody] CustomerRequestDto request)
    {
        var customer = await _customerService.Create(request);
        return CreatedAtAction(nameof(GetById), new { id = customer.Id }, customer);
    }

    // PUT: api/customer/{id}
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] CustomerRequestDto request)
    {
        var updated = await _customerService.Update(id, request);
        if (!updated) return NotFound();
        return Ok(ApiResponse<object>.SuccessResponse(null, "Updated"));
    }

    // DELETE: api/customer/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var deleted = await _customerService.Delete(id);
        if (!deleted) return NotFound();
        return Ok(ApiResponse<object>.SuccessResponse(null, "Deleted"));
    }
}
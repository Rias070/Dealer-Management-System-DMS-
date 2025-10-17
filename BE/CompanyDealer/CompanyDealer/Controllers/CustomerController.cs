using CompanyDealer;
using CompanyDealer.DAL.Data;
using CompanyDealer.DAL.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using static System.Runtime.InteropServices.JavaScript.JSType;

[Route("api/[controller]")]
[ApiController]
public class CustomerController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public CustomerController(ApplicationDbContext context)
    {
        _context = context;
        _context.Database.EnsureCreated(); // create db if not exists
    }

    // GET: api/customer
    [HttpGet]
    public IActionResult GetAll() => Ok(_context.Accounts.ToList());

    // GET: api/customer/5
    [HttpGet("{id}")]
    public IActionResult GetById(int id)
    {
        var customer = _context.Accounts.Find(id);
        return customer == null ? NotFound() : Ok(customer);
    }

    // POST: api/customer
    [HttpPost]
    public IActionResult Create(Account customer)
    {
        _context.Accounts.Add(customer);
        _context.SaveChanges();
        return CreatedAtAction(nameof(GetById), new { id = customer.Id }, customer);
    }

    // PUT: api/customer/5
    [HttpPut("{id}")]
    public IActionResult Update(int id, Account updated)
    {
        

        var customer = _context.Accounts.Find(id);
        if (customer == null) return NotFound();

        customer.Name = updated.Name;
        customer.ContactPerson = updated.ContactPerson;
        customer.Email = updated.Email;
        customer.Phone = updated.Phone;
        customer.Address = updated.Address;
        customer.IsActive = updated.IsActive;
        _context.SaveChanges();
        return NoContent();
    }

    // DELETE: api/customer/5
    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var customer = _context.Accounts.Find(id);
        if (customer == null) return NotFound();

        _context.Accounts.Remove(customer);
        _context.SaveChanges();
        return NoContent();
    }
}

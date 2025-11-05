using CompanyDealer.BLL.DTOs.OrderDTOs;
using CompanyDealer.BLL.Services;
using Microsoft.AspNetCore.Mvc;

namespace CompanyDealer.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        // GET: api/order
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var orders = await _orderService.GetAllAsync();
            return Ok(orders);
        }

        // ✅ GET: api/order/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(Guid id)
        {
            var order = await _orderService.GetByIdAsync(id);
            if (order == null) return NotFound();
            return Ok(order);
        }

        // ✅ POST: api/order
        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] CreateOrderRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var createdOrder = await _orderService.CreateAsync(request);

            // ✅ Make sure GetById exists with {id} route parameter
            return CreatedAtAction(nameof(GetByIdAsync), new { id = createdOrder.Id }, createdOrder);
        }

        // PUT: api/order/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(Guid id, [FromBody] UpdateOrderRequest request)
        {
            var updated = await _orderService.UpdateAsync(id, request);
            if (updated == null) return NotFound();
            return Ok(updated);
        }

        // DELETE: api/order/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            var deleted = await _orderService.DeleteAsync(id);
            if (!deleted) return NotFound();
            return NoContent();
        }
    }
}

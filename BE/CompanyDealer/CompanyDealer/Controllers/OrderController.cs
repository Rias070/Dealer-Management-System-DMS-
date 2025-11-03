using CompanyDealer.BLL.DTOs.Order;
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

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _orderService.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("{orderNumber}")]
        public async Task<IActionResult> GetByOrderNumber(string orderNumber)
        {
            var result = await _orderService.GetByOrderNumberAsync(orderNumber);
            if (result == null)
                return NotFound($"Order {orderNumber} not found.");
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] OrderCreateRequest request)
        {
            var created = await _orderService.CreateAsync(request);
            return CreatedAtAction(nameof(GetByOrderNumber), new { orderNumber = created.OrderNumber }, created);
        }
    }
}

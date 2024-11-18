using System.Security.Claims;
using Backend_Teamwork.src.Entities;
using Backend_Teamwork.src.Services.order;
using Backend_Teamwork.src.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static Backend_Teamwork.src.DTO.OrderDTO;

namespace Backend_Teamwork.src.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrdersController(IOrderService service)
        {
            _orderService = service;
        }

        // GET: api/v1/orders
        [HttpGet]
        [Authorize(Roles = "Admin")] // Accessible by Admin
        public async Task<ActionResult<List<OrderReadDto>>> GetOrders()
        {
            var orders = await _orderService.GetAllAsync();
            return Ok(orders);
        }

        // GET: api/v1/orders
        [HttpGet("my-orders")]
        [Authorize(Roles = "Customer")]
        public async Task<ActionResult<List<OrderReadDto>>> GetMyOrders()
        {
            var authClaims = HttpContext.User;
            var userId = authClaims.FindFirst(c => c.Type == ClaimTypes.NameIdentifier)!.Value;
            var convertedUserId = new Guid(userId);
            var orders = await _orderService.GetAllAsync(convertedUserId);
            return Ok(orders);
        }

        // GET: api/v1/orders/{id}
        [HttpGet("{id}")]
        [Authorize(Roles = "Admin")] // Accessible by Admin
        public async Task<ActionResult<OrderReadDto>> GetOrderById([FromRoute] Guid id)
        {
            var order = await _orderService.GetByIdAsync(id);
            return Ok(order);
        }

        // GET: api/v1/orders/{id}
        [HttpGet("my-orders/{id}")]
        [Authorize(Roles = "Customer")]
        public async Task<ActionResult<OrderReadDto>> GetMyOrderById([FromRoute] Guid id)
        {
            var authClaims = HttpContext.User;
            var userId = authClaims.FindFirst(c => c.Type == ClaimTypes.NameIdentifier)!.Value;
            var convertedUserId = new Guid(userId);
            var order = await _orderService.GetByIdAsync(id, convertedUserId);
            return Ok(order);
        }

        // POST: api/v1/orders
        [HttpPost("add")]
        [Authorize(Roles = "Customer")]
        public async Task<ActionResult<OrderReadDto>> AddOrder([FromBody] OrderCreateDto createDto)
        {
            // extract user information
            var authenticateClaims = HttpContext.User;
            var userId = authenticateClaims
                .FindFirst(c => c.Type == ClaimTypes.NameIdentifier)!
                .Value;
            var userGuid = new Guid(userId);

            var orderCreated = await _orderService.CreateOneAsync(userGuid, createDto);

            return CreatedAtAction(
                nameof(GetOrderById),
                new { id = orderCreated.Id },
                orderCreated
            );
        }

        // PUT: api/v1/orders/{id}
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")] // Accessible by Admin
        public async Task<ActionResult<bool>> UpdateOrder(
            [FromRoute] Guid id,
            [FromBody] OrderUpdateDto updateDto
        )
        {
            var isUpdated = await _orderService.UpdateOneAsync(id, updateDto);
            return Ok(isUpdated);
        }

        // DELETE: api/v1/orders/{id}
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")] // Accessible by Admin
        public async Task<ActionResult<bool>> DeleteOrder(Guid id)
        {
            await _orderService.DeleteOneAsync(id);
            return NoContent();
        }

        // Extra Features
        // GET: api/v1/users/page
        [HttpGet("pagination")]
        [Authorize(Roles = "Admin")] // Accessible by Admin
        public async Task<ActionResult<List<OrderReadDto>>> GetOrdersByPage(
            [FromQuery] PaginationOptions paginationOptions
        )
        {
            var orders = await _orderService.GetOrdersByPage(paginationOptions);
            return Ok(orders);
        }

        [HttpGet("sort-by-date")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<List<OrderReadDto>>> SortOrdersByDate()
        {
            var orders = await _orderService.SortOrdersByDate();
            return Ok(orders);
        }
    }
}

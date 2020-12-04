using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using System.Text;

namespace OrderService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly ILogger<OrderController> _logger;
        private readonly OrderDbContext _dbContext;
        private readonly IHttpClientFactory _httpClientFactory;

        public OrderController(ILogger<OrderController> logger, OrderDbContext dbContext, IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _dbContext = dbContext;
            _httpClientFactory = httpClientFactory;
        }

        [HttpPost]        
        public async Task<IActionResult> CreateOrder([FromBody] Order order)
        {
            var isItemAvailable = await VerifyInventory(order);
            if(!isItemAvailable) return BadRequest("Item is not available");

            var updated = await _dbContext.Order.AddAsync(order);
            await _dbContext.SaveChangesAsync();

            await ClaimInventory(order);

            return Created(nameof(GetOrderDetails), updated.Entity);
        }

        [HttpGet]
        public async Task<List<Order>> GetOrders()
        {
            var item = await _dbContext.Order.ToListAsync();
            return item;
        }

        [HttpGet]
        [Route("orderId")]
        public async Task<Order> GetOrderDetails(int orderId) 
        {
            var item = await _dbContext.Order.SingleOrDefaultAsync(x => x.Id == orderId);
            return item;
        }

        private async Task<bool> VerifyInventory(Order order) 
        {
          using(var httpClient = this._httpClientFactory.CreateClient("InventoryService")) {
              var content = new StringContent(JsonSerializer.Serialize(new { ItemCode = order.ItemCode, Quantity = order.Quantity }), Encoding.UTF8, "application/json");
              var response = await httpClient.PostAsync("api/inventory/verify", content);              
              return response.IsSuccessStatusCode;              
          }   
        }

        private async Task<bool> ClaimInventory(Order order) 
        {
            using(var httpClient = this._httpClientFactory.CreateClient("InventoryService")) {
              var content = new StringContent(JsonSerializer.Serialize(new { ItemCode = order.ItemCode, Quantity = order.Quantity }), Encoding.UTF8, "application/json");
              var response = await httpClient.PostAsync("api/inventory/claim", content);
              return response.IsSuccessStatusCode;              
          } 
        }
    }
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EcommerceApp.Data;
using EcommerceApp.Models;
using System.Text.Json;
using System.Security.Claims;

namespace EcommerceApp.Controllers
{
    [Authorize]
    public class CheckoutController : Controller
    {
        private readonly ApplicationDbContext _context;
        private const string CartSessionKey = "Cart";

        public CheckoutController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var cart = GetCart();
            if (!cart.Any()) return RedirectToAction("Index", "Cart");

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Process(string address, string creditCard)
        {
            var cart = GetCart();
            if (!cart.Any()) return RedirectToAction("Index", "Cart");

            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            
            // Calculate total
            decimal total = 0;
            var orderItems = new List<OrderItem>();

            foreach (var item in cart)
            {
                var product = await _context.Products.FindAsync(item.ProductId);
                if (product != null)
                {
                    total += product.Price * item.Quantity;
                    orderItems.Add(new OrderItem
                    {
                        ProductId = product.Id,
                        Quantity = item.Quantity,
                        Price = product.Price
                    });
                }
            }

            var order = new Order
            {
                UserId = userId,
                TotalAmount = total,
                Status = "Completed", // Mock payment success
                OrderItems = orderItems
            };

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            // Clear Cart
            HttpContext.Session.Remove(CartSessionKey);

            return View("Success", order.Id);
        }

        private List<CartItemDto> GetCart()
        {
            var sessionCart = HttpContext.Session.GetString(CartSessionKey);
            return sessionCart == null ? new List<CartItemDto>() : JsonSerializer.Deserialize<List<CartItemDto>>(sessionCart);
        }
    }
    

}

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EcommerceApp.Data;
using EcommerceApp.Models;
using System.Text.Json;

namespace EcommerceApp.Controllers
{
    public class CartController : Controller
    {
        private readonly ApplicationDbContext _context;
        private const string CartSessionKey = "Cart";

        public CartController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var cart = GetCart();
            var cartViewModel = new List<CartItemViewModel>();

            foreach (var item in cart)
            {
                var product = await _context.Products.FindAsync(item.ProductId);
                if (product != null)
                {
                    cartViewModel.Add(new CartItemViewModel
                    {
                        ProductId = item.ProductId,
                        ProductName = product.Name,
                        Price = product.Price,
                        Quantity = item.Quantity,
                        Total = product.Price * item.Quantity
                    });
                }
            }

            return View(cartViewModel);
        }

        [HttpPost]
        public IActionResult Add(int productId, int quantity)
        {
            var cart = GetCart();
            var existingItem = cart.FirstOrDefault(i => i.ProductId == productId);

            if (existingItem != null)
            {
                existingItem.Quantity += quantity;
            }
            else
            {
                cart.Add(new CartItemDto { ProductId = productId, Quantity = quantity });
            }

            SaveCart(cart);
            return RedirectToAction("Index");
        }

        public IActionResult Remove(int productId)
        {
            var cart = GetCart();
            var item = cart.FirstOrDefault(i => i.ProductId == productId);
            if (item != null)
            {
                cart.Remove(item);
                SaveCart(cart);
            }
            return RedirectToAction("Index");
        }

        private List<CartItemDto> GetCart()
        {
            var sessionCart = HttpContext.Session.GetString(CartSessionKey);
            return sessionCart == null ? new List<CartItemDto>() : JsonSerializer.Deserialize<List<CartItemDto>>(sessionCart);
        }

        private void SaveCart(List<CartItemDto> cart)
        {
            HttpContext.Session.SetString(CartSessionKey, JsonSerializer.Serialize(cart));
        }
    }


}

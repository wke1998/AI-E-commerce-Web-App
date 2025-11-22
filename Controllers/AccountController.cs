using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EcommerceApp.Data;
using EcommerceApp.Models;
using EcommerceApp.Services;
using System.Security.Claims;

namespace EcommerceApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IEmailService _emailService;

        public AccountController(ApplicationDbContext context, IEmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                ModelState.AddModelError("", "Email is required");
                return View();
            }

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null)
            {
                user = new User { Email = email };
                _context.Users.Add(user);
                await _context.SaveChangesAsync();
            }

            // Generate Token
            var token = new Random().Next(100000, 999999).ToString();
            var loginToken = new LoginToken
            {
                UserId = user.Id,
                Token = token,
                Expiry = DateTime.UtcNow.AddMinutes(15)
            };
            _context.LoginTokens.Add(loginToken);
            await _context.SaveChangesAsync();

            // Send Email
            await _emailService.SendEmailAsync(email, "Your Login Code", $"Your code is: {token}");

            return RedirectToAction("Verify", new { email });
        }

        [HttpGet]
        public IActionResult Verify(string email)
        {
            return View(model: email);
        }

        [HttpPost]
        public async Task<IActionResult> Verify(string email, string code)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null) return RedirectToAction("Login");

            var validToken = await _context.LoginTokens
                .FirstOrDefaultAsync(t => t.UserId == user.Id && t.Token == code && !t.IsUsed && t.Expiry > DateTime.UtcNow);

            if (validToken == null)
            {
                ModelState.AddModelError("", "Invalid or expired code");
                return View(model: email);
            }

            validToken.IsUsed = true;
            user.LastLogin = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            // Sign In
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Email),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var identity = new ClaimsIdentity(claims, "CookieAuth");
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync("CookieAuth", principal);

            if (user.Role == "Admin")
            {
                return RedirectToAction("Index", "Admin");
            }

            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync("CookieAuth");
            return RedirectToAction("Index", "Home");
        }

        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}

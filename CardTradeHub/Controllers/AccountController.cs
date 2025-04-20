using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using CardTradeHub.Models;
using CardTradeHub.Models.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using CardTradeHub.Data;
using Microsoft.AspNetCore.Authorization;

namespace CardTradeHub.Controllers
{
    public class AccountController : Controller
    {
        private readonly CardTradeHubContext _context;
        private readonly IPasswordHasher<User> _passwordHasher;

        public AccountController(CardTradeHubContext context, IPasswordHasher<User> passwordHasher)
        {
            _context = context;
            _passwordHasher = passwordHasher;
        }

        // GET: /Account/Login
        [HttpGet]
        public IActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        // POST: /Account/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _context.Users
                    .FirstOrDefaultAsync(u => u.Email == model.Email);

                if (user != null && _passwordHasher.VerifyHashedPassword(null, user.PasswordHash, model.Password) == PasswordVerificationResult.Success)
                {
                    if (!user.IsActive)
                    {
                        ModelState.AddModelError("", "Your account has been deactivated.");
                        return View(model);
                    }

                    await SignInUser(user);
                    return RedirectToAction("Index", "Cards");
                }

                ModelState.AddModelError("", "Invalid email or password.");
            }

            return View(model);
        }

        // GET: /Account/Register
        [HttpGet]
        public IActionResult Register()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        // POST: /Account/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Check if user already exists
                var existingUser = await _context.Users
                    .FirstOrDefaultAsync(u => u.Email == model.Email || u.Username == model.Username);
                if (existingUser != null)
                {
                    if (existingUser.Email == model.Email)
                    {
                        ModelState.AddModelError("Email", "This email is already registered.");
                    }
                    if (existingUser.Username == model.Username)
                    {
                        ModelState.AddModelError("Username", "This username is already taken.");
                    }
                    return View(model);
                }

                // Create new user
                var user = new User
                {
                    Username = model.Username,
                    Email = model.Email,
                    PasswordHash = _passwordHasher.HashPassword(null, model.Password),
                    Role = model.Email.ToLower() == "admin@cardtradehub.com" ? "Admin" : "User",
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                };

                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                // Sign in the user
                await SignInUser(user);

                return RedirectToAction("Index", "Home");
            }

            return View(model);
        }

        // POST: /Account/Logout
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ResetAdminPassword()
        {
            var admin = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == "admin@cardtradehub.com");
            
            if (admin == null)
            {
                return NotFound("Admin user not found");
            }

            // Reset password to "Admin123!"
            admin.PasswordHash = _passwordHasher.HashPassword(null, "Admin123!");
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "Home");
        }

        private async Task SignInUser(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.UserID.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var authProperties = new AuthenticationProperties
            {
                IsPersistent = true,
                ExpiresUtc = DateTimeOffset.UtcNow.AddDays(7)
            };

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);
        }
    }
}
 
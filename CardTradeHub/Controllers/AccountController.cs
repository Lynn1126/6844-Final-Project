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
using System.Collections.Generic;
using CardTradeHub.Data;
using Microsoft.AspNetCore.Authorization;
using System.Linq;

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
            if (User?.Identity?.IsAuthenticated == true)
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
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(model);
                }

                var user = await _context.Users
                    .FirstOrDefaultAsync(u => u.Email == model.Email);

                if (user == null)
                {
                    ModelState.AddModelError("", "Invalid username or password");
                    return View(model);
                }

                var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash ?? "", model.Password);
                if (result == PasswordVerificationResult.Failed)
                {
                    ModelState.AddModelError("", "Invalid username or password");
                    return View(model);
                }

                if (!user.IsActive)
                {
                    ModelState.AddModelError("", "This account has been disabled");
                    return View(model);
                }
                
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.Email),
                    new Claim(ClaimTypes.NameIdentifier, user.UserID.ToString()),
                    new Claim("FullName", user.Username),
                    new Claim(ClaimTypes.Role, user.Role ?? "User")
                };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = model.RememberMe,
                    ExpiresUtc = DateTimeOffset.UtcNow.AddDays(7)
                };

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    authProperties);

                user.LastLoginDate = DateTime.UtcNow;
                await _context.SaveChangesAsync();

                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Login error: {ex.Message}");
                
                return View("Error", new ErrorViewModel
                {
                    ErrorTitle = "Login Failed",
                    ErrorMessage = "An error occurred during login. Please try again later.",
                    RequestId = HttpContext.TraceIdentifier
                });
            }
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
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(model);
                }

                // Check email domain
                if (!IsValidEmailDomain(model.Email))
                {
                    ModelState.AddModelError("Email", "Please use a valid email domain");
                    return View(model);
                }

                // Check if user exists
                var existingUser = await _context.Users
                    .FirstOrDefaultAsync(u => u.Email.ToLower() == model.Email.ToLower() || 
                                            u.Username.ToLower() == model.Username.ToLower());
                if (existingUser != null)
                {
                    if (existingUser.Email.ToLower() == model.Email.ToLower())
                    {
                        ModelState.AddModelError("Email", "This email has already been registered");
                    }
                    if (existingUser.Username.ToLower() == model.Username.ToLower())
                    {
                        ModelState.AddModelError("Username", "This username has already been taken");
                    }
                    return View(model);
                }

                // Create new user
                var user = new User
                {
                    Username = model.Username.Trim(),
                    Email = model.Email.Trim().ToLower(),
                    PasswordHash = _passwordHasher.HashPassword(null, model.Password),
                    Role = model.Email.ToLower() == "admin@cardtradehub.com" ? "Admin" : "User",
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow,
                    FirstName = model.FirstName?.Trim(),
                    LastName = model.LastName?.Trim(),
                    PhoneNumber = model.PhoneNumber?.Trim()
                };

                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                // Sign in user
                await SignInUser(user);

                return RedirectToAction("Index", "Home");
            }
            catch (DbUpdateException ex)
            {
                // Database error
                Console.WriteLine($"Database error: {ex.Message}");
                return View("Error", new ErrorViewModel
                {
                    ErrorTitle = "Registration Failed",
                    ErrorMessage = "An error occurred while saving user information. Please try again later.",
                    RequestId = HttpContext.TraceIdentifier
                });
            }
            catch (Exception ex)
            {
                // Other errors
                Console.WriteLine($"Registration error: {ex.Message}");
                return View("Error", new ErrorViewModel
                {
                    ErrorTitle = "Registration Failed",
                    ErrorMessage = "An error occurred during registration. Please try again later.",
                    RequestId = HttpContext.TraceIdentifier
                });
            }
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

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Profile()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null || !int.TryParse(userId, out int id))
            {
                return RedirectToAction("Login");
            }

            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            var model = new ProfileViewModel
            {
                Username = user.Username,
                Email = user.Email,
                Role = user.Role,
                CreatedAt = user.CreatedAt
            };

            return View(model);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Profile(ProfileViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null || !int.TryParse(userId, out int id))
            {
                return RedirectToAction("Login");
            }

            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            // 更新用户名
            if (user.Username != model.Username)
            {
                // 检查用户名是否已被使用
                if (await _context.Users.AnyAsync(u => u.Username == model.Username && u.UserID != id))
                {
                    ModelState.AddModelError("Username", "This username is already taken.");
                    return View(model);
                }
                user.Username = model.Username;
            }

            // 如果提供了当前密码和新密码，则更新密码
            if (!string.IsNullOrEmpty(model.CurrentPassword) && !string.IsNullOrEmpty(model.NewPassword))
            {
                var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, model.CurrentPassword);
                if (result == PasswordVerificationResult.Failed)
                {
                    ModelState.AddModelError("CurrentPassword", "Current password is incorrect");
                    return View(model);
                }

                user.PasswordHash = _passwordHasher.HashPassword(null, model.NewPassword);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("Profile", new { Message = "Profile updated" });
        }

        private async Task SignInUser(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.UserID.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role),
                new Claim("FullName", user.Username)
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

        // 用于重置用户密码的辅助方法
        private async Task<bool> ResetUserPassword(string email, string newPassword)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null)
            {
                return false;
            }

            user.PasswordHash = _passwordHasher.HashPassword(null, newPassword);
            await _context.SaveChangesAsync();
            return true;
        }

        [HttpGet]
        public IActionResult GoogleLogin()
        {
            var properties = new AuthenticationProperties { RedirectUri = Url.Action("GoogleResponse") };
            return Challenge(properties, "Google");
        }

        [HttpGet]
        public async Task<IActionResult> GoogleResponse()
        {
            var result = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            if (!result.Succeeded)
                return RedirectToAction("Login");

            var claims = result.Principal.Identities.FirstOrDefault()?.Claims;
            var email = claims?.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            
            if (string.IsNullOrEmpty(email))
                return RedirectToAction("Login");

            // 检查用户是否已存在
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null)
            {
                // 创建新用户
                var username = claims?.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value ?? email.Split('@')[0];
                user = new User
                {
                    Email = email,
                    Username = username,
                    PasswordHash = "GoogleAuth", // 标记为 Google 认证用户
                    Role = "Customer",
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow,
                    RegisterDate = DateTime.UtcNow,
                    LastLoginDate = DateTime.UtcNow,
                    IsEmailVerified = true
                };
                _context.Users.Add(user);
                await _context.SaveChangesAsync();
            }

            // 更新登录时间
            user.LastLoginDate = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            // 创建身份验证票据
            var identity = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.UserID.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role)
            }, CookieAuthenticationDefaults.AuthenticationScheme);

            var principal = new ClaimsPrincipal(identity);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

            return RedirectToAction("Index", "Home");
        }

        // Validate email domain
        private bool IsValidEmailDomain(string email)
        {
            if (string.IsNullOrEmpty(email))
                return false;

            var domain = email.Split('@').LastOrDefault();
            if (string.IsNullOrEmpty(domain))
                return false;

            // List of invalid domains
            var invalidDomains = new[] { "example.com", "test.com", "temporary.com" };
            return !invalidDomains.Contains(domain.ToLower());
        }
    }
}
 
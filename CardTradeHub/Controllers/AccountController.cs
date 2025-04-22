using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using CardTradeHub.Models;
using CardTradeHub.Models.ViewModels;
using Microsoft.EntityFrameworkCore;
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
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public AccountController(
            CardTradeHubContext context,
            UserManager<User> userManager,
            SignInManager<User> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        // GET: /Account/Login
        [HttpGet]
        public IActionResult Login(string returnUrl = null)
        {
            if (User?.Identity?.IsAuthenticated == true)
            {
                return RedirectToAction("Index", "Home");
            }
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        // POST: /Account/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null)
        {
            try
            {
                ViewData["ReturnUrl"] = returnUrl;
                if (!ModelState.IsValid)
                {
                    return View(model);
                }

                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user == null)
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return View(model);
                }

                var result = await _signInManager.PasswordSignInAsync(
                    user,
                    model.Password,
                    model.RememberMe,
                    lockoutOnFailure: true);

                if (result.Succeeded)
                {
                    user.LastLoginDate = DateTime.UtcNow;
                    await _context.SaveChangesAsync();

                    if (!await _userManager.IsInRoleAsync(user, user.Role))
                    {
                        await _userManager.AddToRoleAsync(user, user.Role);
                    }

                    if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                    {
                        return Redirect(returnUrl);
                    }
                    return RedirectToAction("Index", "Home");
                }

                if (result.IsLockedOut)
                {
                    return View("Lockout");
                }

                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                return View(model);
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
        public IActionResult Register(string returnUrl = null)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        // POST: /Account/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model, string returnUrl = null)
        {
            try
            {
                ViewData["ReturnUrl"] = returnUrl;
                if (!ModelState.IsValid)
                {
                    return View(model);
                }

                if (!IsValidEmailDomain(model.Email))
                {
                    ModelState.AddModelError("Email", "Please use a valid email domain");
                    return View(model);
                }

                var user = new User
                {
                    UserName = model.Username,
                    Email = model.Email,
                    Role = model.Email.ToLower() == "admin@cardtradehub.com" ? "Admin" : "User",
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow,
                    FirstName = model.FirstName?.Trim(),
                    LastName = model.LastName?.Trim(),
                    PhoneNumber = model.PhoneNumber?.Trim()
                };

                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, user.Role);
                    await _signInManager.SignInAsync(user, isPersistent: false);

                    if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                    {
                        return Redirect(returnUrl);
                    }
                    return RedirectToAction("Index", "Home");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return View(model);
            }
            catch (Exception ex)
            {
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
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ResetAdminPassword()
        {
            var admin = await _userManager.FindByEmailAsync("admin@cardtradehub.com");
            if (admin == null)
            {
                return NotFound("Admin user not found");
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(admin);
            var result = await _userManager.ResetPasswordAsync(admin, token, "Admin123!");

            if (!result.Succeeded)
            {
                return BadRequest("Failed to reset admin password");
            }

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Profile()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }

            var viewModel = new ProfileViewModel
            {
                Username = user.UserName,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                PhoneNumber = user.PhoneNumber,
                Role = user.Role,
                IsActive = user.IsActive,
                RegisterDate = user.CreatedAt,
                LastLoginDate = user.LastLoginDate
            };

            return View(viewModel);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Profile(ProfileViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }

            // 检查用户名是否已被使用
            if (user.UserName != model.Username)
            {
                var existingUser = await _userManager.FindByNameAsync(model.Username);
                if (existingUser != null)
                {
                    ModelState.AddModelError("Username", "This username is already taken");
                    return View(model);
                }
            }

            user.UserName = model.Username?.Trim();
            user.FirstName = model.FirstName?.Trim();
            user.LastName = model.LastName?.Trim();
            user.PhoneNumber = model.PhoneNumber?.Trim();

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return View(model);
            }

            // 更新成功后刷新用户的 Claims
            await _signInManager.RefreshSignInAsync(user);

            return RedirectToAction(nameof(Profile));
        }

        [HttpGet]
        public IActionResult GoogleLogin()
        {
            var properties = _signInManager.ConfigureExternalAuthenticationProperties("Google", Url.Action(nameof(GoogleResponse)));
            return Challenge(properties, "Google");
        }

        [HttpGet]
        public async Task<IActionResult> GoogleResponse()
        {
            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                return RedirectToAction(nameof(Login));
            }

            var result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false);
            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Home");
            }

            var email = info.Principal.FindFirstValue(ClaimTypes.Email);
            var username = email.Split('@')[0];
            var user = new User
            {
                UserName = username,
                Email = email,
                Role = "User",
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            var createResult = await _userManager.CreateAsync(user);
            if (createResult.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "User");
                await _userManager.AddLoginAsync(user, info);
                await _signInManager.SignInAsync(user, isPersistent: false);
                return RedirectToAction("Index", "Home");
            }

            return RedirectToAction(nameof(Login));
        }

        private bool IsValidEmailDomain(string email)
        {
            if (string.IsNullOrEmpty(email)) return false;

            var domain = email.Split('@').Last().ToLower();
            var validDomains = new[] { "gmail.com", "yahoo.com", "hotmail.com", "outlook.com", "cardtradehub.com" };
            return validDomains.Contains(domain);
        }
    }
}
 
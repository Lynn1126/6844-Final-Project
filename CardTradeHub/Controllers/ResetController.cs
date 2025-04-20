using Microsoft.AspNetCore.Mvc;
using CardTradeHub.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace CardTradeHub.Controllers
{
    public class ResetController : Controller
    {
        private readonly CardTradeHubContext _context;
        private readonly IPasswordHasher<Models.User> _passwordHasher;

        public ResetController(CardTradeHubContext context, IPasswordHasher<Models.User> passwordHasher)
        {
            _context = context;
            _passwordHasher = passwordHasher;
        }

        public async Task<IActionResult> ResetAdmin()
        {
            var admin = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == "admin@cardtradehub.com");
            
            if (admin == null)
            {
                return Content("Admin user not found");
            }

            // Reset password to "Admin123!"
            admin.PasswordHash = _passwordHasher.HashPassword(null, "Admin123!");
            await _context.SaveChangesAsync();

            return Content("Admin password has been reset to: Admin123!");
        }
    }
} 
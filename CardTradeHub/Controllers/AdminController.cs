using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using CardTradeHub.Data;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Threading.Tasks;
using CardTradeHub.Models;
using System.Linq;

namespace CardTradeHub.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly CardTradeHubContext _context;

        public AdminController(CardTradeHubContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        // User Management
        public async Task<IActionResult> ManageUsers()
        {
            var users = await _context.Users
                .OrderBy(u => u.Email)
                .ToListAsync();
            return View(users);
        }

        [HttpPost]
        public async Task<IActionResult> ToggleUserStatus(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            user.IsActive = !user.IsActive;
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(ManageUsers));
        }

        [HttpPost]
        public async Task<IActionResult> ChangeUserRole(int id, string role)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            user.Role = role;
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(ManageUsers));
        }

        // Card Management
        public async Task<IActionResult> ManageCards()
        {
            var cards = await _context.Cards
                .Include(c => c.User)
                .ToListAsync();
            return View(cards);
        }

        [HttpPost]
        public async Task<IActionResult> ToggleCardStatus(int id)
        {
            var card = await _context.Cards.FindAsync(id);
            if (card == null)
            {
                return NotFound();
            }

            card.Status = card.Status == "Available" ? "Unavailable" : "Available";
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(ManageCards));
        }

        // Transaction Management
        public async Task<IActionResult> ManageTrades()
        {
            var trades = await _context.Transactions
                .Include(t => t.Buyer)
                .Include(t => t.Seller)
                .Include(t => t.Card)
                .ToListAsync();
            return View(trades);
        }

        [HttpPost]
        public async Task<IActionResult> ResolveDispute(int id, string resolution)
        {
            var transaction = await _context.Transactions.FindAsync(id);
            if (transaction == null)
            {
                return NotFound();
            }

            transaction.DisputeStatus = resolution;
            transaction.HasDispute = false;
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(ManageTrades));
        }
    }
} 
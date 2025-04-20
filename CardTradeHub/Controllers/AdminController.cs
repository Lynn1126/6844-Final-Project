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

        public async Task<IActionResult> Index()
        {
            var users = await _context.Users
                .OrderBy(u => u.Username)
                .ToListAsync();
            return View(users);
        }

        // User Management
        public async Task<IActionResult> ManageUsers()
        {
            var users = await _context.Users
                .Include(u => u.Cards)
                .Include(u => u.Transactions)
                .OrderBy(u => u.Username)
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
                .OrderByDescending(c => c.ListedDate)
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
            var transactions = await _context.Transactions
                .Include(t => t.Card)
                .Include(t => t.Buyer)
                .Include(t => t.Seller)
                .OrderByDescending(t => t.Date)
                .ToListAsync();
            return View(transactions);
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
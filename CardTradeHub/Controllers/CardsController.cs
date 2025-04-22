using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using CardTradeHub.Models;
using CardTradeHub.Models.ViewModels;
using CardTradeHub.Data;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace CardTradeHub.Controllers
{
    public class CardsController : Controller
    {
        private readonly CardTradeHubContext _context;
        private readonly int _pageSize = 8; // 每页显示8张卡片

        public CardsController(CardTradeHubContext context)
        {
            _context = context;
        }

        // GET: Cards
        public async Task<IActionResult> Index(string category = null, string condition = null, string sortOrder = null, string searchString = null)
        {
            var cards = _context.Cards
                .Include(c => c.User)
                .Where(c => c.Status == "Available");

            if (!string.IsNullOrEmpty(category))
            {
                cards = cards.Where(c => c.Category == category);
            }

            if (!string.IsNullOrEmpty(condition))
            {
                cards = cards.Where(c => c.Condition == condition);
            }

            if (!string.IsNullOrEmpty(searchString))
            {
                cards = cards.Where(c => c.Title.Contains(searchString) || 
                                       c.Description.Contains(searchString) ||
                                       c.Category.Contains(searchString));
            }

            // Apply sorting
            cards = sortOrder switch
            {
                "price_desc" => cards.OrderByDescending(c => c.Price),
                "price_asc" => cards.OrderBy(c => c.Price),
                "date_desc" => cards.OrderByDescending(c => c.ListedDate),
                "date_asc" => cards.OrderBy(c => c.ListedDate),
                _ => cards.OrderByDescending(c => c.ListedDate)
            };

            var categories = await _context.Cards
                .Select(c => c.Category)
                .Distinct()
                .ToListAsync();

            var conditions = await _context.Cards
                .Select(c => c.Condition)
                .Distinct()
                .ToListAsync();

            ViewBag.Categories = categories;
            ViewBag.Conditions = conditions;
            ViewBag.CurrentCategory = category;
            ViewBag.CurrentCondition = condition;
            ViewBag.CurrentSort = sortOrder;
            ViewBag.CurrentSearch = searchString;

            return View(await cards.ToListAsync());
        }

        // GET: Cards/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var card = await _context.Cards
                .Include(c => c.User)
                .FirstOrDefaultAsync(m => m.CardID == id);

            if (card == null)
            {
                return NotFound();
            }

            return View(card);
        }

        // GET: Cards/MyCards
        [Authorize]
        public async Task<IActionResult> MyCards()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var cards = await _context.Cards
                .Where(c => c.UserID == userId)
                .OrderByDescending(c => c.ListedDate)
                .ToListAsync();

            var user = await _context.Users.FindAsync(userId);
            
            var viewModel = new MyCardsViewModel
            {
                Cards = cards,
                UserName = user?.UserName ?? "Unknown",
                TotalCards = cards.Count,
                TotalValue = cards.Sum(c => c.Price)
            };

            return View(viewModel);
        }

        // GET: Cards/Create
        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Cards/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create(Card card)
        {
            if (ModelState.IsValid)
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (userId == null)
                {
                    return RedirectToAction("Login", "Account");
                }

                card.UserID = userId;
                card.ListedDate = DateTime.UtcNow;
                card.Status = "Available";

                _context.Add(card);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(MyCards));
            }
            return View(card);
        }

        // GET: Cards/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var card = await _context.Cards.FindAsync(id);
            if (card == null)
            {
                return NotFound();
            }

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            if (card.UserID != userId)
            {
                return Forbid();
            }

            return View(card);
        }

        // POST: Cards/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Edit(int id, Card card)
        {
            if (id != card.CardID)
            {
                return NotFound();
            }

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            if (card.UserID != userId)
            {
                return Forbid();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(card);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CardExists(card.CardID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(MyCards));
            }
            return View(card);
        }

        // POST: Cards/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var card = await _context.Cards.FindAsync(id);
            if (card == null)
            {
                return NotFound();
            }

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            if (card.UserID != userId)
            {
                return Forbid();
            }

            _context.Cards.Remove(card);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(MyCards));
        }

        // GET: Cards/ListCard/5
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> ListCard(int id)
        {
            var card = await _context.Cards.FindAsync(id);
            if (card == null)
            {
                return NotFound();
            }

            // Check if the current user owns this card
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            if (card.UserID != userId)
            {
                return Forbid();
            }

            // Make the card available
            card.Status = "Available";
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Your card has been listed successfully!";
            return RedirectToAction(nameof(MyCards));
        }

        // GET: Cards/UnlistCard/5
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> UnlistCard(int id)
        {
            var card = await _context.Cards.FindAsync(id);
            if (card == null)
            {
                return NotFound();
            }

            // Check if the current user owns this card
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            if (card.UserID != userId)
            {
                return Forbid();
            }

            // Remove the card from public view
            card.Status = "Unlisted";
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Your card has been unlisted successfully!";
            return RedirectToAction(nameof(MyCards));
        }

        private bool CardExists(int id)
        {
            return _context.Cards.Any(e => e.CardID == id);
        }
    }
} 
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
        public async Task<IActionResult> Index(
            string category = null, 
            string condition = null, 
            string sortOrder = null, 
            string searchString = null,
            int page = 1)
        {
            var query = _context.Cards
                .Include(c => c.User)
                .Where(c => c.Status == "Available");

            if (!string.IsNullOrEmpty(category))
            {
                query = query.Where(c => c.Category == category);
            }

            if (!string.IsNullOrEmpty(condition))
            {
                query = query.Where(c => c.Condition == condition);
            }

            if (!string.IsNullOrEmpty(searchString))
            {
                query = query.Where(c => c.Title.Contains(searchString) || 
                                       c.Description.Contains(searchString) ||
                                       c.Category.Contains(searchString));
            }

            // Apply sorting
            query = sortOrder switch
            {
                "price_desc" => query.OrderByDescending(c => c.Price),
                "price_asc" => query.OrderBy(c => c.Price),
                "date_desc" => query.OrderByDescending(c => c.ListedDate),
                "date_asc" => query.OrderBy(c => c.ListedDate),
                _ => query.OrderByDescending(c => c.ListedDate)
            };

            var totalItems = await query.CountAsync();
            var items = await query
                .Skip((page - 1) * _pageSize)
                .Take(_pageSize)
                .ToListAsync();

            var categories = await _context.Cards
                .Select(c => c.Category)
                .Distinct()
                .ToListAsync();

            var conditions = await _context.Cards
                .Select(c => c.Condition)
                .Distinct()
                .ToListAsync();

            var viewModel = new CardListViewModel
            {
                Cards = items,
                Categories = categories,
                Conditions = conditions,
                CurrentCategory = category,
                CurrentCondition = condition,
                CurrentSort = sortOrder,
                CurrentSearch = searchString,
                PaginationInfo = new PaginationInfo
                {
                    CurrentPage = page,
                    ItemsPerPage = _pageSize,
                    TotalItems = totalItems
                }
            };

            return View(viewModel);
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
        public async Task<IActionResult> Create(CreateCardViewModel viewModel, IFormFile? ImageFile)
        {
            if (ModelState.IsValid)
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (userId == null)
                {
                    return RedirectToAction("Login", "Account");
                }

                string? imageUrl = null;
                if (ImageFile != null && ImageFile.Length > 0)
                {
                    // 确保 wwwroot/images/cards 目录存在
                    var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "cards");
                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                    }

                    // 生成唯一的文件名
                    var uniqueFileName = $"{Guid.NewGuid()}_{ImageFile.FileName}";
                    var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    // 保存文件
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await ImageFile.CopyToAsync(fileStream);
                    }

                    // 设置图片URL（相对路径）
                    imageUrl = $"/images/cards/{uniqueFileName}";
                }

                var card = new Card
                {
                    Title = viewModel.Title,
                    Description = viewModel.Description,
                    Category = viewModel.Category,
                    Condition = viewModel.Condition,
                    Price = viewModel.Price,
                    ImageUrl = imageUrl ?? "/images/card-placeholder.jpg", // 如果没有上传图片，使用默认图片
                    UserID = userId,
                    ListedDate = DateTime.UtcNow,
                    Status = "Available"
                };

                _context.Add(card);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(MyCards));
            }
            return View(viewModel);
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

            var viewModel = new CreateCardViewModel
            {
                Title = card.Title,
                Description = card.Description,
                Category = card.Category,
                Condition = card.Condition,
                Price = card.Price,
                ImageUrl = card.ImageUrl
            };

            return View(viewModel);
        }

        // POST: Cards/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Edit(int id, CreateCardViewModel viewModel)
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

            if (ModelState.IsValid)
            {
                try
                {
                    card.Title = viewModel.Title;
                    card.Description = viewModel.Description;
                    card.Category = viewModel.Category;
                    card.Condition = viewModel.Condition;
                    card.Price = viewModel.Price;
                    card.ImageUrl = viewModel.ImageUrl;

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
            return View(viewModel);
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
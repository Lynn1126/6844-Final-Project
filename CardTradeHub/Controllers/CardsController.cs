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

        // GET: /Cards
        public async Task<IActionResult> Index(string category = "", string searchTerm = "", int page = 1)
        {
            try
            {
                // 获取所有可用的类别
                var categories = await _context.Cards
                    .Select(c => c.Category)
                    .Distinct()
                    .OrderBy(c => c)
                    .ToListAsync();
                ViewBag.Categories = categories;
                ViewBag.SelectedCategory = category;
                ViewBag.SearchTerm = searchTerm;

                // 构建基础查询
                var query = _context.Cards
                    .Where(c => c.Status == "Available");

                // 应用类别筛选
                if (!string.IsNullOrEmpty(category))
                {
                    query = query.Where(c => c.Category == category);
                }

                // 应用搜索条件
                if (!string.IsNullOrEmpty(searchTerm))
                {
                    searchTerm = searchTerm.ToLower();
                    query = query.Where(c => 
                        c.Title.ToLower().Contains(searchTerm) ||
                        c.Description.ToLower().Contains(searchTerm) ||
                        c.Category.ToLower().Contains(searchTerm)
                    );
                }

                query = query.OrderByDescending(c => c.ListedDate)
                    .Include(c => c.User);

                // 获取总数
                var totalItems = await query.CountAsync();
                Console.WriteLine($"Total available cards found: {totalItems}");

                var totalPages = (int)Math.Ceiling(totalItems / (double)_pageSize);

                // 确保页码在有效范围内
                if (page < 1) page = 1;
                if (page > totalPages) page = totalPages;

                var skip = (page - 1) * _pageSize;

                // 执行分页查询
                var cards = await query
                    .Skip(skip)
                    .Take(_pageSize)
                    .ToListAsync();

                ViewBag.CurrentPage = page;
                ViewBag.TotalPages = totalPages;
                ViewBag.HasPreviousPage = page > 1;
                ViewBag.HasNextPage = page < totalPages;

                return View(cards);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in Index method: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
                throw;
            }
        }

        // GET: /Cards/Details/5
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
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdStr) || !int.TryParse(userIdStr, out int userId))
            {
                return RedirectToAction("Login", "Account");
            }

            var cards = await _context.Cards
                .Where(c => c.UserID == userId)
                .OrderByDescending(c => c.ListedDate)
                .ToListAsync();

            var viewModel = new MyCardsViewModel
            {
                UserCards = cards.Select(card => new CardViewModel
                {
                    CardID = card.CardID,
                    Title = card.Title,
                    Description = card.Description,
                    Category = card.Category,
                    Condition = card.Condition,
                    Status = card.Status,
                    Price = card.Price,
                    ImageUrl = card.ImageUrl
                })
            };

            return View(viewModel);
        }

        // GET: Cards/Create
        [HttpGet]
        [Authorize]
        public IActionResult Create()
        {
            return View(new CreateCardViewModel());
        }

        // POST: Cards/Create
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateCardViewModel model, IFormFile? ImageFile)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    string? imageUrl = null;
                    if (ImageFile != null && ImageFile.Length > 0)
                    {
                        // Generate a unique filename
                        var fileName = Path.GetRandomFileName() + Path.GetExtension(ImageFile.FileName);
                        var filePath = Path.Combine("wwwroot", "images", "cards", fileName);

                        // Ensure the directory exists
                        var directory = Path.GetDirectoryName(filePath);
                        if (directory != null && !Directory.Exists(directory))
                        {
                            Directory.CreateDirectory(directory);
                        }

                        // Save the file
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await ImageFile.CopyToAsync(stream);
                        }

                        // Set the URL (relative to wwwroot)
                        imageUrl = "/images/cards/" + fileName;
                    }

                    var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
                    var card = new Card
                    {
                        Title = model.Title,
                        Description = model.Description,
                        Category = model.Category,
                        Condition = model.Condition,
                        Price = model.Price,
                        ImageUrl = imageUrl,
                        UserID = userId,
                        ListedDate = DateTime.UtcNow,
                        Status = "Available"
                    };

                    _context.Add(card);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(MyCards));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Error creating card: " + ex.Message);
                }
            }
            return View(model);
        }

        // POST: Cards/Purchase/5
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Purchase(int id)
        {
            try
            {
                var card = await _context.Cards
                    .Include(c => c.User)
                    .FirstOrDefaultAsync(c => c.CardID == id);

                if (card == null)
                {
                    return Json(new { success = false, message = "Card not found." });
                }

                if (card.Status != "Available")
                {
                    return Json(new { success = false, message = "Card is not available for purchase." });
                }

                var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(userIdStr) || !int.TryParse(userIdStr, out int buyerId))
                {
                    return Json(new { success = false, message = "User not found." });
                }

                if (card.UserID == buyerId)
                {
                    return Json(new { success = false, message = "You cannot purchase your own card." });
                }

                var buyer = await _context.Users.FindAsync(buyerId);
                if (buyer == null)
                {
                    return Json(new { success = false, message = "Buyer not found." });
                }

                using var transaction = await _context.Database.BeginTransactionAsync();
                try
                {
                    var sellerId = card.UserID; // Save the original seller ID
                    var seller = card.User; // Save the original seller

                    // Update card status and owner
                    card.Status = "Sold";
                    card.UserID = buyerId; // Transfer ownership to the buyer
                    card.User = buyer; // Set the new owner
                    _context.Cards.Update(card);

                    // Create transaction record
                    var cardTransaction = new Transaction
                    {
                        CardID = card.CardID,
                        Card = card,
                        BuyerID = buyerId,
                        Buyer = buyer,
                        SellerID = sellerId,
                        Seller = seller,
                        Amount = card.Price,
                        Date = DateTime.UtcNow,
                        Status = "Completed",
                        PaymentMethod = "Direct",
                        TransactionReference = Guid.NewGuid().ToString(),
                        ShippingAddress = "Default Address", // This should be updated to use actual user address
                        TrackingNumber = "",
                        HasDispute = false,
                        DisputeReason = "",
                        DisputeStatus = "None"
                    };

                    _context.Transactions.Add(cardTransaction);
                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();

                    return Json(new { success = true, message = "Card purchased successfully!" });
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    return Json(new { success = false, message = $"An error occurred while processing your purchase: {ex.Message}" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"An unexpected error occurred: {ex.Message}" });
            }
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

            // Check if the current user owns this card
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
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
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CreateCardViewModel model, IFormFile ImageFile)
        {
            if (id == 0)
            {
                return NotFound();
            }

            var card = await _context.Cards.FindAsync(id);
            if (card == null)
            {
                return NotFound();
            }

            // Check if the current user owns this card
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            if (card.UserID != userId)
            {
                return Forbid();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // 处理图片上传
                    if (ImageFile != null && ImageFile.Length > 0)
                    {
                        // 验证文件类型
                        var allowedTypes = new[] { "image/jpeg", "image/png", "image/gif" };
                        if (!allowedTypes.Contains(ImageFile.ContentType.ToLower()))
                        {
                            ModelState.AddModelError("ImageFile", "Only JPG, PNG and GIF files are allowed.");
                            return View(model);
                        }

                        // 验证文件大小（5MB）
                        if (ImageFile.Length > 5 * 1024 * 1024)
                        {
                            ModelState.AddModelError("ImageFile", "File size cannot exceed 5MB.");
                            return View(model);
                        }

                        // 获取文件扩展名并验证
                        var extension = Path.GetExtension(ImageFile.FileName).ToLowerInvariant();
                        var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
                        if (string.IsNullOrEmpty(extension) || !allowedExtensions.Contains(extension))
                        {
                            ModelState.AddModelError("ImageFile", "Invalid file type. Only .jpg, .jpeg, .png, and .gif files are allowed.");
                            return View(model);
                        }

                        // 创建上传目录
                        var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "cards");
                        if (!Directory.Exists(uploadsFolder))
                        {
                            Directory.CreateDirectory(uploadsFolder);
                        }

                        // 生成安全的文件名：时间戳_用户ID_GUID.扩展名
                        var timestamp = DateTime.UtcNow.ToString("yyyyMMddHHmmss");
                        var safeFileName = $"{timestamp}_{userId}_{Guid.NewGuid():N}{extension}";
                        var filePath = Path.Combine(uploadsFolder, safeFileName);

                        // 检查文件是否已存在（以防万一）
                        if (System.IO.File.Exists(filePath))
                        {
                            safeFileName = $"{timestamp}_{userId}_{Guid.NewGuid():N}{extension}";
                            filePath = Path.Combine(uploadsFolder, safeFileName);
                        }

                        // 删除旧图片文件（如果存在）
                        if (!string.IsNullOrEmpty(card.ImageUrl))
                        {
                            var oldFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "cards",
                                card.ImageUrl.TrimStart('/'));
                            if (System.IO.File.Exists(oldFilePath))
                            {
                                try
                                {
                                    System.IO.File.Delete(oldFilePath);
                                }
                                catch (Exception ex)
                                {
                                    // 记录错误但继续执行，因为这不是关键操作
                                    Console.WriteLine($"Error deleting old image: {ex.Message}");
                                }
                            }
                        }

                        // 保存新文件
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await ImageFile.CopyToAsync(stream);
                        }

                        // 更新数据库中的图片URL
                        card.ImageUrl = $"/images/cards/{safeFileName}";
                    }

                    // 更新其他字段
                    card.Title = model.Title;
                    card.Description = model.Description;
                    card.Category = model.Category;
                    card.Condition = model.Condition;
                    card.Price = model.Price;

                    _context.Update(card);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CardExists(id))
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
            return View(model);
        }

        // POST: Cards/Delete/5
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var card = await _context.Cards.FindAsync(id);
            if (card == null)
            {
                return NotFound();
            }

            // Check if the current user owns this card
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            if (card.UserID != userId)
            {
                return Forbid();
            }

            // Instead of deleting, mark the card as deleted
            card.Status = "Deleted";
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
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
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
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
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
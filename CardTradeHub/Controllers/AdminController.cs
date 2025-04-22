using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using CardTradeHub.Data;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Threading.Tasks;
using CardTradeHub.Models;
using System.Linq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.AspNetCore.Identity;
using CardTradeHub.Models.ViewModels;

namespace CardTradeHub.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly CardTradeHubContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly UserManager<User> _userManager;

        public AdminController(CardTradeHubContext context, IWebHostEnvironment webHostEnvironment, UserManager<User> userManager)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
            _userManager = userManager;
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
        public async Task<IActionResult> ToggleUserStatus(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            user.IsActive = !user.IsActive;
            await _userManager.UpdateAsync(user);

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

            return Json(new { success = true, newStatus = card.Status });
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

        // GET: Admin/EditCard/5
        public async Task<IActionResult> EditCard(int? id)
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

            var viewModel = new CreateCardViewModel
            {
                CardID = card.CardID,
                Title = card.Title,
                Description = card.Description,
                Category = card.Category,
                Condition = card.Condition,
                Price = card.Price,
                Status = card.Status,
                ImageUrl = card.ImageUrl
            };

            return View(viewModel);
        }

        // POST: Admin/EditCard/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditCard(int id, CreateCardViewModel model, IFormFile? ImageFile)
        {
            if (id != model.CardID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var card = await _context.Cards.FindAsync(id);
                    if (card == null)
                    {
                        return NotFound();
                    }

                    // 处理图片上传
                    if (ImageFile != null)
                    {
                        var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads");
                        var uniqueFileName = Guid.NewGuid().ToString() + "_" + ImageFile.FileName;
                        var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await ImageFile.CopyToAsync(fileStream);
                        }

                        // 删除旧图片
                        if (!string.IsNullOrEmpty(card.ImageUrl))
                        {
                            var oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, card.ImageUrl.TrimStart('/'));
                            if (System.IO.File.Exists(oldImagePath))
                            {
                                System.IO.File.Delete(oldImagePath);
                            }
                        }

                        card.ImageUrl = "/uploads/" + uniqueFileName;
                    }

                    // 更新卡片信息
                    card.Title = model.Title;
                    card.Description = model.Description;
                    card.Category = model.Category;
                    card.Condition = model.Condition;
                    card.Price = model.Price;
                    card.Status = model.Status;

                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(ManageCards));
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
            }
            return View(model);
        }

        // GET: Admin/EditUser/5
        public async Task<IActionResult> EditUser(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: Admin/EditUser/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditUser(string id, User model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var user = await _userManager.FindByIdAsync(id);
                    if (user == null)
                    {
                        return NotFound();
                    }

                    user.UserName = model.UserName;
                    user.Email = model.Email;
                    user.FirstName = model.FirstName;
                    user.LastName = model.LastName;
                    user.IsActive = model.IsActive;

                    // 更新用户信息
                    var result = await _userManager.UpdateAsync(user);
                    if (!result.Succeeded)
                    {
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError("", error.Description);
                        }
                        return View(model);
                    }

                    // 更新用户角色
                    var currentRoles = await _userManager.GetRolesAsync(user);
                    await _userManager.RemoveFromRolesAsync(user, currentRoles);
                    await _userManager.AddToRoleAsync(user, model.Role);

                    return RedirectToAction(nameof(ManageUsers));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            return View(model);
        }

        // GET: Admin/TradeDetails/5
        public async Task<IActionResult> TradeDetails(int id)
        {
            var transaction = await _context.Transactions
                .Include(t => t.Card)
                .Include(t => t.Buyer)
                .Include(t => t.Seller)
                .FirstOrDefaultAsync(t => t.TransactionID == id);

            if (transaction == null)
            {
                return NotFound();
            }

            return View(transaction);
        }

        // POST: Admin/UpdateTradeStatus
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateTradeStatus(int id, string status)
        {
            var transaction = await _context.Transactions
                .Include(t => t.Card)
                .FirstOrDefaultAsync(t => t.TransactionID == id);

            if (transaction == null)
            {
                return NotFound();
            }

            if (!new[] { "Pending", "Completed", "Cancelled" }.Contains(status))
            {
                return BadRequest("Invalid status");
            }

            transaction.Status = status;
            
            if (status == "Completed")
            {
                // Update card status to Sold if the transaction is completed
                if (transaction.Card != null)
                {
                    transaction.Card.Status = "Sold";
                }
            }
            else if (status == "Cancelled")
            {
                // Make the card available again if the transaction is cancelled
                if (transaction.Card != null)
                {
                    transaction.Card.Status = "Available";
                }
            }

            await _context.SaveChangesAsync();
            return Ok();
        }

        private bool CardExists(int id)
        {
            return _context.Cards.Any(e => e.CardID == id);
        }

        private bool UserExists(string id)
        {
            return _context.Users.Any(e => e.Id == id);
        }
    }
} 
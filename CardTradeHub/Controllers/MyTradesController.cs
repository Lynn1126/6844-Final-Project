using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using CardTradeHub.Models;
using CardTradeHub.Models.ViewModels;
using CardTradeHub.Data;
using System.Security.Claims;
using System.Linq;
using System.Threading.Tasks;

namespace CardTradeHub.Controllers
{
    [Authorize] // 确保只有登录用户可以访问
    public class MyTradesController : Controller
    {
        private readonly CardTradeHubContext _context;

        public MyTradesController(CardTradeHubContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "0");
                if (userId == 0)
                {
                    return RedirectToAction("Login", "Account");
                }

                var purchases = await _context.Transactions
                    .Include(t => t.Card)
                    .Include(t => t.Seller)
                    .Where(t => t.BuyerID == userId)
                    .OrderByDescending(t => t.Date)
                    .ToListAsync();

                var sales = await _context.Transactions
                    .Include(t => t.Card)
                    .Include(t => t.Buyer)
                    .Where(t => t.SellerID == userId)
                    .OrderByDescending(t => t.Date)
                    .ToListAsync();

                var viewModel = new MyTradesViewModel
                {
                    Purchases = purchases,
                    Sales = sales
                };

                return View(viewModel);
            }
            catch (Exception ex)
            {
                // Log the error
                return RedirectToAction("Error", "Home", new { message = "An error occurred while loading your trades." });
            }
        }

        public async Task<IActionResult> TradeDetails(int id)
        {
            try
            {
                var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "0");
                if (userId == 0)
                {
                    return RedirectToAction("Login", "Account");
                }

                var transaction = await _context.Transactions
                    .Include(t => t.Card)
                    .Include(t => t.Buyer)
                    .Include(t => t.Seller)
                    .FirstOrDefaultAsync(t => t.TransactionID == id && (t.BuyerID == userId || t.SellerID == userId));

                if (transaction == null)
                {
                    return NotFound();
                }

                return View(transaction);
            }
            catch (Exception ex)
            {
                // Log the error
                return RedirectToAction("Error", "Home", new { message = "An error occurred while loading the trade details." });
            }
        }
    }
} 
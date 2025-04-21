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
    [Authorize]
    public class TransactionsController : Controller
    {
        private readonly CardTradeHubContext _context;

        public TransactionsController(CardTradeHubContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

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

        public async Task<IActionResult> TradeDetails(int id)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

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
    }
} 
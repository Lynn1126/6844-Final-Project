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

        public async Task<IActionResult> TradeDetails(string? id)
        {
            if (string.IsNullOrEmpty(id) || !int.TryParse(id, out int tradeId))
            {
                return NotFound();
            }

            var transaction = await _context.Transactions
                .Include(t => t.Card)
                .Include(t => t.Buyer)
                .Include(t => t.Seller)
                .FirstOrDefaultAsync(t => t.TransactionID == tradeId);

            if (transaction == null)
            {
                return NotFound();
            }

            var viewModel = new TradeDetailsViewModel
            {
                TradeID = transaction.TransactionID,
                CardID = transaction.CardID,
                Price = transaction.Amount,
                TradeDate = transaction.Date,
                CardTitle = transaction.Card?.Title ?? "Unknown",
                CardCategory = transaction.Card?.Category ?? "Unknown",
                Status = transaction.Status,
                SellerName = transaction.Seller?.Username ?? "Unknown",
                TrackingNumber = transaction.TrackingNumber
            };

            return View(viewModel);
        }
    }
} 
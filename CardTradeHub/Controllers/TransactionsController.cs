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

        public async Task<IActionResult> MyTrades()
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
    }
} 
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
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
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

        [HttpPost]
        public async Task<IActionResult> InitiatePurchase(int cardId)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var card = await _context.Cards
                .Include(c => c.User)
                .FirstOrDefaultAsync(c => c.CardID == cardId);

            if (card == null)
            {
                return NotFound();
            }

            if (card.Status != "Available")
            {
                TempData["Error"] = "This card is no longer available for purchase.";
                return RedirectToAction("Details", "Cards", new { id = cardId });
            }

            if (card.UserID == userId)
            {
                TempData["Error"] = "You cannot purchase your own card.";
                return RedirectToAction("Details", "Cards", new { id = cardId });
            }

            var transaction = new Transaction
            {
                CardID = cardId,
                BuyerID = userId,
                SellerID = card.UserID,
                Amount = card.Price,
                Date = DateTime.UtcNow,
                Status = "Pending",
                PaymentMethod = "Direct",
                TransactionReference = Guid.NewGuid().ToString(),
                ShippingAddress = "To be provided",
                TrackingNumber = string.Empty,
                DisputeReason = string.Empty,
                DisputeStatus = "None"
            };

            card.Status = "Pending";

            _context.Transactions.Add(transaction);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Purchase initiated successfully.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> UpdateStatus(int transactionId, string status)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var transaction = await _context.Transactions
                .Include(t => t.Card)
                .FirstOrDefaultAsync(t => t.TransactionID == transactionId);

            if (transaction == null)
            {
                return NotFound();
            }

            if (transaction.SellerID != userId && transaction.BuyerID != userId)
            {
                return Forbid();
            }

            transaction.Status = status;
            if (status == "Completed")
            {
                transaction.Card.Status = "Sold";
            }
            else if (status == "Cancelled")
            {
                transaction.Card.Status = "Available";
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> UpdateShippingAddress(int transactionId, string address)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var transaction = await _context.Transactions
                .FirstOrDefaultAsync(t => t.TransactionID == transactionId);

            if (transaction == null)
            {
                return NotFound();
            }

            if (transaction.BuyerID != userId)
            {
                return Forbid();
            }

            transaction.ShippingAddress = address;
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> UpdateTrackingNumber(int transactionId, string trackingNumber)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var transaction = await _context.Transactions
                .FirstOrDefaultAsync(t => t.TransactionID == transactionId);

            if (transaction == null)
            {
                return NotFound();
            }

            if (transaction.SellerID != userId)
            {
                return Forbid();
            }

            transaction.TrackingNumber = trackingNumber;
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> InitiateDispute(int transactionId, string reason)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var transaction = await _context.Transactions
                .FirstOrDefaultAsync(t => t.TransactionID == transactionId);

            if (transaction == null)
            {
                return NotFound();
            }

            if (transaction.BuyerID != userId && transaction.SellerID != userId)
            {
                return Forbid();
            }

            transaction.HasDispute = true;
            transaction.DisputeReason = reason;
            transaction.DisputeDate = DateTime.UtcNow;
            transaction.DisputeStatus = "Pending";

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
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
                SellerName = transaction.Seller?.UserName ?? "Unknown",
                TrackingNumber = transaction.TrackingNumber
            };

            return View(viewModel);
        }
    }
} 
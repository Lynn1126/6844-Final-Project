using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using CardTradeHub.Models;
using CardTradeHub.Data;
using Microsoft.EntityFrameworkCore;
using CardTradeHub.Models.ViewModels;

namespace CardTradeHub.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly CardTradeHubContext _context;

    public HomeController(ILogger<HomeController> logger, CardTradeHubContext context)
    {
        _logger = logger;
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var availableCards = await _context.Cards
            .Where(c => c.Status == "Available")
            .Include(c => c.User)
            .OrderByDescending(c => c.ListedDate)
            .Take(9)
            .ToListAsync();

        return View(availableCards);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error(string? message = null)
    {
        return View(new ErrorViewModel
        {
            RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
            ErrorTitle = "Error",
            ErrorMessage = message ?? "An unexpected error occurred. Please try again later."
        });
    }
}

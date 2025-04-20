using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using CardTradeHub.Models;
using CardTradeHub.Data;
using Microsoft.EntityFrameworkCore;

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
        var latestCards = await _context.Cards
            .OrderByDescending(c => c.ListedDate)
            .Take(3)
            .ToListAsync();

        return View(latestCards);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}

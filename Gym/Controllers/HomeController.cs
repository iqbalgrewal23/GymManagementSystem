using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Gym.Models;

namespace Gym.Controllers;

/// <summary>
/// Handles basic navigation for the application such as Home, Privacy, and Error pages.
/// </summary>
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    /// <summary>
    /// Constructor for HomeController that accepts a logger.
    /// </summary>
    /// <param name="logger">Logger instance for logging information.</param>
    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Displays the home page.
    /// </summary>
    /// <returns>The Home/Index view.</returns>
    public IActionResult Index()
    {
        return View();
    }

    /// <summary>
    /// Displays the privacy policy page.
    /// </summary>
    /// <returns>The Home/Privacy view.</returns>
    public IActionResult Privacy()
    {
        return View();
    }

    /// <summary>
    /// Handles application errors and displays error information.
    /// </summary>
    /// <returns>The Home/Error view with an error model.</returns>
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}

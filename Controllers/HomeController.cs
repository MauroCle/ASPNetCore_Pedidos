using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Examen.Models;
using Examenes.ViewModels;
using Examenes.Services;

namespace Examen.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IHomeService _homeService;

    public HomeController(ILogger<HomeController> logger, IHomeService homeService)
    {
        _logger = logger;
        _homeService = homeService;

    }


    public async Task<IActionResult> Index()
    {
        HomeViewModel homeViewModel = new HomeViewModel
        {
            AvailableClients = await _homeService.AnyClientAvailable(),
            AvailableAddress = await _homeService.AnyAddressAvailable(),
            AvailableOrders = await _homeService.AnyOrderAvailable(),
            AvailableProducts = await _homeService.AnyProductAvailable()

        };
        return View(homeViewModel);
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

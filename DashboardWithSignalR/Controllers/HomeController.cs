using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using DashboardWithSignalR.Models;
using Microsoft.AspNetCore.SignalR;

namespace DashboardWithSignalR.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IHubContext<ChartHub> _hub;

    public HomeController(ILogger<HomeController> logger, IHubContext<ChartHub> hub)
    {
        _logger = logger;
        _hub = hub;
    }

    public async Task PushUpdates()
    {
        var rnd = new Random();

        await _hub.Clients.All.SendAsync("UpdateLineChart", rnd.Next(10, 90));

        await _hub.Clients.All.SendAsync("UpdateBarChart", new[] {
            rnd.Next(50, 100),
            rnd.Next(1, 20)
        });

        await _hub.Clients.All.SendAsync("UpdateDoughnutChart", new[] {
            rnd.Next(20, 40),
            rnd.Next(10, 30),
            rnd.Next(5, 20)
        });
    }

    [HttpGet]
    public async Task<IActionResult> Update()
    {
        await PushUpdates();

        return Ok();
    }

    public IActionResult Index()
    {
        return View();
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

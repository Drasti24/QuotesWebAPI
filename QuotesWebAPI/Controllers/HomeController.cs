//DRASTI PATEL
//MARCH 30, 2025
//PROLEM ANALYSIS 03

using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using QuotesWebAPI.Models;

namespace QuotesWebAPI.Controllers
{
    //default controller or basic site pages like Index, Privacy and Error
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        //constructor with dependency injection for logging
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }
        // GET: /
        //loads the default home/index view
        public IActionResult Index()
        {
            return View();
        }

        // GET: /Privacy
        //loads the priavcy policy view
        public IActionResult Privacy()
        {
            return View();
        }
        // GET: /Error
        //handles error responsesand passes request ID to view for debugging
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

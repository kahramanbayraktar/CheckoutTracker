using Customer.Utils.Utils;
using Customer.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Customer.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IWebHostEnvironment _environment;
        private readonly IConfiguration _config;

        public HomeController(ILogger<HomeController> logger, IWebHostEnvironment environment, IConfiguration config)
        {
            _logger = logger;
            _environment = environment;
            _config = config;
        }

        public IActionResult Index()
        {
            var path = Path.Combine(_environment.WebRootPath, _config["Data:FilePath"]!);
            var content = System.IO.File.ReadAllLines(path);

            var model = CustomerCsvParser.Parse(content);

            return View(model);
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
}
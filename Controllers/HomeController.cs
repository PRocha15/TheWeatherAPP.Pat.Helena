using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;
using TheWeatherAPP.Pat.Helena.Models;

namespace TheWeatherAPP.Pat.Helena.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult Validation(string email, string password)
        {
            if (email == "superMan@super.com" && password == "123")
            {
                return View("Main");
            }
            else
            {
                ViewData["auth_error"] = "Por favor introduza as suas credencias";
                return View("Index");

            }
        }
        public IActionResult Results()
        {
            return View();
        }

        [HttpPost]

        public async Task<ActionResult> WeatherAPIForm(Weather wm)
        {
            //HTTP request CALL API

            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Get, "https://weather.visualcrossing.com/VisualCrossingWebServices/rest/services/timeline/" + wm.LocationName + "?key=9ML3SDK9ZECE68356PEA4G45V");
            var response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode(); //método para tratamento de exceções

            var body = await response.Content.ReadAsStringAsync();
            dynamic weather = JsonConvert.DeserializeObject(body);
            List<string> results = new List<string>();

            foreach (var day in weather.days)
            {
                results.Add("Forecast for date " + day.datetime);
                results.Add("General conditions will be: " + day.description);
                results.Add(" ");
            }

            ViewBag.Output = results;
            return View("Main", wm);

            ViewBag.Output = body;

            return View("Main");

        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
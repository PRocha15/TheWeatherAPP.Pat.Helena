using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Net.Mime;
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
                Weather wm = new Weather();
                wm.period = "Today";
                return View("Main", wm);
            }
            else
            {
                ViewData["auth_error"] = "Por favor introduza as suas credenciais";
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

            //Check the intended period
            string API_URL;
            switch (wm.Period)
            {
                case "Today":
                    API_URL = "https://weather.visualcrossing.com/VisualCrossingWebServices/rest/services/timeline/" + wm.LocationName + "/today?unitGroup=metric&key=KT998THUCKM395HUR6LLQRWYH&contentType=json";
                    break;
                case "Tomorrow":
                    API_URL = "https://weather.visualcrossing.com/VisualCrossingWebServices/rest/services/timeline/" + wm.LocationName + "/tomorrow?unitGroup=metric&key=KT998THUCKM395HUR6LLQRWYH&contentType=json";
                    break;
                case "Next 7 days":
                    API_URL = "https://weather.visualcrossing.com/VisualCrossingWebServices/rest/services/timeline/" + wm.LocationName + "/next7days?unitGroup=metric&key=9ML3SDK9ZECE68356PEA4G45V&contentType=json";
                    break;
                default: //next 15 days
                    API_URL = "https://weather.visualcrossing.com/VisualCrossingWebServices/rest/services/timeline/" + wm.LocationName + "/?unitGroup=metric&key=9ML3SDK9ZECE68356PEA4G45V&contentType=json";
                    break;              
            }
            //HTTP request CALL API

            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Get, API_URL);
           var response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode(); //método para tratamento de exceções
            var body = await response.Content.ReadAsStringAsync();
            string result = " ";
            dynamic weather = JsonConvert.DeserializeObject(body);
            List<string> results = new List<string>();

            foreach (var day in weather.days)
            {
                results.Add("Forecast for date " + day.datetime);
                results.Add("General conditions will be: " + day.description);
                results.Add(" The high temperature will be: " + day.tempmax);
                results.Add(" The low temperature will be: " + day.tempmin);
                results.Add("The sunrise hour will be: " + day.sunrise);
                results.Add("The sunset hour will be: " + day.sunset);
                results.Add("estacoes: " + day.stations);
                results.Add("alertas: " + day.alerts);
               // results.Add("Weather warnings for today: " + day.alerts);
                results.Add(" ");

            }

            //dynamic estacao = JsonConvert.DeserializeObject(body);
            //List<string> stationsId = new List<string>();
            //foreach(var i in estacao.days)
            //{
            //    stationsId.Add("A estaçao é " + i.stations);
            //}

            //dynamic alert = JsonConvert.DeserializeObject(body);
            //List<string> WeatherAlerts = new List<string>();
            //foreach(var i in alert.days)
            //{
            //    WeatherAlerts.Add("Alerta para hoje: " + i.alerts);
            //}




            // dynamic alert = JsonConvert.DeserializeObject(body);


            // stationsIds = weather.stationsIds["stations"];
            // results.Add("Weather stations: " + weather.stations);

            //dynamic estacao = JsonConvert.DeserializeObject(body);
            //List<string> stationsIds = new List<string>();

            //foreach( var station in estacao.days)
            //{
            //        stationsIds.Add("Weather statiuons: " + station.stations);
            //}

            ViewBag.Output = results; 
         
            //ViewBag.Output = stationsId;
           // ViewBag.Output = WeatherAlerts;
            return View("Results", wm);


            ViewBag.Output = body;

            return View("Results");

        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
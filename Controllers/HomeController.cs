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
                    API_URL = "https://weather.visualcrossing.com/VisualCrossingWebServices/rest/services/timeline/" + wm.LocationName + "/today?unitGroup=metric&key=9ML3SDK9ZECE68356PEA4G45V&contentType=json";
                    break;
                case "Tomorrow":
                    API_URL = "https://weather.visualcrossing.com/VisualCrossingWebServices/rest/services/timeline/" + wm.LocationName + "/tomorrow?unitGroup=metric&key=KT998THUCKM395HUR6LLQRWYH&contentType=json";
                    break;
                case "Next 7 days":
                    API_URL = "https://weather.visualcrossing.com/VisualCrossingWebServices/rest/services/timeline/" + wm.LocationName + "/next7days?unitGroup=metric&key=9ML3SDK9ZECE68356PEA4G45V&contentType=json";
                    break;
                default: //next 15 days
                    API_URL = "https://weather.visualcrossing.com/VisualCrossingWebServices/rest/services/timeline/" + wm.LocationName + "?unitGroup=metric&key=9ML3SDK9ZECE68356PEA4G45V&contentType=json";
                    break;
            }
            //HTTP request CALL API

            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Get, API_URL);
           var response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode(); //método para tratamento de exceções
            var body = await response.Content.ReadAsStringAsync();
           
            //string result = "";
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
                results.Add("The UV radiation will be: " + day.uvindex);

                // results.Add("As estações da cidade são: " + day.stations);
                // results.Add("alertas: " + body.alerts);
                // results.Add("Weather warnings for today: " + day.alerts);
                results.Add(" ");
            }

            //informação sobre as estaçoes de cada localização;
            List<string> stationsIds = new List<string>();

            results.Add("****************************");
            results.Add("Here is the information about the associated stations for this location: ");

            var weatherStations = weather.currentConditions["stations"];
            string stationDetails = "";
            foreach(string stationID in weatherStations)

            {
                results.Add("Station Name: " + weather.stations[stationID].name);
                results.Add("ID: " + weather.stations[stationID].id);
                results.Add("Distance: " + weather.stations[stationID].distance);
                results.Add("Latitude: " + weather.stations[stationID].latitude);
                results.Add("Longitude: " + weather.stations[stationID].longitude);
                results.Add("Quality: " + weather.stations[stationID].quality);
                results.Add(" ");
            }

            //dynamic severe = JsonConvert.DeserializeObject(body);

            List<string> WeatherAlerts = new List<string>();

            foreach (var alert in weather)
            {
                if (WeatherAlerts != null)
                {
                    results.Add("Alerta para hoje: " + weather.alerts[0].headline);
                }
                break;
            }


            //var weatherAlerts = weather.currentConditions["alerts"];
            //dynamic severe = JsonConvert.DeserializeObject(body);
            //string alertDetails = "";
            //List<string> WeatherAlerts = new List<string>();

            //foreach (string alert in weatherAlerts.days)
            //{
            //    if (WeatherAlerts != null)
            //    {
            //        results.Add("Alerta para hoje: " + weather.alerts[WeatherAlerts]);
            //    }

            //}

            ViewBag.Output = results;
           //ViewBag.Output = WeatherAlerts;
         
            //ViewBag.Output = stationsId;
           // ViewBag.Output = WeatherAlerts;
            return View("Results", wm);


            //ViewBag.Output = body;

            //return View("Results");

        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
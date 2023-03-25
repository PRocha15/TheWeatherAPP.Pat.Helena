using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Diagnostics;
using System.Net.Mime;
using TheWeatherAPP.Pat.Helena.Data;
using TheWeatherAPP.Pat.Helena.Models;

namespace TheWeatherAPP.Pat.Helena.Controllers
{
    public class HomeController : Controller
    {
        private readonly TheWeatherAPPPatHelenaContext _context;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, TheWeatherAPPPatHelenaContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Main()
        {
            Weather wm = new Weather();
            wm.period = "Today";
            return View("Main", wm);
        }

        public async Task<IActionResult> Validation(string email, string password)
        {
            // query database for user
            List<Users> users = await _context.Users.ToListAsync();
            List<Users> validUsers = users.FindAll(user => user.Email == email && user.Password == password);

            if (validUsers.Count > 0)
            {   
                Weather wm = new Weather();
                wm.period = "Today";
                return View("Main", wm);
            }
            else
            {
                ViewData["auth_error"] = "User or password invalid";
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
            string API_URL="";
            switch (wm.Period)
            {
                case "Today":
                    API_URL = "https://weather.visualcrossing.com/VisualCrossingWebServices/rest/services/timeline/" + wm.LocationName + "/today?iconSet=icons2&unitGroup=metric&key=9ML3SDK9ZECE68356PEA4G45V&contentType=json";
                    break;
                case "Tomorrow":
                    API_URL = "https://weather.visualcrossing.com/VisualCrossingWebServices/rest/services/timeline/" + wm.LocationName + "/tomorrow?iconSet=icons2&unitGroup=metric&key=KT998THUCKM395HUR6LLQRWYH&contentType=json";
                    break;
                case "Next 7 days":
                    API_URL = "https://weather.visualcrossing.com/VisualCrossingWebServices/rest/services/timeline/" + wm.LocationName + "/next7days?iconSet=icons2&unitGroup=metric&key=9ML3SDK9ZECE68356PEA4G45V&contentType=json";
                    break;
                case "Next 15 days": //next 15 days
                    API_URL = "https://weather.visualcrossing.com/VisualCrossingWebServices/rest/services/timeline/" + wm.LocationName + "/next15days?iconSet=icons2&unitGroup=metric&key=9ML3SDK9ZECE68356PEA4G45V&contentType=json";
                    break;
            }
            
            //HTTP request CALL API Visual Crossing
            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Get, API_URL);
            var response = await client.SendAsync(request);
            if (!response.IsSuccessStatusCode)
            {
                // in case of error show error page
                ErrorViewModel errorModel = new ErrorViewModel();
                return View("Error", errorModel);
            }
            
            var body = await response.Content.ReadAsStringAsync();

            //string result = "";
            bool error = true;
            dynamic responseJSON = null;
            try
            {
                responseJSON = JsonConvert.DeserializeObject(body);
                error = false;
            } catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
           
            // Parse Visual Crossing Response for View Model
            WeatherResult weatherResult = new WeatherResult(responseJSON);


            //Image API HTTP request
            client = new HttpClient();
            // async wait get image api
            string imageLink = null;
            string imageAPI = "https://api.unsplash.com/search/photos?page=1&query=" + wm.locationName.ToLower() + "&client_id=0hyxSmBqyWQc8Cw1AsayLABdynhWpRgbtFEwS21TKEE";
            request = new HttpRequestMessage(HttpMethod.Get, imageAPI);
            response = await client.SendAsync(request);
            if (!response.IsSuccessStatusCode)
            {
                // in case of error show error page
                ErrorViewModel errorModel = new ErrorViewModel();
                return View("Error", errorModel);
            }
            body = await response.Content.ReadAsStringAsync();
            responseJSON = null;
            try
            {
                responseJSON = JsonConvert.DeserializeObject(body);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            // parse response to obtain image
            var results = responseJSON.results;
            foreach (var result in results)
            {
                imageLink = result.urls.thumb;
                break;
            }




            weatherResult.ImageLink = imageLink;
            ViewBag.Output = weatherResult;

            // in case of error show error page
            if (error)
            {
                ErrorViewModel errorModel = new ErrorViewModel();
                return View("Error", errorModel);
            } 
                        
            return View("Results", wm);
        }

        
        public IActionResult CreateAccount()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


        // Create User
        public async Task<IActionResult> CreateUser([Bind("ID,FirstName,LastName,Gender,Birthdate,Email,PhoneNumber,Password")] Users users)
        {
            if (ModelState.IsValid) /*copiar controlador botao submit -*/
            {
                _context.Add(users);
                await _context.SaveChangesAsync();

                Weather wm = new Weather();
                wm.period = "Today";
                return View("Main", wm);
                
            }
            return View("CreateAccount");
        }
    }
}
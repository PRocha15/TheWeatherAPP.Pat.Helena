namespace TheWeatherAPP.Pat.Helena.Models
{
    public class Weather
    {
        public string locationName;
        public string period;
        ///public string alerts;


        public Weather() 
        { }

        public string LocationName { get => locationName; set => locationName = value; }
        public string Period { get => period; set => period = value; }

      //  public string Alerts { get => alerts;set => alerts = value; }
    }
}

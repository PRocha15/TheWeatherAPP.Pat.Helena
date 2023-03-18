namespace TheWeatherAPP.Pat.Helena.Models
{
    public class WeatherAlert
    {
        public string Event { get; set; }
        public string Headline { get; set; }
        public string Description { get; set; }
        public string Id { get; set; }

        public WeatherAlert(dynamic alert) { 
            this.Event = alert["event"];
            this.Headline = alert.headline;
            this.Description = alert.description;
            this.Id = alert.id;
        }
    }
}

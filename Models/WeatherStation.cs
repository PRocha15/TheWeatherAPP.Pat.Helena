namespace TheWeatherAPP.Pat.Helena.Models
{
    public class WeatherStation
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public double Distance { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public int UseCount { get; set; }
        public int Quality { get; set; }
        public double Contribution { get; set; }
        public WeatherStation(dynamic station) 
        {
            this.Id = station.id;
            this.Distance= station.distance;
            this.Latitude= station.latitude;
            this.Longitude= station.longitude;
            this.UseCount= station.useCount;
            this.Quality= station.quality;
            this.Contribution= station.contribution;
            this.Name = station.name;

        }
    }
}

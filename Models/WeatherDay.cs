namespace TheWeatherAPP.Pat.Helena.Models
{
    public class WeatherDay
    {
        public string date { get; set; }
        public string conditions { get; set; }
        public double maxTemperature { get; set; }
        public double minTemperature { get; set; }

        public double temperature { get; set; }

        public double feelsLikeMax { get; set; }    
        public double feelsLikeMin { get; set; }

        public double dew { get; set; }

        public Dictionary<string, string> table { 
            get {
                return new Dictionary<string, string>() {
                    { "Temperatura Máxima", this.maxTemperature.ToString()},
                    { "Temperatura Mínima", this.maxTemperature.ToString()},
                    { "Parece Máximo", this.feelsLikeMax.ToString()},
                    { "Parece mínimo", this.feelsLikeMin.ToString()},
                    { "Orvalho", this.dew.ToString() }
                };
            }
        }

        public WeatherDay(dynamic data){ 
        
            this.date = data.date;
            this.conditions = data.conditions;
            this.maxTemperature = data.tempmax;
            this.minTemperature = data.tempmin;
            this.temperature = data.temp;
            this.feelsLikeMin = data.feelslikemin;
            this.feelsLikeMax = data.feelslikemax;
            this.dew= data.dew;
        }
    }
}

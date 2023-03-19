namespace TheWeatherAPP.Pat.Helena.Models
{
    public class WeatherDay
    {
        public string Date { get; set; }
        public string Conditions { get; set; }
        public double MaxTemperature { get; set; }
        public double MinTemperature { get; set; }

        public double Temperature { get; set; }

        public double FeelsLikeMax { get; set; }    
        public double FeelsLikeMin { get; set; }

        public double Dew { get; set; }

        public double Humidity { get; set; }   
        public double Precipitation { get; set; }
        public double PrecipitationProbability { get; set; }
        public double Snow { get; set; }
        public double WindSpeed { get; set; }
        public double WindDireciton { get; set; }
        public double Pressure { get; set; }
        public double CloudCover { get; set; }
        public double Visibility { get; set; }
        public double UVIndex { get; set; }
        public TimeOnly SunRise { get; set; }
        public TimeOnly SunSet { get; set; }

        public double MoonPhase { get; set; }
        public string Description { get; set; }
        public string Icon { get; set; }





        public WeatherDay(dynamic data){ 
        
            
            this.Date = data.datetime;
            this.Conditions = data.conditions;
            this.MaxTemperature = data.tempmax;
            this.MinTemperature = data.tempmin;
            this.Temperature = data.temp;
            this.FeelsLikeMin = data.feelslikemin;
            this.FeelsLikeMax = data.feelslikemax;
            this.Dew= data.dew;
            this.Humidity= data.humidity;
            this.Precipitation = data.precip;
            this.PrecipitationProbability = data.precipprob;
            this.Snow = data.snow != null ? data.snow : 0;
            this.WindSpeed = data.windspeed;
            this.WindDireciton = data.winddir;
            this.Pressure = data.pressure;
            this.CloudCover = data.cloudcover;
            this.Visibility = data.visibility;
            this.UVIndex= data.uvindex != null ? data.uvindex : 0;
            this.SunRise= data.sunrise;
            this.SunSet= data.sunset;
            this.MoonPhase= data.moonphase;
            this.Description= data.description;
            this.Icon= data.icon;

        }


        public Dictionary<string, string> table
        {
            get
            {
                return new Dictionary<string, string>() {
                    { "Feels Like Max", this.FeelsLikeMax.ToString() + "º"},
                    { "Feels Like Min", this.FeelsLikeMin.ToString() + "º"},
                    { "Dew", this.Dew.ToString() + "%" },
                    { "Humidity", this.Humidity.ToString() + "%" },
                    { "Precipitation", this.Precipitation.ToString() },
                    { "Precipitation Probability", this.PrecipitationProbability.ToString() + "%" },
                    { "Snow", this.Snow.ToString() },
                    { "Wind Speed", this.WindSpeed.ToString() },
                    { "Wind Direction", this.WindDireciton.ToString() + "º" },
                    { "Pressure", this.Pressure.ToString() },
                    { "Cloud Cover", this.CloudCover.ToString() + "%" },
                    { "Visibility", this.Visibility.ToString() },
                    { "UV Index", this.UVIndex.ToString() },
                    { "Sun Rise", this.SunRise.ToString() },
                    { "Sun Set", this.SunSet.ToString() },
                    { "Moon Phase", this.MoonPhaseText }
                };
            }
        }

        public string MoonPhaseText
        {
            get
            {
                string response = "New Moon";
                if(MoonPhase>0 && MoonPhase < 0.25)
                {
                    response = "Waxing Crescent";
                } 
                else if (MoonPhase == 0.25)
                {
                    response = "First Quarter";
                } 
                else if (MoonPhase >0.25 && MoonPhase < 0.5)
                {
                    response = "Waxing Gibbous";
                }
                else if (MoonPhase == 0.5)
                {
                    response = "Full Moon";
                }
                else if (MoonPhase > 0.5 && MoonPhase < 0.75)
                {
                    response = "Waning Gibbous";
                }
                else if (MoonPhase == 0.75)
                {
                    response = "Last Quarter";
                }
                else if (MoonPhase > 0.75 && MoonPhase < 1)
                {
                    response = "Wanning Crescent";
                }
                return response;
            }
        }
        public string IconLink
        {
            get
            {
                return "/img/WeatherIcons/" + Icon + ".svg";
            }
        }
    }
}

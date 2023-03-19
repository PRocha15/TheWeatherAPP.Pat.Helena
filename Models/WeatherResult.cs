﻿using Microsoft.AspNetCore.Http;

namespace TheWeatherAPP.Pat.Helena.Models
{
    

    public class WeatherResult
    {
        public List<WeatherAlert> Alerts { get; set; }
        public List<WeatherStation> Stations { get; set; }
        public List<WeatherDay> Days { get; set; }

        public string ImageLink { get; set; }

        public WeatherResult(dynamic response) {

            this.Alerts = new List<WeatherAlert>();
            this.Days = new List<WeatherDay>();
            this.Stations = new List<WeatherStation>();

            //alerts
            if (response.alerts != null) { 
                foreach (var alert in response.alerts)
                {
                    var alertItem = new WeatherAlert(alert);
                    this.Alerts.Add(alertItem);
                }
            }

            // weather per day
            foreach (var day in response.days)
            {
                var weatherDay = new WeatherDay(day);
                this.Days.Add(weatherDay);

            }


            // stations
            if(response.currentConditions != null && response.currentConditions.stations != null){
                foreach (string stationId in response.currentConditions.stations)
                {
                    var station = new WeatherStation(response.stations[stationId]);
                    this.Stations.Add(station);
                }

            }



        }
    }
}

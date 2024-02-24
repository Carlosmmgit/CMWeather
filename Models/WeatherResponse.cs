
using System.Collections.Generic;

namespace CMWeather.Models
{
    class WeatherResponse
    {
        public string name { get; set; }
        public List<Weather> weather { get; set; }
        public Main main { get; set; }
    }
    class Weather
    {
        public string main { get; set; }
        public string description { get; set; }

    }
    class Main
    {
        public float temp { get; set; }
        public float humidity { get; set; }
    }
}


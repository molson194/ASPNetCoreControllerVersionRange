using System;

namespace TestApiVersioning2
{
    public class WeatherForecast2
    {
        public DateTime Date { get; set; }

        public int TemperatureC { get; set; }

        public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

        public string Summary { get; set; }

        public string MattTest => "Matt rules!";
    }
}

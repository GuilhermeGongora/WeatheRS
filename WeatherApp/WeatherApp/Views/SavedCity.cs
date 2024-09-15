using System;
using System.Collections.Generic;
using System.Text;

namespace WeatherApp.Views
{
    // SavedCity.cs
    public class SavedCity
    {
        public string Name { get; set; }
        public double Temperature { get; set; }
        public int Humidity { get; set; }
        public double Pressure { get; set; }
        public double WindSpeed { get; set; }
        public int Cloudiness { get; set; }
        public string Icon { get; set; }
        public DateTime Date { get; set; }
    }

}

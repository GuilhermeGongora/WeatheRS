using SQLite;
using System;

namespace WeatherApp.Models
{
    public class SavedCity
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Name { get; set; }
        public double Temperature { get; set; }
        public int Humidity { get; set; }
        public int Pressure { get; set; }
        public double WindSpeed { get; set; }
        public int Cloudiness { get; set; }
        public string Icon { get; set; }
        public DateTime Date { get; set; }
    }
}

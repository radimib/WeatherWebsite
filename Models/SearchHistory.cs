using System;

namespace CsharpWeather.Models
{
    public class SearchHistory
    {
        public int Id { get; set; }
        public string City { get; set; }
        public DateTime SearchTime { get; set; } = DateTime.Now;
    }
}

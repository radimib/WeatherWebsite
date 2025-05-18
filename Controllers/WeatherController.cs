using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using CsharpWeather.Models;
using CsharpWeather.Data;


namespace CsharpWeather.Controllers
{
    public class WeatherController : Controller
    {
        private readonly string apiKey = "93f6668259933dac48fb121ab06ea8ed"; 

        // This constructor initializes the context
        private readonly AppDbContext _context;

        public WeatherController(AppDbContext context)
        {
            _context = context;
        }

        // The Index action returns the view and performs necessary logic
       [HttpGet]
public IActionResult Index()
{
    ViewBag.History = _context.SearchHistories
        .OrderByDescending(s => s.SearchTime)
        .Take(5)
        .ToList();

    return View();
}


        // This is the POST action to handle the city search and fetch weather data
        [HttpPost]
public async Task<IActionResult> Index(string city)
{
    var weather = new WeatherModel();

    using var client = new HttpClient();
    var url = $"https://api.openweathermap.org/data/2.5/weather?q={city}&appid={apiKey}&units=metric";
    var response = await client.GetAsync(url);

    if (response.IsSuccessStatusCode)
    {
        var json = await response.Content.ReadAsStringAsync();
        using var doc = JsonDocument.Parse(json);
        var root = doc.RootElement;

        weather.City = city;
        weather.Description = root.GetProperty("weather")[0].GetProperty("description").GetString();
        weather.Temperature = root.GetProperty("main").GetProperty("temp").GetSingle();
        weather.Humidity = root.GetProperty("main").GetProperty("humidity").GetInt32();

        // ✅ Save the actual searched city to the DB
        _context.SearchHistories.Add(new SearchHistory { City = city });
        await _context.SaveChangesAsync();
    }
    else
    {
        ModelState.AddModelError("", "Weather data could not be fetched.");
    }

    // ✅ Reload the recent searches
    ViewBag.History = _context.SearchHistories
        .OrderByDescending(s => s.SearchTime)
        .Take(5)
        .ToList();

    return View(weather);
}

    }
}

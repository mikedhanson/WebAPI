using Microsoft.AspNetCore.Mvc;
using NuGet.Configuration;
using NuGet.Protocol;
using RestSharp;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger) => _logger = logger;

        [HttpGet(Name = "GetWeatherForecast")]
        public async Task<string?> GetWeatherAsync()
        {
            var configBuilder = new ConfigurationBuilder().AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            var configuration = configBuilder.Build();
            var _apiKey = configuration.GetValue<string>("WeatherAPIKey");

            var CityID = configuration.GetValue<int>("CityID");
           
            var options = new RestClientOptions("https://api.openweathermap.org")
            {
                MaxTimeout = -1,
            };

            var client = new RestClient(options);
            var request = new RestRequest($"/data/2.5/weather?id={CityID}&appid={_apiKey}", Method.Get);
            RestResponse response = await client.ExecuteAsync(request);

            return response.Content;
        }
    }
}
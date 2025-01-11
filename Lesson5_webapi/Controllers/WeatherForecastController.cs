using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net;
using System;

namespace Lesson5_webapi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        // ���� Person ��
        public class Person
        {
            public string Name { get; set; }
            public int Age { get; set; }
            public Address Address { get; set; }
        }

        // ���� Address ��
        public class Address
        {
            public string City { get; set; }
            public string ZipCode { get; set; }
        }
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get()
        {
            var person = new Person
            {
                Name = "Alice",
                Age = 25,
                Address = new Address { City = "New York", ZipCode = "10001" }
            };

            // ���������л�Ϊ JSON �ַ���
            string jsonString = JsonConvert.SerializeObject(person, Formatting.Indented);
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }
    }
}

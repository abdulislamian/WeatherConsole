using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherConsole.Repository
{
    public class WeatherStatus : IWeatherStatus
    {
        public async Task<Weather> GetWeatherStatus(string CityName, IConfiguration configuration)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri($"{configuration["WeatherApi:BaseUrl"]}/weather?q={CityName}&appid={configuration["WeatherApi:ApiKey"]}&units=imperial");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            var result =  await GetWeather(client);
            return result;
        }

        private static string ConvertToCelsius(double temp)
        {
            return ((temp - 32) * 0.55).ToString("0.00");
        }

        static async Task<Weather> GetWeather(HttpClient cons)
        {
            using (cons)
            {
                try
                {
                    HttpResponseMessage res = await cons.GetAsync("");
                    var WeatherReport = new Weather();
                    res.EnsureSuccessStatusCode();
                    if (res.IsSuccessStatusCode)
                    {
                        string weather = await res.Content.ReadAsStringAsync();
                        var jobj = JObject.Parse(weather);
                        double WeatherState = (double)jobj["main"]["temp"];
                        string City = jobj["name"].ToString();
                        string country = jobj["sys"]["country"].ToString();
                        var currentDate = DateTime.Now.ToString("yyyy-MM-dd");

                        WeatherReport.temp = Convert.ToDouble(ConvertToCelsius(WeatherState));
                        WeatherReport.text = City;
                        WeatherReport.date = currentDate;
                        WeatherReport.country = country;
                        return WeatherReport;
                    }
                    return WeatherReport;
                }catch(Exception ex)
                {
                    return null;
                }
            }
        }

        public void PrintWeatherInfo(Weather weather)
        {
            Console.WriteLine("\n");
            Console.WriteLine("Weather Station: Open Weather API");
            Console.WriteLine("Temperature Details");
            Console.WriteLine("-----------------------------------------------------------");
            Console.WriteLine("Temperature (in deg. C): " + weather.temp);
            Console.WriteLine("Weather City: " + weather.text + "," + weather.country);
            Console.WriteLine("Applicable Time: " + weather.date);
        }
    }
}

using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherConsole.Repository
{
    public interface IWeatherStatus
    {
         Task<Weather> GetWeatherStatus(string CityName, IConfiguration configuration);
         void PrintWeatherInfo(Weather resultweather);
    }
}

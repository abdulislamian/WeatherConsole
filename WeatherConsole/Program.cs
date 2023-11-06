using Microsoft.Extensions.Configuration;
using WeatherConsole;
using WeatherConsole.Repository;

namespace Application
{
    class Program
    {
        static void Main(string[] args)
        {
            IConfiguration configuration = new ConfigurationBuilder()
           .AddJsonFile("appsettings.json")
           .Build();
            IWeatherStatus weather = new WeatherStatus();

            bool continueSearching = true;

            while (continueSearching)
            {
                Dictionary<string, List<string>> provinceCities = new Dictionary<string, List<string>>
            {
                { "Punjab", new List<string> { "Lahore,PK", "Faisalabad,PK", "Rawalpindi,PK", "Multan,PK" } },
                { "Sindh", new List<string> { "Karachi,PK", "Hyderabad,PK", "Sukkur,PK" } },
                { "Khyber Pakhtunkhwa", new List<string> { "Peshawar,PK", "Abbottabad,PK", "Swat,PK" } },
                { "Balochistan", new List<string> { "Quetta,PK", "Gwadar,PK", "Khuzdar,PK" } },
                { "Gilgit-Baltistan", new List<string> { "Gilgit,PK", "Skardu,PK" } },
                { "Azad Kashmir", new List<string> { "Muzaffarabad,PK", "Mirpur,PK" } }
            };

                Console.WriteLine("\n");
                Console.WriteLine("Select an option:");
                Console.WriteLine("1. Select a city by province");
                Console.WriteLine("2. Search for a custom city by name input");
                Console.Write("Enter the option number: ");

                if (int.TryParse(Console.ReadLine(), out int option) && (option == 1 || option == 2))
                {
                    if (option == 1)
                    {
                        Console.WriteLine("\nSelect a province by number from the following list:");
                        List<string> provinces = new List<string>(provinceCities.Keys);

                        for (int i = 0; i < provinces.Count; i++)
                        {
                            Console.WriteLine($"{i + 1}. {provinces[i]}");
                        }

                        Console.Write("\nEnter the number of the province: ");
                        if (int.TryParse(Console.ReadLine(), out int provinceNumber) && provinceNumber >= 1 && provinceNumber <= provinces.Count)
                        {
                            string selectedProvince = provinces[provinceNumber - 1];
                            Console.WriteLine($"You selected {selectedProvince}");

                            if (provinceCities.ContainsKey(selectedProvince))
                            {
                                List<string> cities = provinceCities[selectedProvince];
                                Console.WriteLine($"Major cities in {selectedProvince}:");

                                for (int i = 0; i < cities.Count; i++)
                                {
                                    Console.WriteLine($"{i + 1}. {cities[i]}");
                                }

                                Console.Write("Enter the number of the city: ");
                                if (int.TryParse(Console.ReadLine(), out int cityNumber) && cityNumber >= 1 && cityNumber <= cities.Count)
                                {
                                    string selectedCity = cities[cityNumber - 1];
                                    Console.WriteLine($"You selected {selectedCity}");
                                    var resultweather = weather.GetWeatherStatus(selectedCity, configuration).Result;
                                    if (resultweather != null)
                                    {
                                        weather.PrintWeatherInfo(resultweather);
                                    }
                                    else
                                    {
                                        Console.WriteLine("No Weather Info Available, or Invalid City Name");
                                    }
                                }
                                else
                                {
                                    Console.WriteLine("Invalid city selection. Please choose a valid number.");
                                }
                            }
                            else
                            {
                                Console.WriteLine("Invalid province selection. Please choose a valid province.");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Invalid province selection. Please choose a valid number.");
                        }
                    }
                    else if (option == 2)
                    {
                        Console.Write("Enter the name of the custom city: ");
                        string City = Console.ReadLine();
                        Console.Write("Enter State Code (eg:PK): ");
                        string countryCode = Console.ReadLine();
                        var CityInfo = $"{City},{countryCode}";
                        Console.WriteLine($"You entered the custom city: {CityInfo} ");
                        var resultweather = weather.GetWeatherStatus(CityInfo, configuration).Result;
                        if (resultweather != null)
                        {
                            weather.PrintWeatherInfo(resultweather);
                        }
                        else
                        {
                            Console.WriteLine("No Weather Info Available, or Invalid City Name");
                        }
                    }
                }
                else
                {
                    Console.WriteLine("Invalid option. Please choose a valid option.");
                }

                Console.Write("\nDo you want to search again? (Y/N): ");
                string response = Console.ReadLine();

                if (response.Trim().Equals("N", StringComparison.OrdinalIgnoreCase))
                {
                    continueSearching = false;
                }
            }
        }
    }
}
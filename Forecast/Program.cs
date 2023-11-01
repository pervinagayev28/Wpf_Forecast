using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace Forecast
{
    class Program
    {
        private const string ApiKey = "0603a06157949b5bba4c921b7eb79f3f";

        static async Task Main(string[] args)
        {
            var location = "Azerbaijan,Qobustan"; // Hava durumu bilgisini almak istediğiniz lokasyonu buraya girin

            var weatherData = await GetWeatherDataAsync(location);

            if (weatherData != null)
            {
                Console.WriteLine($"Location: {weatherData.name}");
                Console.WriteLine($"Temperature: {weatherData.main.temp}°C");
                Console.WriteLine($"Feels Like: {weatherData.main.feels_like}°C");
                Console.WriteLine($"Minimum Temperature: {weatherData.main.temp_min}°C");
                Console.WriteLine($"Maximum Temperature: {weatherData.main.temp_max}°C");
                Console.WriteLine($"Pressure: {weatherData.main.pressure}");
                Console.WriteLine($"Humidity: {weatherData.main.humidity}%");
                Console.WriteLine($"Visibility: {weatherData.visibility}");
                Console.WriteLine($"Wind Speed: {weatherData.wind.speed}");
                Console.WriteLine($"Wind Degree: {weatherData.wind.deg}");
                Console.WriteLine($"Cloudiness: {weatherData.clouds.all}");
                Console.WriteLine($"Date Time: {UnixTimeStampToDateTime(weatherData.dt)}");
                Console.WriteLine($"Country: {weatherData.sys.country}");
                Console.WriteLine($"Sunrise: {UnixTimeStampToDateTime(weatherData.sys.sunrise)}");
                Console.WriteLine($"Sunset: {UnixTimeStampToDateTime(weatherData.sys.sunset)}");
            }
            else
            {
                Console.WriteLine("Hava durumu bilgisi alınamadı.");
            }
        }

        static async Task<WeatherData> GetWeatherDataAsync(string location)
        {
            using (var client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync($"http://api.openweathermap.org/data/2.5/weather?q={location}&appid={ApiKey}&units=metric");
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                WeatherData weatherData = JsonSerializer.Deserialize<WeatherData>(responseBody);
                return weatherData;
            }
        }

        static DateTime UnixTimeStampToDateTime(long unixTimeStamp)
        {
            DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dtDateTime;
        }
    }

    public class Coord
    {
        public float lon { get; set; }
        public float lat { get; set; }
    }

    public class Weather
    {
        public int id { get; set; }
        public string main { get; set; }
        public string description { get; set; }
        public string icon { get; set; }
    }

    public class Main
    {
        public float temp { get; set; }
        public float feels_like { get; set; }
        public float temp_min { get; set; }
        public float temp_max { get; set; }
        public int pressure { get; set; }
        public int humidity { get; set; }
    }

    public class Wind
    {
        public float speed { get; set; }
        public int deg { get; set; }
    }

    public class Clouds
    {
        public int all { get; set; }
    }

    public class Sys
    {
        public int type { get; set; }
        public int id { get; set; }
        public string country { get; set; }
        public long sunrise { get; set; }
        public long sunset { get; set; }
    }

    public class WeatherData
    {
        public Coord coord { get; set; }
        public Weather[] weather { get; set; }
        public string @base { get; set; }
        public Main main { get; set; }
        public int visibility { get; set; }
        public Wind wind { get; set; }
        public Clouds clouds { get; set; }
        public long dt { get; set; }
        public Sys sys { get; set; }
        public int timezone { get; set; }
        public int id { get; set; }
        public string name { get; set; }
        public int cod { get; set; }
    }
}

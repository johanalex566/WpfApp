//https://developer.accuweather.com/apis
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using WpfApp.Model;

namespace WpfApp.ViewModel.Helpers
{
    public class WeatherHelper
    {
        public const string tokenAPI = "lO8gLTfks65nDSjCGMNir2tuEV3zwQKp&q";
        public const string BASE_URL = "http://dataservice.accuweather.com/";
        public const string AUTOCOMPLETE_ENDPOINT = "locations/v1/cities/autocomplete?apikey={0}={1}";
        public const string WEATHER_CONDITION_ENDPOINT = "currentconditions/v1/{0}?apikey={1}";
        public string insightInstrumentationKey = ConfigurationManager.AppSettings["insightInstrumentationKey"];

        static TelemetryClient telemetry = new TelemetryClient();
        TelemetryConfiguration configuration = TelemetryConfiguration.Active;
        public WeatherHelper() => telemetry.InstrumentationKey = insightInstrumentationKey;

        public static async Task<List<City>> GetCities(string query)
        {

            List<City> cities = new List<City>();
            string url = BASE_URL + string.Format(AUTOCOMPLETE_ENDPOINT, tokenAPI, query);

            try
            {
                using (var operation = telemetry.StartOperation<RequestTelemetry>("operationName"))
                {
                    using (HttpClient client = new HttpClient())
                    {
                        Stopwatch stopWatch = new Stopwatch();

                        stopWatch.Start();

                        var response = await client.GetAsync(url);

                        string json = await response.Content.ReadAsStringAsync();

                        cities = JsonConvert.DeserializeObject<List<City>>(json);
                    }
                }
            }
            catch (Exception ex)
            {
                telemetry.TrackTrace($"Error{ex.Message}", SeverityLevel.Information);
                telemetry.TrackException(ex);
                throw new Exception(ex.Message);
            }
            return cities;
        }

        public static async Task<Weather> GetWeather(string cityKey)
        {
            Weather weather = new Weather();
            string url = BASE_URL + string.Format(WEATHER_CONDITION_ENDPOINT, cityKey, tokenAPI);
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    var response = await client.GetAsync(url);

                    string json = await response.Content.ReadAsStringAsync();

                    weather = (JsonConvert.DeserializeObject<List<Weather>>(json)).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return weather;
        }
    }
}

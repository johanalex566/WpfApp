using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp.Model;
using WpfApp.ViewModel.Commands;
using WpfApp.ViewModel.Helpers;

namespace WpfApp.ViewModel
{
    public class WeatherVM : INotifyPropertyChanged
    {
        #region fullproperties
        private string query;
        private Weather weather;
        private City selectedCity;
        public SearchCommand SearchCommand { get; set; }
        public ObservableCollection<City> Cities { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;
        public WeatherVM()
        {

            SelectedCity = new City { LocalizedName = "London" };
            Weather = new Weather
            {
                WeatherText = "Partly cloudy",
                Temperature = new Temperature
                {
                    Metric = new Metric
                    {
                        Value = 21
                    }
                }
            };
            SearchCommand = new SearchCommand(this);
            Cities = new ObservableCollection<City>();
        }
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public string Query
        {
            get { return query; }
            set
            {
                query = value;
                OnPropertyChanged("Query");
            }
        }
        public Weather Weather
        {
            get { return weather; }
            set
            {
                weather = value;
                OnPropertyChanged("Weather");
            }
        }
        public City SelectedCity
        {
            get { return selectedCity; }
            set
            {
                selectedCity = value;
                OnPropertyChanged("SelectedCity");
                if (selectedCity != null)
                {
                    if (selectedCity.Key != null)
                    {
                        GetWeatherConditions();
                    }
                }
            }
        }
        #endregion full properties


        #region Methods
        public async void MakeQuery()
        {
            List<City> cities = await WeatherHelper.GetCities(Query);
            Cities.Clear();
            foreach (var city in cities)
            {
                Cities.Add(city);
            }
        }
        private async void GetWeatherConditions()
        {
            Query = string.Empty;
            Weather = await WeatherHelper.GetWeather(SelectedCity.Key);
        }
        #endregion
    }
}

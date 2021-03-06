﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Notifications;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Newtonsoft.Json;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Weather
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        Dictionary<string, int> map = new Dictionary<string, int>()
            {
                {"London",6058560},
                {"Hamilton", 5969785},
                {"Waterloo", 6176821},
            };

        private int cityId = 0;

        public MainPage()
        {
            this.InitializeComponent();

        }


        /// <summary>
        /// Gets information from online API 
        /// </summary>
        /// <param name="cityId"></param>
        /// <returns></returns>
        async Task<string[]> RunAsync(int cityId)
        {
            //0.cityName; 1.temp_min; 2.temp_max; 3.windSpeed; 4.windDegree; 5. weatherConditionMain; 6.iconId;
            string[] weatherInfo = new string[8];

            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response;

                response = await
                    client.GetAsync(
                        "http://api.openweathermap.org/data/2.5/weather?id=" + cityId.ToString() + "&appid=23dde959b0c9a19b586ed9af3bbc8868&units=metric").ConfigureAwait(continueOnCapturedContext: false);

                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    dynamic content1 = JsonConvert.DeserializeObject(content);

                    weatherInfo[0] = content1.name;//city name
                    weatherInfo[1] = content1.main.temp_min;
                    weatherInfo[2] = content1.main.temp_max;
                    weatherInfo[3] = content1.wind.speed;
                    weatherInfo[4] = content1.wind.deg;
                    weatherInfo[5] = content1.weather[0].main;//weather condition
                    weatherInfo[6] = content1.weather[0].icon;//weather icon
                    weatherInfo[7] = content1.main.humidity;
                }
            }
            return weatherInfo;
        }

        async Task<string[]> FutureWeather(int cityId)
        {
            //0.cityName; 1.temp_min; 2.temp_max; 3.windSpeed; 4.windDegree; 5. weatherConditionMain; 6.iconId;
            string[] weatherInfo = new string[16];

            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response;

                response = await
                    client.GetAsync(
                        "http://api.openweathermap.org/data/2.5/forecast?id=" + cityId.ToString() + "&cnt=4&appid=23dde959b0c9a19b586ed9af3bbc8868&units=metric").ConfigureAwait(continueOnCapturedContext: false);

                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    dynamic content1 = JsonConvert.DeserializeObject(content);

                    weatherInfo[0] = content1.list[0].dt_txt;
                    weatherInfo[1] = content1.list[0].weather[0].main;
                    weatherInfo[2] = content1.list[0].main.temp;
                    weatherInfo[3] = content1.list[0].main.humidity;
                    weatherInfo[4] = content1.list[1].dt_txt;
                    weatherInfo[5] = content1.list[1].weather[0].main;
                    weatherInfo[6] = content1.list[1].main.temp;
                    weatherInfo[7] = content1.list[1].main.humidity;
                    weatherInfo[8] = content1.list[2].dt_txt;
                    weatherInfo[9] = content1.list[2].weather[0].main;
                    weatherInfo[10] = content1.list[2].main.temp;
                    weatherInfo[11] = content1.list[2].main.humidity;
                    weatherInfo[12] = content1.list[3].dt_txt;
                    weatherInfo[13] = content1.list[3].weather[0].main;
                    weatherInfo[14] = content1.list[3].main.temp;
                    weatherInfo[15] = content1.list[3].main.humidity;
                }
            }
            return weatherInfo;
        }


        private async void button_Click(object sender, RoutedEventArgs e)
        {
            ComboBoxItem typeItem = (ComboBoxItem)comboBox.SelectedItem;
            string value = typeItem.Content.ToString();
            cityId = CityID(value);
            string[] weatherInfo = await RunAsync(cityId);
            string[] futureInfo = await FutureWeather(cityId);
            cityNameTB.Text = weatherInfo[0];
            //
            today.Text = "Minimum Temperature: " + weatherInfo[1] + "\n"
                + "Max Temperature: " + weatherInfo[2] + "\n"
                + "Wind speed: " + weatherInfo[3] + "\n"
                + "Wind degree: " + weatherInfo[4] + "\n"
                + "Weather condition: " + weatherInfo[5] + "\n"
                + "Weather icon: " + weatherInfo[6] + "\n"
                + "Weather humidity" + weatherInfo[7] + "\n"
                ;
            secondDay.Text = "Date" + futureInfo[0] + "\n"
                + "Weather condition: " + futureInfo[1] + "\n"
                + "Temperature: " + futureInfo[2] + "\n"
                + "Humidity: " + futureInfo[3] + "\n"
                ;
            thirdDay.Text = "Date" + futureInfo[0] + "\n"
                + "Weather condition: " + futureInfo[1] + "\n"
                + "Temperature: " + futureInfo[2] + "\n"
                + "Humidity: " + futureInfo[3] + "\n"
                ;
            fourthDay.Text = "Date" + futureInfo[0] + "\n"
                + "Weather condition: " + futureInfo[1] + "\n"
                + "Temperature: " + futureInfo[2] + "\n"
                + "Humidity: " + futureInfo[3] + "\n"
                ;
            fifthDay.Text = "Date" + futureInfo[0] + "\n"
                + "Weather condition: " + futureInfo[1] + "\n"
                + "Temperature: " + futureInfo[2] + "\n"
                + "Humidity: " + futureInfo[3] + "\n"
                ;
            //string temperature = 
        }

        private int CityID(string cityName)
        {

            int output = map[cityName];
            return output;
        }
    }
}

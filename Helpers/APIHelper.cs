using CMWeather.Models;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;

namespace CMWeather.Helpers
{
    class APIHelper
    {
        private const string PublicKey = "6af770018fd96fdcc702ab0b3b928642";
        public async Task<WeatherResponse> Call_API()
        {
            #region URL
            string baseUrl = "https://api.openweathermap.org/data/2.5/";
            float latMadrid = 40.42f;
            float lonMadrid = -3.70f;
            string apiUrl = $"{baseUrl}weather?lat={latMadrid}&lon={lonMadrid}&appid={PublicKey}";
            #endregion
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    HttpResponseMessage response = await client.GetAsync(apiUrl);

                    if (response.IsSuccessStatusCode)
                    {
                        string responseData = await response.Content.ReadAsStringAsync();
                        WeatherResponse resp = JsonConvert.DeserializeObject<WeatherResponse>(responseData);
                        return resp;
                    }
                    else
                        MessageBox.Show("La solicitud a la API falló: " + response.ReasonPhrase);
                    return null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al conectar con la API: " + ex.Message);
                return null;
            }
        }
    }
}

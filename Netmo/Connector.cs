using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;


namespace Netmo
{
    public class Connector
    {

        public static Token CurrentToken;

        public Connector(string _clientid, string _clientsecret, string _username, string _password)
        {
            var nvc = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("client_id", _clientid),
                new KeyValuePair<string, string>("client_secret", _clientsecret),
                new KeyValuePair<string, string>("grant_type", "password"),
                new KeyValuePair<string, string>("username", _username),
                new KeyValuePair<string, string>("password", _password)
            };

            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = client.PostAsync("https://api.netatmo.com/oauth2/token", new FormUrlEncodedContent(nvc)).Result;
                if (!response.IsSuccessStatusCode) {
                    CurrentToken = null;
                    throw new Exception ("Ein Fehler ist aufgetreten, der Token konnte nicht abgerufen werden." + response.StatusCode);
                }
                var strtoken = response.Content.ReadAsStringAsync().Result;
                CurrentToken = Newtonsoft.Json.JsonConvert.DeserializeObject<Token>(strtoken);
            }
        }

        public async Task<NetAtmoResponse> GetNetatmoWeatherDataAsync()
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var kvp = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("access_token", CurrentToken.access_token),
                    new KeyValuePair<string, string>("device_id", "70:ee:50:36:f0:2a")
                };
                var response2 = await client.PostAsync("https://api.netatmo.com/api/getstationsdata", new FormUrlEncodedContent(kvp));
                var datastr = response2.Content.ReadAsStringAsync().Result;
                return Newtonsoft.Json.JsonConvert.DeserializeObject<NetAtmoResponse>(datastr);
            }
        }

        public NetAtmoResponse GetNetatmoWeatherData()
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var kvp = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("access_token", CurrentToken.access_token),
                    new KeyValuePair<string, string>("device_id", "70:ee:50:36:f0:2a")
                };
                var response2 = client.PostAsync("https://api.netatmo.com/api/getstationsdata", new FormUrlEncodedContent(kvp)).Result;
                var datastr = response2.Content.ReadAsStringAsync().Result;
                return Newtonsoft.Json.JsonConvert.DeserializeObject<NetAtmoResponse>(datastr);
            }
        }
    }
}

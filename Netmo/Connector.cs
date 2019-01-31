using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using System.IO;
using System.Windows;

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
                WriteFile(response2); // TODO: Remove
                return Newtonsoft.Json.JsonConvert.DeserializeObject<NetAtmoResponse>(datastr);
            }
        }

        // TODO: Remove
        private async void WriteFile(HttpResponseMessage response2)
        {
            if (response2 == null || !response2.IsSuccessStatusCode)
            {
                return;
            }

            string executingpath = System.Reflection.Assembly.GetExecutingAssembly().Location;
            executingpath = RemoveEnding(executingpath);

            if (File.Exists(executingpath + @"\temp.json"))
            {
                File.Delete(executingpath + @"\temp.json");
            }
            FileStream fs = File.Create(executingpath + @"\temp.json");
            fs.Close();
            File.WriteAllBytes(executingpath + @"\temp.json", await response2.Content.ReadAsByteArrayAsync());

            Console.WriteLine(executingpath + @"\temp.json");
        }

        // TODO: Remove
        private string RemoveEnding(string executingpath)
        {
            List<string> pathTiles = executingpath.Split(new char[] { Char.Parse(@"\") }).ToList();
            pathTiles.Remove(pathTiles[pathTiles.Count - 1]);
            return String.Join(@"\", pathTiles);
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

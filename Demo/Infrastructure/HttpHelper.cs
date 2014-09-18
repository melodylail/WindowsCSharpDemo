using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json.Serialization;

namespace Demo.Infrastructure
{
    public class HttpHelper
    {
        public static async Task<String> GetAsync(HttpClient client, String requestURL)
        {
            var responseBody = String.Empty;

            try
            {
                using (client = new HttpClient())
                { 
                    HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Get, requestURL);
                    requestMessage.Headers.Add("Authorization", string.Format("Espresso {0}", ApiHelper.ApiKey + ":1"));

                    var getResponse =  client.SendAsync(requestMessage).Result;
                  
                    getResponse.EnsureSuccessStatusCode();
                    responseBody = await getResponse.Content.ReadAsStringAsync();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }

            return responseBody;
        }
        public static async Task<String> Authenticate(String user, String pass)
        {
            string apiKey = "";

            using (var client = new HttpClient())
            {
                var authInfo = new { username = user, password = pass };

                HttpContent content = new StringContent(JsonConvert.SerializeObject(authInfo), Encoding.UTF8, "application/json");

                try
                {

                    using (HttpResponseMessage response = client.PostAsync(BaseURLHelper.BaseURL +"/@authentication", content).Result)
                    {
                        response.EnsureSuccessStatusCode();
                        var responseBody = await response.Content.ReadAsStringAsync();

                        dynamic authoResponse = JObject.Parse(responseBody);
                        apiKey = authoResponse.apikey;
                    }
                }

                catch (HttpRequestException)
                {
                    Debug.WriteLine("Login attempt failed.");
                    apiKey = "";
                }
            }
            return apiKey;
        }
    }
}

using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ReversiMVCApplication.Controllers;
using ReversiMVCApplication.Models;

namespace ReversiMVCApplication.Services
{
    public class ApiService
    {
        private readonly HttpClient _httpClient;

        public ApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // taking all availabke games which where the gamestate = wachten, to be join to 
        // haal alle spellen uit Rest 
        public async Task<List<Spel>> GetSpelInfo()
        {
            var response = await _httpClient.GetAsync($"api/Spel");
            if (!response.IsSuccessStatusCode) throw new Exception($"Status code was {response.StatusCode}");

            var gameInfo = await response.Content.ReadAsStringAsync();

            var spelResponses = JsonConvert.DeserializeObject<List<Spel>>(gameInfo);

            // ! checking later how can i use mapping and for what it is 11.04 and i am tired
            // var spellenLijst = new List<Spel>();
            // foreach (var spel in spelResponses)
            // {
            //     var mappedSpel = MapSpelResponseToSpel(spel);
            //     spellenLijst.Add(mappedSpel);
            // }

            return spelResponses;
        }


        public async Task<int> GetBeurt(string token)
        {
            var response = await _httpClient.GetAsync($"api/Spel/Beurt/{token}");
            if (!response.IsSuccessStatusCode) throw new Exception($"Status code was {response.StatusCode}");

            var gameInfo = await response.Content.ReadAsStringAsync();

            var getBeurtResponse = JsonConvert.DeserializeObject<int>(gameInfo);

            return getBeurtResponse;
        }

        public async Task<Spel> CreateSpel(SpelInfoApi spelInfo)
        {
            var json = JsonConvert.SerializeObject(spelInfo);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("api/Spel", data);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Status code was {response.StatusCode} ");
            }

            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Spel>(result);
        }


        public async Task<Spel> JoinGame(string token, string Speler2Token)
        {
            var json = JsonConvert.SerializeObject(Speler2Token);
            Console.WriteLine(json);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync($"api/Spel/join/{token}", data);

            Console.WriteLine(response);
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Status code was {response.StatusCode}");
            }

            var result = await response.Content.ReadAsStringAsync();
            Console.WriteLine(result);
            return JsonConvert.DeserializeObject<Spel>(result);
        }


        public async Task<Spel> GetSpelByToken(string token)
        {
            if (token == null)
            {
                Console.WriteLine($"spelToken is null: {token}");
            }

            //GetSpelByToken/{token}
            var response = await _httpClient.GetAsync($"api/Spel/GetSpelByToken/{token}");
            if (!response.IsSuccessStatusCode)
                throw new Exception(
                    $"Status code was {response.StatusCode} and status message is {response.RequestMessage}");

            var gameInfo = await response.Content.ReadAsStringAsync();

            Console.WriteLine(gameInfo);
            return JsonConvert.DeserializeObject<Spel>(gameInfo);
        }

        public async Task<Spel> GetSpelById(int id)
        {
            //GetSpelByToken/{token}
            var response = await _httpClient.GetAsync($"api/Spel/GetSpelById/{id}");
            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }

            if (!response.IsSuccessStatusCode)
                throw new Exception(
                    $"Status code was {response.StatusCode} and status message is {response.RequestMessage}");

            var gameInfo = await response.Content.ReadAsStringAsync();

            Console.WriteLine(gameInfo);
            return JsonConvert.DeserializeObject<Spel>(gameInfo);
        }

        // [HttpGet("spelerToken/{speler1Token}")]
        public async Task<Spel> GetSpelFromSpeler1Token(string speler1Token)
        {
            //GetSpelFromSpelerToken/{spelerToken}
            if (speler1Token == null)
            {
                Console.WriteLine($"speler is null: {speler1Token}");
            }

            //! cuz every player must be associated with just one game and cannot make another just after finishing the one b4 and delete it 
            var response = await _httpClient.GetAsync($"api/Spel/spelerToken/{speler1Token}");
            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }

            if (!response.IsSuccessStatusCode)
                throw new Exception(
                    $"Status code was {response.StatusCode} and the statusMessage is {response.RequestMessage}");

            var gameInfo = await response.Content.ReadAsStringAsync();

            Console.WriteLine(gameInfo);
            return JsonConvert.DeserializeObject<Spel>(gameInfo);
        }


        
        // [HttpGet("spelerToken/{speler2Token}")]
        public async Task<Spel> GetSpelFromSpeler2Token(string speler2Token)
        {
            //GetSpelFromSpelerToken/{spelerToken}
            if (speler2Token == null)
            {
                Console.WriteLine($"speler is null: {speler2Token}");
            }

            //! cuz every player must be associated with just one game and cannot make another just after finishing the one b4 and delete it 
            var response = await _httpClient.GetAsync($"api/Spel/spelerToken/{speler2Token}");
            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }

            if (!response.IsSuccessStatusCode)
                throw new Exception(
                    $"Status code was {response.StatusCode} and the statusMessage is {response.RequestMessage}");

            var gameInfo = await response.Content.ReadAsStringAsync();

            Console.WriteLine(gameInfo);
            return JsonConvert.DeserializeObject<Spel>(gameInfo);
        }
        
        public bool Delete(string spelToken)
        {
            var response = _httpClient.DeleteAsync($"api/spel/verwijder/{spelToken}").Result;
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Status code was {response.StatusCode}");
                return false;
            }

            return true;
        }

        
        public async Task<object> Pass(string spelToken, string spelerToken)
        {
            // Create an instance of PassModel
            var passModel = new PassModel
            {
                spelToken = spelToken,
                spelerToken = spelerToken
            };

            // Serialize the passModel instance into JSON
            var json = JsonConvert.SerializeObject(passModel);

            // Create a new StringContent with the JSON data
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            // Send a PUT request to the API
            var response = await _httpClient.PutAsync("api/Spel/pass", data);

            if (response.StatusCode == HttpStatusCode.BadRequest)
            {
                var error = await response.Content.ReadAsStringAsync();
                return error;
            }
            
            // Check the response status code
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Status code was {response.StatusCode} and the statusMessage is {response.RequestMessage}");
            }

            // Read the response content
            var result = await response.Content.ReadAsStringAsync();

            // Deserialize the response content into a Spel instance and return it
            return JsonConvert.DeserializeObject<Spel>(result);
        }

        // public async Task<Spel> Pass(string spelToken, string spelerToken)
        // {
        //     var json = JsonConvert.SerializeObject(spelerToken);
        //     Console.WriteLine(json);
        //     var data = new StringContent(json, Encoding.UTF8, "application/json");
        //
        //     var response = await _httpClient.PutAsync($"api/Spel/pass/{spelToken}", data);
        //
        //     Console.WriteLine(response);
        //     if (!response.IsSuccessStatusCode)
        //     {
        //         throw new Exception($"Status code was {response.StatusCode} and the statusMessage is {response.Content}");
        //     }
        //
        //     var result = await response.Content.ReadAsStringAsync();
        //     Console.WriteLine(result);
        //     return JsonConvert.DeserializeObject<Spel>(result);
        // }
    }

    public class SpelInfoApi
    {
        public string SpelerToken { get; set; }
        public string SpelOmschrijving { get; set; }
    }
}
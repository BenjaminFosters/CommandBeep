using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace CommandBeep.Backend
{
    internal class BeeperSrv
    {
        private readonly HttpClient _httpClient;

        // Temporarily for MVP
        public BeeperSrv(string baseURI, string bearer)
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri(baseURI);

            // Temporary Bearer Token, since it works locally, it's pratically useless if you're mamaged to steal it
            // (tho won't be surprised if GitHub's secret scanning decided to delete it.)

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", bearer);
        }

        public async Task<List<chatsByTitle>> fetchChatList(string query)
        {
            try
            {
                var response = await _httpClient.GetFromJsonAsync<rawResponse>($"v1/chats/search?query={Uri.EscapeDataString(query)}&scope=titles&limit=10");
                return response?.items ?? new List<chatsByTitle>();
            }
            catch (Exception ex)
            {
                return new List<chatsByTitle>();
            }
        }

        public async Task<bool> sendMessage(string ID, string message)
        {
            var response = await _httpClient.PostAsJsonAsync($"v1/chats/{ID}/messages", new { text = message });
            return response.IsSuccessStatusCode;
        }
    }
    internal class rawResponse
    {
        public List<chatsByTitle> items { get; set; }
    }

    internal class chatsByTitle
    {
        public string id { get; set; }
        public string title { get; set; }
        public string network { get; set; }
        public string imgURL { get; set; }
    }
}

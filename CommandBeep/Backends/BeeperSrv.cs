using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace CommandBeep.Backends
{
    internal class BeeperSrv
    {
        private readonly HttpClient _httpClient;
        public BeeperSrv(string baseURI, string bearer)
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri(baseURI);

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", bearer);
        }

        public async Task<FetchChatListResponse> fetchChatList(string query)
        {
            try
            {
                var response = await _httpClient.GetAsync($"v1/chats/search?{(String.IsNullOrEmpty(query) ? "" : $"query={Uri.EscapeDataString(query)}&")}scope=titles");

                return new FetchChatListResponse
                {
                    StatusCode = response.StatusCode,
                    Items = response.IsSuccessStatusCode ? (await response.Content.ReadFromJsonAsync<FetchChatListResponse>())?.Items ?? [] : []
                };
            }
            catch (HttpRequestException) // Error handling for offline Endpoint (which can happen if PowerToys launches first before Beeper)
            {
                return new FetchChatListResponse
                {
                    StatusCode = HttpStatusCode.ServiceUnavailable,
                    Items = []
                };
            }
        }

        public async Task<bool> sendMessage(string ID, string message)
        {
            var response = await _httpClient.PostAsJsonAsync($"v1/chats/{ID}/messages", new { text = message });
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> focusChat(string ID, string draft)
        {
            var response = await _httpClient.PostAsJsonAsync($"v1/focus", new { chatID = ID, draftText = draft });
            return response.IsSuccessStatusCode;
        }
    }
}
internal class FetchChatListResponse
{
    public List<ChatsByTitle> Items { get; set; } = [];
    public HttpStatusCode StatusCode { get; set; }
}

internal class ChatsByTitle
{
    public required string Id { get; set; }
    public required string Title { get; set; }
    public required string Network { get; set; }
    public required string Type { get; set; }
    public bool IsReadOnly { get; set; }
}
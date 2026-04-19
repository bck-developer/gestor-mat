
using Blazored.SessionStorage;
using System.Net.Http.Headers;

namespace GestorMat.Frontend.Services
{
    public class AuthHttpClient
    {
        private readonly HttpClient _http;
        private readonly ISessionStorageService _sessionStorage;

        public AuthHttpClient(HttpClient http, ISessionStorageService sessionStorage)
        {
            _http = http;
            _sessionStorage = sessionStorage;
        }

        public async Task<HttpClient> GetClient()
        {
            var token = await _sessionStorage.GetItemAsync<string>("token");

            if (!string.IsNullOrEmpty(token))
            {
                _http.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", token);
            }

            return _http;
        }
    }
}

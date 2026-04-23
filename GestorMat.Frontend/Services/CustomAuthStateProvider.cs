using Microsoft.AspNetCore.Components.Authorization;
using Blazored.SessionStorage;
using System.Security.Claims;

namespace GestorMat.Frontend.Services
{
    public class CustomAuthStateProvider : AuthenticationStateProvider
    {
        private readonly ISessionStorageService _sessionStorage;

        public CustomAuthStateProvider(ISessionStorageService sessionStorage)
        {
            _sessionStorage = sessionStorage;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var token = await _sessionStorage.GetItemAsync<string>("token");

            if (string.IsNullOrEmpty(token))
            {
                return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
            }

            try
            {
                var claims = ParseClaims(token).ToList();

                // Validate exp claim if present
                var expClaim = claims.FirstOrDefault(c => c.Type == "exp");
                if (expClaim != null && long.TryParse(expClaim.Value, out var expSeconds))
                {
                    var expDate = DateTimeOffset.FromUnixTimeSeconds(expSeconds).UtcDateTime;
                    if (expDate <= DateTime.UtcNow)
                    {
                        // Token expired: remove it and return anonymous
                        await _sessionStorage.RemoveItemAsync("token");
                        return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
                    }
                }

                var identity = new ClaimsIdentity(claims, "jwt");
                var user = new ClaimsPrincipal(identity);

                return new AuthenticationState(user);
            }
            catch
            {
                // If any error parsing/validating token, remove it and return anonymous
                await _sessionStorage.RemoveItemAsync("token");
                return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
            }
        }

        private IEnumerable<Claim> ParseClaims(string jwt)
        {
            string payload = jwt.Split('.')[1];
            byte[] jsonBytes = Convert.FromBase64String(PadBase64(payload));

            Dictionary<string, object>? keyValuePairs =
                System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);

            List<Claim> claims = new List<Claim>();

            foreach (KeyValuePair<string, object> kvp in keyValuePairs ?? new())
            {
                string key = kvp.Key;
                string value = kvp.Value?.ToString() ?? string.Empty;

                if (key == "role" || key == "roles")
                {
                    claims.Add(new Claim(ClaimTypes.Role, value));
                }
                else if (key == "unique_name" || key == "name")
                {
                    claims.Add(new Claim(ClaimTypes.Name, value));
                }
                else
                {
                    claims.Add(new Claim(key, value));
                }
            }

            return claims;
        }

        private string PadBase64(string base64)
        {
            switch (base64.Length % 4)
            {
                case 2: base64 += "=="; break;
                case 3: base64 += "="; break;
            }
            return base64;
        }
    }
}
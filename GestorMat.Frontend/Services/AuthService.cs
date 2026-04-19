using Blazored.SessionStorage;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace GestorMat.Frontend.Services
{
    public class AuthService
    {
        private readonly ISessionStorageService _sessionStorage;

        public AuthService(ISessionStorageService sessionStorage)
        {
            _sessionStorage = sessionStorage;
        }

        public async Task<bool> EstaLogueado()
        {
            var token = await _sessionStorage.GetItemAsync<string>("token");
            return !string.IsNullOrEmpty(token);
        }

        public async Task Logout()
        {
            await _sessionStorage.RemoveItemAsync("token");
        }

        public async Task<string> ObtenerToken()
        {
            return await _sessionStorage.GetItemAsync<string>("token");
        }

        public async Task<(string usuario, string rol)> ObtenerDatosUsuario()
        {
            var token = await _sessionStorage.GetItemAsync<string>("token");

            if (string.IsNullOrEmpty(token))
                return ("", "");

            var handler = new JwtSecurityTokenHandler();
            var jwt = handler.ReadJwtToken(token);

            var usuario = jwt.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;
            var rol = jwt.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;

            return (usuario ?? "Usuario", rol ?? "Sin rol");
        }
    }
}
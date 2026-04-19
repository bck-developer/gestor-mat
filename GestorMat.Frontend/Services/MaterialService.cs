using System.Net.Http.Json;

namespace GestorMat.Frontend.Services
{
    public class MaterialDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public decimal Precio { get; set; }
        public string UnidadMedida { get; set; } = string.Empty;
    }

    public class MaterialService
    {
        private readonly AuthHttpClient _authHttp;

        public MaterialService(AuthHttpClient authHttp)
        {
            _authHttp = authHttp;
        }

        public async Task<List<MaterialDto>> ObtenerMateriales()
        {
            var client = await _authHttp.GetClient();
            var response = await client.GetAsync("api/material");

            if (!response.IsSuccessStatusCode)
                throw new Exception("Error al obtener materiales");

            return await response.Content.ReadFromJsonAsync<List<MaterialDto>>()
                   ?? new List<MaterialDto>();
        }
    }
}

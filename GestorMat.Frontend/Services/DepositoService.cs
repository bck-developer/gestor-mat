using System.Net.Http.Json;

namespace GestorMat.Frontend.Services;

public class DepositoDto
{
    public int Id_Deposito { get; set; }
    public string CodigoDeposito { get; set; } = string.Empty;
    public string Nombre { get; set; } = string.Empty;
    public string Direccion { get; set; } = string.Empty;
    public bool Habilitado { get; set; }
}

public class CrearDepositoDto
{
    public string CodigoDeposito { get; set; } = string.Empty;
    public string Nombre { get; set; } = string.Empty;
    public string Direccion { get; set; } = string.Empty;
    public bool Habilitado { get; set; } = true;
}

public class DepositoService(AuthHttpClient authHttp)
{
    public async Task<List<DepositoDto>> ObtenerDepositos()
    {
        HttpClient client = await authHttp.GetClient();
        HttpResponseMessage response = await client.GetAsync("api/deposito");

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception("Error al obtener depósitos");
        }

        List<DepositoDto>? depositos = await response.Content.ReadFromJsonAsync<List<DepositoDto>>();
        return depositos ?? new List<DepositoDto>();
    }

    public async Task<DepositoDto?> ObtenerDepositoPorId(int id)
    {
        HttpClient client = await authHttp.GetClient();
        HttpResponseMessage response = await client.GetAsync($"api/deposito/{id}");

        if (!response.IsSuccessStatusCode)
        {
            return null;
        }

        DepositoDto? deposito = await response.Content.ReadFromJsonAsync<DepositoDto>();
        return deposito;
    }

    public async Task CrearDeposito(CrearDepositoDto dto)
    {
        HttpClient client = await authHttp.GetClient();
        HttpResponseMessage response = await client.PostAsJsonAsync("api/deposito", dto);

        if (!response.IsSuccessStatusCode)
        {
            string errorContent = await response.Content.ReadAsStringAsync();
            throw new Exception($"Error al crear depósito: {errorContent}");
        }
    }

    public async Task EditarDeposito(int id, CrearDepositoDto dto)
    {
        HttpClient client = await authHttp.GetClient();
        HttpResponseMessage response = await client.PutAsJsonAsync($"api/deposito/{id}", dto);

        if (!response.IsSuccessStatusCode)
        {
            string errorContent = await response.Content.ReadAsStringAsync();
            throw new Exception($"Error al editar depósito: {errorContent}");
        }
    }

    public async Task EliminarDeposito(int id)
    {
        HttpClient client = await authHttp.GetClient();
        HttpResponseMessage response = await client.DeleteAsync($"api/deposito/{id}");

        if (!response.IsSuccessStatusCode)
        {
            string errorContent = await response.Content.ReadAsStringAsync();
            throw new Exception($"Error al eliminar depósito: {errorContent}");
        }
    }

    public async Task InhabilitarDeposito(int id)
    {
        HttpClient client = await authHttp.GetClient();
        HttpResponseMessage response = await client.PatchAsync($"api/deposito/{id}/inhabilitar", null);

        if (!response.IsSuccessStatusCode)
        {
            string errorContent = await response.Content.ReadAsStringAsync();
            throw new Exception($"Error al inhabilitar depósito: {errorContent}");
        }
    }

    public async Task HabilitarDeposito(int id)
    {
        HttpClient client = await authHttp.GetClient();
        HttpResponseMessage response = await client.PatchAsync($"api/deposito/{id}/habilitar", null);

        if (!response.IsSuccessStatusCode)
        {
            string errorContent = await response.Content.ReadAsStringAsync();
            throw new Exception($"Error al habilitar depósito: {errorContent}");
        }
    }
}

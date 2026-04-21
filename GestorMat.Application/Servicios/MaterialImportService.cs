using GestorMat.Application.DTOs.Material;
using GestorMat.Application.Interfaces;
using GestorMat.Domain.Entidades;

namespace GestorMat.Application.Servicios;

public class MaterialImportService
{
    private readonly IMaterialRepository _materialRepo;
    private readonly IUnidadMedidaRepository _unidadRepo;

    public MaterialImportService(IMaterialRepository materialRepo, IUnidadMedidaRepository unidadRepo)
    {
        _materialRepo = materialRepo;
        _unidadRepo = unidadRepo;
    }

    public async Task<ResultadoImportacionDto> ImportarAsync(List<ImportarMaterialDto> items)
    {
        var resultado = new ResultadoImportacionDto();
        var unidades = await _unidadRepo.ObtenerTodosAsync();

        var unidadesDict = unidades.ToDictionary(u => u.Nombre.ToLower());

        var materiales = new List<Material>();

        int fila = 2;

        foreach (var item in items)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(item.Nombre))
                    throw new Exception("Nombre vacío");

                if (!unidadesDict.ContainsKey(item.UnidadMedida.ToLower()))
                    throw new Exception("Unidad inválida");

                var unidad = unidadesDict[item.UnidadMedida.ToLower()];

                var material = new Material(
                    item.CodigoMaterial,
                    item.Nombre,
                    item.Precio,
                    unidad.Id_UnidadMedida,
                    item.Activo,
                    item.Descripcion,
                    item.PermiteStockNegativo,
                    item.StockMinimo
                );

                materiales.Add(material);
            }
            catch (Exception ex)
            {
                resultado.Errores.Add($"Fila {fila}: {ex.Message}");
            }

            fila++;
        }

        if (materiales.Any())
            await _materialRepo.AgregarRangoAsync(materiales);

        resultado.RegistrosInsertados = materiales.Count;

        return resultado;
    }
}
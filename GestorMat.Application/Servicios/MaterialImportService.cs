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

    public async Task<MaterialImportResultDto> ImportarAsync(List<MaterialExcelRowDto> filas)
    {
        var resultado = new MaterialImportResultDto();

        var unidades = (await _unidadRepo.ObtenerTodosAsync(false))
            .ToDictionary(u => u.Nombre.ToLower());

        foreach (var row in filas)
        {
            try
            {
                var dto = Mapear(row, unidades);

                Validar(dto);

                var material = new Material(
                    dto.CodigoMaterial,
                    dto.Nombre,
                    dto.Precio,
                    dto.Id_UnidadMedida,
                    dto.Activo,
                    dto.Descripcion,
                    dto.PermiteStockNegativo,
                    dto.StockMinimo
                );

                await _materialRepo.AgregarAsync(material);

                resultado.Exitosos++;
            }
            catch (Exception ex)
            {
                resultado.Fallidos++;

                resultado.Errores.Add(new ErrorImportacionDto
                {
                    Fila = row.Fila,
                    Mensaje = ex.Message
                });
            }
        }

        resultado.TotalProcesados = filas.Count;

        return resultado;
    }
    private CrearMaterialDto Mapear(MaterialExcelRowDto row, Dictionary<string, UnidadMedida> unidades)
    {
        if (!decimal.TryParse(row.PrecioRaw, out var precio))
            throw new Exception($"Precio inválido: '{row.PrecioRaw}'");

        if (!double.TryParse(row.StockMinimoRaw, out var stock))
            throw new Exception($"Stock mínimo inválido: '{row.StockMinimoRaw}'");

        if (!unidades.TryGetValue(row.UnidadNombre.ToLower(), out var unidad))
            throw new Exception($"Unidad '{row.UnidadNombre}' no existe");

        return new CrearMaterialDto
        {
            CodigoMaterial = row.CodigoMaterial,
            Nombre = row.Nombre,
            Precio = precio,
            Id_UnidadMedida = unidad.Id_UnidadMedida,
            Descripcion = row.Descripcion,
            Activo = ParseBool(row.ActivoRaw),
            PermiteStockNegativo = ParseBool(row.PermiteStockNegativoRaw),
            StockMinimo = stock
        };
    }
    private void Validar(CrearMaterialDto dto)
    {
        
        if (string.IsNullOrWhiteSpace(dto.CodigoMaterial))
            throw new Exception("Código obligatorio");

        if (dto.CodigoMaterial.Length > 30)
            throw new Exception("Código máximo 30 caracteres");

        if (string.IsNullOrWhiteSpace(dto.Nombre))
            throw new Exception("Nombre obligatorio");

        if (dto.Nombre.Length > 80)
            throw new Exception("Nombre máximo 80 caracteres");

        if (dto.Precio <= 0)
            throw new Exception("Precio debe ser mayor a 0");

        if (dto.StockMinimo < 0)
            throw new Exception("Stock mínimo no puede ser negativo");
    }
    private bool ParseBool(string value)
    {
        var v = value.Trim().ToUpper();

        return v == "SI" || v == "TRUE" || v == "1";
    }
}
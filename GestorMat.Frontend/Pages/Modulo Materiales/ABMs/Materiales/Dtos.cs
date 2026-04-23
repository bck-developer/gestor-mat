using System.ComponentModel.DataAnnotations;

namespace GestorMat.Frontend.Pages.Modulo_Materiales.Materiales;

public class MaterialDto
{
    public int Id_Material { get; set; }

    [Required(ErrorMessage = "El código es obligatorio")]
    public string CodigoMaterial { get; set; } = string.Empty;
    [Required(ErrorMessage = "El nombre es obligatorio")]
    public string Nombre { get; set; } = string.Empty;
    public string Descripcion { get; set; } = string.Empty;

    [Required(ErrorMessage = "El precio es obligatorio")]
    [Range(1, int.MaxValue, ErrorMessage = "Debe ingresar un precio válido")]
    public decimal Precio { get; set; }

    [Required(ErrorMessage = "La unidad de medida es obligatoria")]
    [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar una unidad válida")]
    public int Id_UnidadMedida { get; set; }
    public double? StockMinimo { get; set; }
    public bool PermiteStockNegativo { get; set; }
    public bool Activo { get; set; }
    public string UnidadMedida { get; set; } = string.Empty;
}

public class UnidadMedidaDto
{
    public int Id_UnidadMedida { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Abreviatura { get; set; } = string.Empty;
}

public class CrearMaterialDto
{
    [Required(ErrorMessage = "El código es obligatorio")]
    public string CodigoMaterial { get; set; } = string.Empty;
    [Required(ErrorMessage = "El nombre es obligatorio")]
    public string Nombre { get; set; } = string.Empty;
    public string Descripcion { get; set; } = string.Empty;

    [Required(ErrorMessage = "El precio es obligatorio")]
    [Range(1, int.MaxValue, ErrorMessage = "Debe ingresar un precio válido")]
    public decimal Precio { get; set; }

    [Required(ErrorMessage = "La unidad de medida es obligatoria")]
    [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar una unidad válida")]
    public int Id_UnidadMedida { get; set; }
    public double? StockMinimo { get; set; }
    public bool PermiteStockNegativo { get; set; } = false;
    public bool Activo { get; set; } = true;
}

public class MaterialImportResultDto
{
    public int TotalProcesados { get; set; }
    public int Exitosos { get; set; }
    public int Fallidos { get; set; }
    public List<ErrorImportacionDto> Errores { get; set; } = new();
}

public class ErrorImportacionDto
{
    public int Fila { get; set; }
    public string Mensaje { get; set; } = string.Empty;
}

using System.ComponentModel.DataAnnotations;

namespace GestorMat.Frontend.Pages.Modulo_Materiales.Materiales;

public abstract class MaterialBaseDto
{
    [Required(ErrorMessage = "El código es obligatorio")]
    public string CodigoMaterial { get; set; } = string.Empty;

    [Required(ErrorMessage = "El nombre es obligatorio")]
    public string Nombre { get; set; } = string.Empty;

    [StringLength(255, ErrorMessage = "La descripción no puede superar los 255 caracteres")]
    public string Descripcion { get; set; } = string.Empty;

    [Required(ErrorMessage = "El precio es obligatorio")]
    public virtual decimal Precio { get; set; }

    [Required(ErrorMessage = "La unidad de medida es obligatoria")]
    public virtual int Id_UnidadMedida { get; set; }

    public virtual double? StockMinimo { get; set; }

    public bool PermiteStockNegativo { get; set; }
    public bool Activo { get; set; }
}

public class MaterialDto : MaterialBaseDto
{
    public int Id_Material { get; set; }

    [Range(0, int.MaxValue, ErrorMessage = "Debe ingresar un precio válido")]
    public override decimal Precio { get; set; }

    [Range(0, int.MaxValue, ErrorMessage = "Debe seleccionar una unidad válida")]
    public override int Id_UnidadMedida { get; set; }

    [Range(0, int.MaxValue, ErrorMessage = "Debe seleccionar una número válido")]
    public override double? StockMinimo { get; set; }

    public string UnidadMedida { get; set; } = string.Empty;
}

public class UnidadMedidaDto
{
    public int Id_UnidadMedida { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Abreviatura { get; set; } = string.Empty;
}

public class CrearMaterialDto : MaterialBaseDto
{
    [Range(1, int.MaxValue, ErrorMessage = "Debe ingresar un precio válido")]
    public override decimal Precio { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar una unidad válida")]
    public override int Id_UnidadMedida { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Debe ingresar un valor válido")]
    public override double? StockMinimo { get; set; }

    public new bool PermiteStockNegativo { get; set; } = false;
    public new bool Activo { get; set; } = true;
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

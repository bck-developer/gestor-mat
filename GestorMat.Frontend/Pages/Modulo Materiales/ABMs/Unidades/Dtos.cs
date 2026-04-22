using System.ComponentModel.DataAnnotations;

namespace GestorMat.Frontend.Pages.Modulo_Materiales.Unidades;

public class UnidadMedidaDto
{
    public int Id_UnidadMedida { get; set; }

    [Required(ErrorMessage = "El nombre es obligatorio")]
    [StringLength(30, MinimumLength = 2, ErrorMessage = "La longitud del nombre debe estar entre 2 y 30 caracteres")]
    public string Nombre { get; set; } = string.Empty;

    [Required(ErrorMessage = "La abreviatura es obligatoria")]
    [StringLength(10, MinimumLength = 1, ErrorMessage = "La longitud de la abreviatura debe estar entre 1 y 10 caracteres")]
    public string Abreviatura { get; set; } = string.Empty;
    public bool Activo { get; set; }
}

public class CrearUnidadMedidaDto
{
    [Required(ErrorMessage = "El nombre es obligatorio")]
    [StringLength(30, MinimumLength = 2, ErrorMessage = "La longitud del nombre debe estar entre 2 y 30 caracteres")]
    public string Nombre { get; set; } = string.Empty;

    [Required(ErrorMessage = "La abreviatura es obligatoria")]
    [StringLength(10, MinimumLength = 1, ErrorMessage = "La longitud de la abreviatura debe estar entre 1 y 10 caracteres")]
    public string Abreviatura { get; set; } = string.Empty;
    public bool Activo { get; set; }
}
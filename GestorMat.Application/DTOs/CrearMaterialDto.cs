using System.ComponentModel.DataAnnotations;

namespace GestorMat.Application.DTOs;

public class CrearMaterialDto
{
    [Required]
    public required string Nombre { get; set; }

    [Range(1, double.MaxValue)]
    public decimal Precio { get; set; }

    [Required]
    public int IdUnidadMedida { get; set; }
}

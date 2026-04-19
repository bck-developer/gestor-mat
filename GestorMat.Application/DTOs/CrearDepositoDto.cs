using System.ComponentModel.DataAnnotations;

namespace GestorMat.Application.DTOs;

public class CrearDepositoDto
{
    [Required]
    public required string CodigoDeposito { get; set; }

    [Required]
    public required string Nombre { get; set; }

    [Required]
    public required string Direccion { get; set; }

    public bool Habilitado { get; set; } = true;
}

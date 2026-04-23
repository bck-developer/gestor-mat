using System.ComponentModel.DataAnnotations;

namespace GestorMat.Frontend.Pages.Modulo_Materiales.ABMs.Depositos;
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
    [Required(ErrorMessage = "El código es obligatorio")]
    public string CodigoDeposito { get; set; } = string.Empty;

    [Required(ErrorMessage = "El nombre es obligatorio")]
    public string Nombre { get; set; } = string.Empty;

    [Required(ErrorMessage = "La dirección es obligatoria")]
    public string Direccion { get; set; } = string.Empty;
    public bool Habilitado { get; set; } = true;
}
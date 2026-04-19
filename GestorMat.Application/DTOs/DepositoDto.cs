namespace GestorMat.Application.DTOs;

public class DepositoDto
{
    public int Id_Deposito { get; set; }
    public required string CodigoDeposito { get; set; }
    public required string Nombre { get; set; }
    public required string Direccion { get; set; }
    public bool Habilitado { get; set; }
}

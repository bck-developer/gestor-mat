namespace GestorMat.Application.Queries;

public class SaldoQuery
{
    public int? IdMaterial { get; set; }
    public int? IdDeposito { get; set; }

    public bool IncluirMaterialesInactivos { get; set; }
    public bool IncluirDepositosInactivos { get; set; }
}

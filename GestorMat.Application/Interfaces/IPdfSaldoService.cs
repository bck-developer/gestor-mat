using GestorMat.Application.DTOs.Saldo;

namespace GestorMat.Application.Interfaces;

public interface IPdfSaldoService
{
    byte[] GenerarReporte(List<SaldoDto> data, string filtros);
}

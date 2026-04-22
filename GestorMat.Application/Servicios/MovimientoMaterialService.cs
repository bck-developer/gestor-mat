using GestorMat.Application.DTOs.MovimientoMaterial;
using GestorMat.Application.Interfaces;
using GestorMat.Domain.Entidades;

public class MovimientoMaterialService(
    IMovimientoMaterialRepository movRepo,
    ISaldoRepository saldoRepo,
    IUnitOfWork uow)
{
    public async Task CrearAsync(CrearMovimientoMaterialDto dto)
    {
        await uow.BeginTransactionAsync();

        try
        {
            await ProcesarSaldo(dto);

            MovimientoMaterial movimiento = new MovimientoMaterial(
                dto.IdMaterial,
                dto.IdDepositoOrigen,
                dto.IdDepositoDestino,
                dto.CodigoMovimiento,
                dto.Tipo,
                dto.Cantidad,
                dto.Fecha,
                dto.UserName
            );

            await movRepo.AgregarAsync(movimiento);

            await uow.CommitAsync();

        }
        catch
        {
            await uow.RollbackAsync();
            throw;
        }
    }

    private async Task ProcesarSaldo(CrearMovimientoMaterialDto dto)
    {
        switch (dto.Tipo)
        {
            case "Ingreso":
                await Sumar(dto.IdMaterial, dto.IdDepositoOrigen, dto.Cantidad);
                break;

            case "Egreso":
                await Restar(dto.IdMaterial, dto.IdDepositoOrigen, dto.Cantidad);
                break;

            case "Ingreso/Egreso":
                await Restar(dto.IdMaterial, dto.IdDepositoOrigen, dto.Cantidad);
                await Sumar(dto.IdMaterial, dto.IdDepositoDestino!.Value, dto.Cantidad);
                break;

            default:
                throw new Exception("Tipo de movimiento inválido");
        }
    }

    private async Task Sumar(int mat, int dep, double cant)
    {
        Saldo? saldo = await saldoRepo.ObtenerAsync(mat, dep);

        if (saldo == null)
        {
            await saldoRepo.AgregarAsync(new Saldo(mat, dep, cant, DateTime.Now));
        }
        else
        {
            saldo.Incrementar(cant);
            await saldoRepo.ActualizarAsync(saldo);
        }
    }

    private async Task Restar(int mat, int dep, double cant)
    {
        Saldo? saldo = await saldoRepo.ObtenerAsync(mat, dep);

        if (saldo == null)
        {
            await saldoRepo.AgregarAsync(new Saldo(mat, dep, -cant, DateTime.Now));
        }
        else
        {
            saldo.Decrementar(cant);
            await saldoRepo.ActualizarAsync(saldo);
        }
    }

    public async Task<List<MovimientoMaterialDto>> ObtenerTodosAsync()
    {
        List<MovimientoMaterial> lista = await movRepo.ObtenerTodosAsync();

        return lista.Select(x => new MovimientoMaterialDto
        {
            Id_MovimientoMaterial = x.Id_MovimientoMaterial,
            CodigoMovimiento = x.CodigoMovimiento,
            Tipo = x.Tipo,
            Cantidad = x.Cantidad,
            Fecha = x.Fecha,
            Id_Material = x.Id_Material,
            Id_DepositoOrigen = x.Id_DepositoOrigen,
            Id_DepositoDestino = x.Id_DepositoDestino,
            UserName = x.UserName
        }).ToList();
    }

    public async Task<MovimientoMaterial?> ObtenerPorIdAsyncService(int id) =>
         await movRepo.ObtenerPorIdAsync(id);

    public async Task<MovimientoMaterial?> ObtenerPorCodigoAsync(string codigo) =>
         await movRepo.ObtenerPorCodigoAsync(codigo);
}
using GestorMat.Application.DTOs.Saldo;
using GestorMat.Application.Interfaces;
using GestorMat.Application.Queries;
using GestorMat.Domain.Entidades;
using GestorMat.Infrastructure.Persistencia;
using Microsoft.EntityFrameworkCore;

namespace GestorMat.Infrastructure.Repositorios;

public class SaldoRepository(AppDbContext context) : ISaldoRepository
{
    public async Task<Saldo?> ObtenerAsync(int idMaterial, int idDeposito) =>

         await context.Saldos
            .FirstOrDefaultAsync(x =>
                x.Id_Material == idMaterial &&
                x.Id_Deposito == idDeposito);


    public async Task AgregarAsync(Saldo saldo) => await context.Saldos.AddAsync(saldo);

    public Task ActualizarAsync(Saldo saldo)
    {
        context.Saldos.Update(saldo);
        return Task.CompletedTask;
    }

    public async Task<List<SaldoDto>> ObtenerSaldos(SaldoQuery query)
    {
        IQueryable<SaldoDto> queryable =
            from s in context.Saldos
            join m in context.Materiales on s.Id_Material equals m.Id_Material
            join d in context.Depositos on s.Id_Deposito equals d.Id_Deposito
            join u in context.UnidadesMedida on m.Id_UnidadMedida equals u.Id_UnidadMedida

            where
                (query.IdMaterial == null || s.Id_Material == query.IdMaterial) &&
                (query.IdDeposito == null || s.Id_Deposito == query.IdDeposito) &&
                (query.IncluirMaterialesInactivos || m.Activo) &&
                (query.IncluirDepositosInactivos || d.Habilitado)

            select new SaldoDto
            {
                IdMaterial = m.Id_Material,
                CodigoMaterial = m.CodigoMaterial,
                NombreMaterial = m.Nombre,

                IdDeposito = d.Id_Deposito,
                CodigoDeposito = d.CodigoDeposito,
                NombreDeposito = d.Nombre,

                UnidadMedida = u.Abreviatura,

                CantidadDisponible = s.Cantidad,
                StockMinimo = m.StockMinimo,

                FechaUltimoMovimiento = s.FechaUltimaModificacion
            };

        return await queryable.ToListAsync();
    }
}

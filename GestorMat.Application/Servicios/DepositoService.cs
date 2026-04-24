using GestorMat.Application.DTOs;
using GestorMat.Application.Interfaces;
using GestorMat.Domain.Entidades;
using Mapster;

namespace GestorMat.Application.Servicios;

public class DepositoService(IDepositoRepository depositoRepository)
{
    public async Task CrearDepositoAsync(CrearDepositoDto dto)
    {
        Deposito deposito = new Deposito(dto.CodigoDeposito, dto.Nombre, dto.Direccion, dto.Habilitado);
        await depositoRepository.AgregarAsync(deposito);
    }

    public async Task<List<DepositoDto>> ObtenerDepositosAsync()
    {
        IEnumerable<Deposito> depositos = await depositoRepository.ObtenerTodosAsync();
        return depositos.Adapt<List<DepositoDto>>();
    }

    public async Task<DepositoDto?> ObtenerDepositoPorIdAsync(int id)
    {
        Deposito? deposito = await depositoRepository.ObtenerPorIdAsync(id);
        return deposito?.Adapt<DepositoDto>();
    }

    public async Task EditarDepositoAsync(int id, CrearDepositoDto dto)
    {
        Deposito? deposito = await depositoRepository.ObtenerPorIdAsync(id);

        if (deposito == null)
        {
            throw new Exception("Depósito no encontrado");
        }

        Deposito depositoActualizado = new Deposito(dto.CodigoDeposito, dto.Nombre, dto.Direccion, dto.Habilitado)
        {
            Id_Deposito = id
        };

        await depositoRepository.ActualizarAsync(depositoActualizado);
    }

    public async Task EliminarDepositoAsync(int id)
    {
        Deposito? deposito = await depositoRepository.ObtenerPorIdAsync(id);

        if (deposito == null)
        {
            throw new Exception("Depósito no encontrado");
        }

        await depositoRepository.EliminarAsync(id);
    }

    public async Task InhabilitarDepositoAsync(int id)
    {
        Deposito? deposito = await depositoRepository.ObtenerPorIdAsync(id);

        if (deposito == null)
        {
            throw new Exception("Depósito no encontrado");
        }

        if (!deposito.Habilitado)
        {
            throw new Exception("El depósito ya está inhabilitado");
        }

        Deposito depositoActualizado = new Deposito(deposito.CodigoDeposito, deposito.Nombre, deposito.Direccion, false)
        {
            Id_Deposito = id
        };

        await depositoRepository.ActualizarAsync(depositoActualizado);
    }

    public async Task HabilitarDepositoAsync(int id)
    {
        Deposito? deposito = await depositoRepository.ObtenerPorIdAsync(id);

        if (deposito == null)
        {
            throw new Exception("Depósito no encontrado");
        }

        if (deposito.Habilitado)
        {
            throw new Exception("El depósito ya está habilitado");
        }

        Deposito depositoActualizado = new Deposito(deposito.CodigoDeposito, deposito.Nombre, deposito.Direccion, true)
        {
            Id_Deposito = id
        };

        await depositoRepository.ActualizarAsync(depositoActualizado);
    }
}

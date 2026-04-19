using GestorMat.Application.DTOs;
using GestorMat.Application.Interfaces;
using GestorMat.Domain.Entidades;

namespace GestorMat.Application.Servicios;

public class RolService(IRolRepository repo)
{
    public async Task<List<RolDto>> ObtenerAsync()
    {
        List<Rol> roles = await repo.ObtenerTodosAsync();

        return [.. roles.Select(r => new RolDto
        {
            IdRol = r.IdRol,
            RolName = r.RolName
        })];
    }
}

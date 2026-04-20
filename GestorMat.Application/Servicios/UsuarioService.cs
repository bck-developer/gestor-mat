using GestorMat.Application.DTOs;
using GestorMat.Application.Interfaces;
using GestorMat.Domain.Entidades;

namespace GestorMat.Application.Servicios;

public class UsuarioService(IUsuarioRepository repo, IPasswordHasher hasher)
{
    // 1 - CREAR
    public async Task CrearAsyncService(CrearUsuarioDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Password))
        {
            throw new Exception("La contraseña es obligatoria");
        }

        if (dto.IdRol == 0)
        {
            throw new Exception("Debe seleccionar un rol");
        }

        string hash = hasher.Hash(dto.Password);

        Usuario usuario = new Usuario(
            dto.Username,
            hash,
            dto.Nombre,
            dto.Mail,
            dto.IdRol,
            dto.Activo
        );

        await repo.AgregarAsync(usuario);
    }

    // 2 - OBTENER TODOS
    public async Task<List<UsuarioDto>> ObtenerTodosAsyncService()
    {
        IEnumerable<Usuario> usuarios = await repo.ObtenerTodosAsync();

        return usuarios.Select(u => new UsuarioDto
        {
            Id = u.Id,
            Username = u.Username,
            Nombre = u.Nombre,
            Mail = u.Mail,
            Rol = u.Rol?.RolName ?? string.Empty,
            Activo = u.Activo
        }).ToList();
    }

    // 3 - OBTENER POR ID
    public async Task<Usuario?> ObtenerPorIdAsyncService(int id)
    {
        return await repo.ObtenerPorIdAsync(id);
    }

    // 4- ACTUALIZAR
    public async Task ActualizarAsyncService(int id, CrearUsuarioDto dto)
    {
        Usuario? usuario = await repo.ObtenerPorIdAsync(id);

        if (usuario == null)
        {
            throw new Exception("Usuario no encontrado");
        }

        usuario.ActualizarDatos(dto.Username, dto.Nombre, dto.Mail, dto.IdRol, dto.Activo);

        if (!string.IsNullOrWhiteSpace(dto.Password))
        {
            string hash = hasher.Hash(dto.Password);
            usuario.CambiarPassword(hash);
        }

        await repo.EditarAsync(usuario);
    }

    // 5 - ELIMINAR FISICO 
    public async Task EliminarFisicoAsync(int id)
    {
        Usuario? usuario = await repo.ObtenerPorIdAsync(id);

        if (usuario == null)
        {
            throw new Exception("Usuario no encontrado");
        }

        await repo.EliminarFisicoAsync(usuario);
    }

    // ADICIONALES

    // INHABILITAR
    public async Task InhabilitarAsyncService(int id)
    {
        Usuario? usuario = await repo.ObtenerPorIdAsync(id);

        if (usuario == null)
        {
            throw new Exception("Usuario no encontrado");
        }

        usuario.Desactivar();

        await repo.EditarAsync(usuario);
    }

    // REHABILITAR
    public async Task RehabilitarAsyncService(int id)
    {
        Usuario? usuario = await repo.ObtenerPorIdAsync(id);

        if (usuario == null)
        {
            throw new Exception("Usuario no encontrado");
        }

        usuario.Activar();

        await repo.EditarAsync(usuario);
    }
}
using GestorMat.Application.DTOs;
using GestorMat.Application.Interfaces;
using GestorMat.Domain.Entidades;

namespace GestorMat.Application.Servicios;

public class UsuarioService(IUsuarioRepository repo, IPasswordHasher hasher)
{

    // =========================
    // CREAR
    // =========================
    public async Task CrearAsync(CrearUsuarioDto dto)
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
            dto.IdRol
        );

        await repo.AgregarAsync(usuario);
    }

    // =========================
    // LISTAR
    // =========================
    public async Task<List<UsuarioDto>> ObtenerAsync()
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

    // =========================
    // OBTENER POR ID
    // =========================
    public async Task<Usuario?> ObtenerEntidadPorIdAsync(int id)
    {
        return await repo.ObtenerPorIdAsync(id);
    }

    // =========================
    // EDITAR
    // =========================
    public async Task EditarAsync(int id, CrearUsuarioDto dto)
    {
        Usuario? usuario = await repo.ObtenerPorIdAsync(id);

        if (usuario == null)
        {
            throw new Exception("Usuario no encontrado");
        }

        usuario.ActualizarDatos(dto.Username, dto.Nombre, dto.Mail, dto.IdRol);

        if (!string.IsNullOrWhiteSpace(dto.Password))
        {
            string hash = hasher.Hash(dto.Password);
            usuario.CambiarPassword(hash);
        }

        await repo.EditarAsync(usuario);
    }

    // =========================
    // ELIMINAR (soft delete)
    // =========================
    public async Task EliminarAsync(int id)
    {
        Usuario? usuario = await repo.ObtenerPorIdAsync(id);

        if (usuario == null)
        {
            throw new Exception("Usuario no encontrado");
        }

        usuario.Desactivar();

        await repo.EditarAsync(usuario);
    }

    // =========================
    // INHABILITAR
    // =========================
    public async Task InhabilitarAsync(int id)
    {
        Usuario? usuario = await repo.ObtenerPorIdAsync(id);

        if (usuario == null)
        {
            throw new Exception("Usuario no encontrado");
        }

        usuario.Desactivar();

        await repo.EditarAsync(usuario);
    }

    // =========================
    // REHABILITAR
    // =========================
    public async Task RehabilitarAsync(int id)
    {
        Usuario? usuario = await repo.ObtenerPorIdAsync(id);

        if (usuario == null)
        {
            throw new Exception("Usuario no encontrado");
        }

        usuario.Activar();

        await repo.EditarAsync(usuario);
    }

    // =========================
    // ELIMINAR FISICO (OPCIONAL)
    // =========================
    public async Task EliminarFisicoAsync(int id)
    {
        Usuario? usuario = await repo.ObtenerPorIdAsync(id);

        if (usuario == null)
        {
            throw new Exception("Usuario no encontrado");
        }

        await repo.EliminarFisicoAsync(usuario);
    }
}
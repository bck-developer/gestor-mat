using GestorMat.Application.DTOs;
using GestorMat.Application.Interfaces;
using GestorMat.Domain.Entidades;
using Mapster;

namespace GestorMat.Application.Servicios;

public class UsuarioService(IUsuarioRepository repository, IPasswordHasher hasher)
{
    // 1 - CREAR
    public async Task CrearAsyncService(CrearUsuarioDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Password))
        {
            throw new Exception("La contraseña es obligatoria");
        }

        if (dto.Id_Rol == 0)
        {
            throw new Exception("Debe seleccionar un rol");
        }

        string hash = hasher.Hash(dto.Password);

        Usuario usuario = new Usuario(
            dto.Username,
            hash,
            dto.Nombre,
            dto.Mail,
            dto.Id_Rol,
            dto.Activo
        );

        await repository.AgregarAsync(usuario);
    }

    // 2 - OBTENER TODOS
    public async Task<List<UsuarioDto>> ObtenerTodosAsyncService()
    {
        IEnumerable<Usuario> usuarios = await repository.ObtenerTodosAsync();
        return usuarios.Adapt<List<UsuarioDto>>();
    }

    // 3 - OBTENER POR ID
    public async Task<Usuario?> ObtenerPorIdAsyncService(int id)
    {
        return await repository.ObtenerPorIdAsync(id);
    }

    // 4- ACTUALIZAR
    public async Task ActualizarAsyncService(int id, CrearUsuarioDto dto)
    {
        Usuario? usuario = await repository.ObtenerPorIdAsync(id);

        if (usuario == null)
        {
            throw new Exception("Usuario no encontrado");
        }

        usuario.ActualizarDatos(dto.Username, dto.Nombre, dto.Mail, dto.Id_Rol, dto.Activo);

        if (!string.IsNullOrWhiteSpace(dto.Password))
        {
            string hash = hasher.Hash(dto.Password);
            usuario.CambiarPassword(hash);
        }

        await repository.EditarAsync(usuario);
    }

    // 5 - ELIMINAR FISICO 
    public async Task EliminarFisicoAsync(int id)
    {
        Usuario? usuario = await repository.ObtenerPorIdAsync(id);

        if (usuario == null)
        {
            throw new Exception("Usuario no encontrado");
        }

        await repository.EliminarAsync(usuario);
    }

    // ADICIONALES

    // INHABILITAR
    public async Task InhabilitarAsyncService(int id)
    {
        Usuario? usuario = await repository.ObtenerPorIdAsync(id);

        if (usuario == null)
        {
            throw new Exception("Usuario no encontrado");
        }

        usuario.Desactivar();

        await repository.EditarAsync(usuario);
    }

    // REHABILITAR
    public async Task RehabilitarAsyncService(int id)
    {
        Usuario? usuario = await repository.ObtenerPorIdAsync(id);

        if (usuario == null)
        {
            throw new Exception("Usuario no encontrado");
        }

        usuario.Activar();

        await repository.EditarAsync(usuario);
    }
}
using GestorMat.Application.Interfaces;
using GestorMat.Domain.Entidades;
using GestorMat.Infrastructure.Persistencia;
using Microsoft.EntityFrameworkCore;

namespace GestorMat.Infrastructure.Repositorios;

public class UsuarioRepository(AppDbContext context) : IUsuarioRepository
{
    public async Task AgregarAsync(Usuario usuario)
    {
        await context.Usuarios.AddAsync(usuario);
        await context.SaveChangesAsync();
    }

    public async Task<Usuario?> ObtenerPorUsernameAsync(string username)
    {
        return await context.Usuarios
            .Include(u => u.Rol)
            .FirstOrDefaultAsync(u => u.Username == username);
    }

    public async Task<List<Usuario>> ObtenerTodosAsync()
    {
        return await context.Usuarios
            .Include(u => u.Rol)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<Usuario?> ObtenerPorIdAsync(int id)
    {
        return await context.Usuarios
            .Include(u => u.Rol)
            .FirstOrDefaultAsync(u => u.Id_Usuario == id);
    }

    public async Task EditarAsync(Usuario usuario)
    {
        context.Usuarios.Update(usuario);
        await context.SaveChangesAsync();
    }

    public async Task EliminarAsync(Usuario usuario)
    {
        context.Usuarios.Remove(usuario);
        await context.SaveChangesAsync();
    }
}
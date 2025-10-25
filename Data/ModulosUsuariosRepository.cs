using Domain.Model;
using Microsoft.EntityFrameworkCore;

namespace Data
{
    public class ModulosUsuariosRepository
    {
        private readonly AcademiaContext _context;

        public ModulosUsuariosRepository(AcademiaContext context)
        {
            _context = context;
        }
        public async Task<List<ModulosUsuarios>> GetAllAsync()
        {
            return await _context.ModulosUsuarios
                .Include(mu => mu.Modulo)
                .Include(mu => mu.Usuario)
                .ToListAsync();
        }

        public async Task<ModulosUsuarios?> GetByIdAsync(int id)
        {
            return await _context.ModulosUsuarios
                .Include(mu => mu.Modulo)
                .Include(mu => mu.Usuario)
                .FirstOrDefaultAsync(mu => mu.Id_ModuloUsuario == id);
        }

        public async Task<ModulosUsuarios?> GetByUsuarioIdAsync(int usuarioId)
        {
            return await _context.ModulosUsuarios
                .Include(mu => mu.Modulo)
                .FirstOrDefaultAsync(mu => mu.UsuarioId == usuarioId);
        }

        public async Task<List<ModulosUsuarios>> GetByModuloIdAsync(int moduloId)
        {
            return await _context.ModulosUsuarios
                .Include(mu => mu.Usuario)
                .Where(mu => mu.ModuloId == moduloId)
                .ToListAsync();
        }

        public async Task<ModulosUsuarios> CreateAsync(ModulosUsuarios moduloUsuario)
        {
            _context.ModulosUsuarios.Add(moduloUsuario);
            await _context.SaveChangesAsync();
            return moduloUsuario;
        }

        public async Task UpdateAsync(ModulosUsuarios moduloUsuario)
        {
            _context.Entry(moduloUsuario).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var moduloUsuario = await GetByIdAsync(id);
            if (moduloUsuario != null)
            {
                _context.ModulosUsuarios.Remove(moduloUsuario);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> UsuarioTienePermisoAsync(int usuarioId, string permiso)
        {
            var moduloUsuario = await GetByUsuarioIdAsync(usuarioId);
            return moduloUsuario?.TienePermiso(permiso) ?? false;
        }
    }
}
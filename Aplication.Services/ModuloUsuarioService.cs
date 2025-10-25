using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data;

namespace Aplication.Services
{
    internal class ModuloUsuarioService
    {
        private readonly AcademiaContext _context;
        private readonly ModulosUsuariosRepository _repository;

        public ModuloUsuarioService()
        {
            _context = new AcademiaContext();
            _repository = new ModulosUsuariosRepository(_context);
        }


        public async Task<List<Domain.Model.ModulosUsuarios>> GetAllModulosUsuariosAsync()
        {
            return await _repository.GetAllAsync();
        }
        public async Task<Domain.Model.ModulosUsuarios?> GetModuloUsuarioByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }
        public async Task<Domain.Model.ModulosUsuarios?> GetModuloUsuarioByUsuarioIdAsync(int usuarioId)
        {
            return await _repository.GetByUsuarioIdAsync(usuarioId);
        }
        public async Task<List<Domain.Model.ModulosUsuarios>> GetModulosUsuariosByModuloIdAsync(int moduloId)
        {
            return await _repository.GetByModuloIdAsync(moduloId);
        }
        public async Task<Domain.Model.ModulosUsuarios> CreateModuloUsuarioAsync(Domain.Model.ModulosUsuarios moduloUsuario)
        {
            return await _repository.CreateAsync(moduloUsuario);
        }
        public async Task UpdateModuloUsuarioAsync(Domain.Model.ModulosUsuarios moduloUsuario)
        {
            await _repository.UpdateAsync(moduloUsuario);
        }
        public async Task DeleteModuloUsuarioAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }

    }
}

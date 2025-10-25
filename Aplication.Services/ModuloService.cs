using Data;
using Domain.Model;
using DTOs;
using Microsoft.EntityFrameworkCore;

namespace Aplication.Services
{
    public class ModuloService
    {
        private readonly AcademiaContext _context;
        private readonly ModuloRepository _repository;

        public ModuloService()
        {
            _context = new AcademiaContext();
            _repository = new ModuloRepository(_context);
        }

        public async Task<IEnumerable<ModuloDto>> GetAllAsync()
        {
            var modulos = await _repository.GetAllAsync();
            return modulos.Select(MapToDto);
        }

        public async Task<ModuloDto?> GetByIdAsync(int id)
        {
            var modulo = await _repository.GetByIdAsync(id);
            return modulo == null ? null : MapToDto(modulo);
        }

        public async Task AddAsync(ModuloDto moduloDto)
        {
            var modulo = new Modulo(moduloDto.Desc_Modulo, moduloDto.Ejecuta);
            await _repository.CreateAsync(modulo);
        }

        public async Task UpdateAsync(ModuloDto moduloDto)
        {
            var modulo = await _repository.GetByIdAsync(moduloDto.Id_Modulo);
            if (modulo == null)
                throw new InvalidOperationException($"Módulo con Id {moduloDto.Id_Modulo} no encontrado");

            modulo.Desc_Modulo = moduloDto.Desc_Modulo;
            modulo.Ejecuta = moduloDto.Ejecuta;
            
            await _repository.UpdateAsync(modulo);
        }

        public async Task DeleteAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }

        private ModuloDto MapToDto(Modulo modulo) => new ModuloDto
        {
            Id_Modulo = modulo.Id_Modulo,
            Desc_Modulo = modulo.Desc_Modulo,
            Ejecuta = modulo.Ejecuta
        };
    }
}
using DTOs;

namespace API.Clients
{
    public interface IAuthApiClient
    {
        Task<LoginResponse> LoginAsync(LoginRequest request);
        Task LogoutAsync();
        Task<bool> IsAuthenticatedAsync();
    }

    public interface IPersonaApiClient
    {
        Task<List<PersonaDto>> GetAllAsync();
        Task<PersonaDto> GetByIdAsync(int id);
        Task<PersonaDto> CreateAsync(PersonaDto persona);
        Task<PersonaDto> UpdateAsync(int id, PersonaDto persona);
        Task DeleteAsync(int id);
    }

    public interface ICursoApiClient
    {
        Task<List<CursoDto>> GetAllAsync();
        Task<CursoDto> GetByIdAsync(int id);
        Task<CursoDto> CreateAsync(CursoDto curso);
        Task<CursoDto> UpdateAsync(int id, CursoDto curso);
        Task DeleteAsync(int id);
    }

    public interface IInscripcionApiClient
    {
        Task<List<AlumnoCursoDto>> GetAllAsync();
        Task<AlumnoCursoDto> GetByIdAsync(int id);
        Task<AlumnoCursoDto> CreateAsync(AlumnoCursoDto inscripcion);
        Task<AlumnoCursoDto> UpdateAsync(int id, AlumnoCursoDto inscripcion);
        Task DeleteAsync(int id);
    }
}
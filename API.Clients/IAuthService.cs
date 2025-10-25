namespace API.Clients
{
    public interface IAuthService
    {
        event Action<bool>? AuthenticationStateChanged;

        Task<bool> IsAuthenticatedAsync();
        Task<string?> GetTokenAsync();
        Task<string?> GetUsernameAsync();
        Task<bool> LoginAsync(string username, string password);
        Task LogoutAsync();
        Task CheckTokenExpirationAsync();
        Task<Dictionary<string, List<string>>> GetModulePermissionsAsync();
        Task<bool> HasModuleAccessAsync(string moduleName);
        Task<bool> HasPermissionAsync(string moduleName, string permission);
    }
}
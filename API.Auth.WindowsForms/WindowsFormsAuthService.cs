using DTOs;
using API.Clients;

namespace API.Auth.WindowsForms
{
    public class WindowsFormsAuthService : IAuthService
    {
        private static string? _currentToken;
        private static DateTime _tokenExpiration;
        private static string? _currentUsername;
        private static int? _currentUserId;
        private static int? _currentPersonaId;
        private static string? _currentTipoUsuario;
        private readonly AuthApiClient _authApiClient;

        public event Action<bool>? AuthenticationStateChanged;

        public WindowsFormsAuthService()
        {
            _authApiClient = new AuthApiClient();
        }

        public async Task<bool> IsAuthenticatedAsync()
        {
            return !string.IsNullOrEmpty(_currentToken) && DateTime.UtcNow < _tokenExpiration;
        }

        public async Task<string?> GetTokenAsync()
        {
            var isAuth = await IsAuthenticatedAsync();
            return isAuth ? _currentToken : null;
        }

        public async Task<string?> GetUsernameAsync()
        {
            var isAuth = await IsAuthenticatedAsync();
            return isAuth ? _currentUsername : null;
        }

        public static int? GetCurrentUserId()
        {
            return _currentUserId;
        }

        public static int? GetCurrentPersonaId()
        {
            return _currentPersonaId;
        }

        public static string? GetCurrentTipoUsuario()
        {
            return _currentTipoUsuario;
        }

        public async Task<bool> LoginAsync(string username, string password)
        {
            var request = new LoginRequest
            {
                Username = username,
                Password = password
            };

            var response = await _authApiClient.LoginAsync(request);

            if (response != null)
            {
                _currentToken = response.Token;
                _tokenExpiration = response.ExpiresAt;
                _currentUsername = response.Username;
                _currentUserId = response.UserId;
                _currentPersonaId = response.PersonaId;
                _currentTipoUsuario = response.TipoUsuario;

                AuthenticationStateChanged?.Invoke(true);
                return true;
            }

            return false;
        }

        public async Task LogoutAsync()
        {
            _currentToken = null;
            _tokenExpiration = default;
            _currentUsername = null;
            _currentUserId = null;
            _currentPersonaId = null;
            _currentTipoUsuario = null;

            AuthenticationStateChanged?.Invoke(false);
        }

        public async Task CheckTokenExpirationAsync()
        {
            if (!string.IsNullOrEmpty(_currentToken) && DateTime.UtcNow >= _tokenExpiration)
            {
                await LogoutAsync();
            }
        }

        public async Task<Dictionary<string, List<string>>> GetModulePermissionsAsync()
        {
            var token = await GetTokenAsync();
            if (string.IsNullOrEmpty(token))
            {
                return new Dictionary<string, List<string>>();
            }

            return await _authApiClient.GetModulePermissionsAsync(token);
        }

        public async Task<bool> HasModuleAccessAsync(string moduleName)
        {
            var permissions = await GetModulePermissionsAsync();
            return permissions.ContainsKey(moduleName);
        }

        public async Task<bool> HasPermissionAsync(string moduleName, string permission)
        {
            var permissions = await GetModulePermissionsAsync();
            if (!permissions.ContainsKey(moduleName))
            {
                return false;
            }

            return permissions[moduleName].Contains(permission);
        }
    }
}
using System.Security.Claims;
using Blazored.LocalStorage;
using BSG.Common.DTO;
using BSG.Common.Helpers;
using BSG.Common.Model;
using BSG.States;
using Microsoft.AspNetCore.Components.Authorization;

namespace BSG.DataServices.Auth;

public interface ICustomAuthState
{
    Task<AuthenticationState> GetAuthenticationStateAsync();
    Task LogInAsync( LoginRequest request );
    Task Logout();
    string GetToken();
    UserDto? GetUser();
}

public class CustomAuthState: AuthenticationStateProvider, ICustomAuthState
{
    private readonly IUserDataService _userDataService;
    private readonly ILocalStorageService _localStorage;
    private readonly IGeneralState _state;
    private readonly ClaimsPrincipal _anonymous = new(new ClaimsIdentity());
    private ClaimsPrincipal _currentUser = new(new ClaimsIdentity());
    private string _token = string.Empty;
    private UserDto? _user;
    private DateTime? _expires;

    public CustomAuthState(IUserDataService userDataService,
        ILocalStorageService localStorage, IGeneralState state)
    {
        _userDataService = userDataService;
        _localStorage = localStorage;
        _state = state;

        SetPropertiesFromLocalStorage();
    }

    public override Task<AuthenticationState> GetAuthenticationStateAsync() =>
        Task.FromResult(new AuthenticationState(_currentUser));

    public Task LogInAsync(LoginRequest request)
    {
        var loginTask = LoginAsyncCore(request);

        NotifyAuthenticationStateChanged(loginTask);

        return loginTask;
    }

    public async Task Logout()
    {
        await _localStorage.ClearAsync();
        _state.Clear();

        _token = string.Empty;
        _expires = null;
        _user = null;
        _currentUser = _anonymous;

        NotifyAuthenticationStateChanged(Task.Run(() => new AuthenticationState(_currentUser)));

        await GetAuthenticationStateAsync();
    }

    public string GetToken() => _token;

    public UserDto? GetUser() => _user;

    public DateTime? GetExpiry() => _expires;

    private async Task<AuthenticationState> LoginAsyncCore(LoginRequest request)
    {
        await LoginWithExternalProviderAsync(request);

        return new AuthenticationState(_currentUser);
    }

    private async Task LoginWithExternalProviderAsync(LoginRequest request)
    {
        var response = await _userDataService.Authenticate(request);
        if (response?.Token == null)
            return;

        var (_, expires) = Utils.DecodeToken(response.Token);

        _user = response.User;
        _expires = expires;
        _token = response.Token;

        await _localStorage.ClearAsync();
        await _localStorage.SetItemAsync("Token", response.Token);
        await _localStorage.SetItemAsync("User", response.User);
        await _localStorage.SetItemAsync("Expires", expires);

        await SetPropertiesFromLocalStorage();
    }

    private async Task SetPropertiesFromLocalStorage()
    {
        _currentUser = new ClaimsPrincipal(new ClaimsIdentity());

        var token = await _localStorage.GetItemAsync<string?>("Token");
        var expires = await _localStorage.GetItemAsync<DateTime?>("Expires");
        if (token == null || expires == null || DateTime.UtcNow > expires)
        {
            await _localStorage.ClearAsync();
            _state.Clear();
            return;
        }

        var user = await _localStorage.GetItemAsync<UserDto?>("User");

        _token = token;
        _expires = expires;
        _user = user;

        var (claims, _) = Utils.DecodeToken(token);

        var roles = claims?
            .Where(r => r.Type == "role")
            .Select(s => s.Value)
            .ToList();

        _state.Set(_expires, _token, _user?.Id, _user?.Username, _user?.FullName, roles);

        var identity = new ClaimsIdentity(claims, "Custom");
        _currentUser = new ClaimsPrincipal(identity);
    }
}
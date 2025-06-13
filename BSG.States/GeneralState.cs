using BSG.Common.DTO;
using BSG.Common.Model;
using BSG.States.Base;

namespace BSG.States;

public interface IGeneralState
{
    DateTime? Expires { get; set; }
    string? Token { get; set; }
    long? UserId { get; set; }
    string? Username { get; set; }
    string? UserFullName { get; set; }
    bool IsAdmin { get; set; }
    List<string>? Roles { get; set; }
    
    List<ComponentDto> Metadata { get; set; }

    void Set(DateTime? expires, string? token, long? userId, string? username, string? userFullName, List<string>? roles);
    void Clear();
}

public class GeneralState: StateBase, IGeneralState
{
    #region Fields & Properties
    
    #region Expires

    private DateTime? _expires;

    public DateTime? Expires
    {
        get => _expires;
        set
        {
            _expires = value;
            OnPropertyChanged();
        }
    }

    #endregion

    #region Token

    private string? _token;

    public string? Token
    {
        get => _token;
        set
        {
            _token = value;
            OnPropertyChanged();
        }
    }

    #endregion

    #region UserId

    private long? _userId;

    public long? UserId
    {
        get => _userId;
        set
        {
            _userId = value;
            OnPropertyChanged();
        }
    }

    #endregion

    #region Username

    private string? _username;

    public string? Username
    {
        get => _username;
        set
        {
            _username = value;
            OnPropertyChanged();
        }
    }

    #endregion

    #region UserFullName

    private string? _userFullName;

    public string? UserFullName
    {
        get => _userFullName;
        set
        {
            _userFullName = value;
            OnPropertyChanged();
        }
    }

    #endregion

    #region Roles

    private List<string>? _roles;

    public List<string>? Roles
    {
        get => _roles;
        set
        {
            _roles = value;
            OnPropertyChanged();
        }
    }

    #endregion

    #region IsAdmin

    private bool _isAdmin;

    public bool IsAdmin
    {
        get => _isAdmin;
        set
        {
            _isAdmin = value;
            NotifyChanges?.Invoke();
        }
    }

    #endregion
    
    #region Metadata
    
    private List<ComponentDto> _metadata = [];
    public List<ComponentDto> Metadata
    {
        get => _metadata;
        set
        {
            _metadata = value;
            OnPropertyChanged();
        }
    }
    
    #endregion
    
    #endregion

    public void Set(DateTime? expires, string? token, long? userId, string? username, string? userFullName, List<string>? roles )
    {
        Expires = expires;
        Token = token;
        UserId = userId;
        Username = username;
        UserFullName = userFullName;
        Roles = roles ?? [];
        IsAdmin = Roles.Contains(Constants.RoleAdmin);
    }

    public void Clear()
    {
        Expires = null;
        Token = null;
        UserId = null;
        Username = null;
        UserFullName = null;
        Roles = null;
        IsAdmin = false;
    }
}
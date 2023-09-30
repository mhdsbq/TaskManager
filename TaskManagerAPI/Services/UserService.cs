using System.Security;
using System.Security.Claims;
using TaskManagerAPI.Data.Providers;
using TaskManagerAPI.Models;

namespace TaskManagerAPI.Services;

public interface IUserService
{
    public int GetAuthorizedUserId();
    public string? GetAuthorizedUsername();
    public User GetAuthenticatedUser();
    public User? GetUserWithUsernameAndPassword(User user);
    public User CreateUser(User user);
    public bool IsExistingUser(string username);
}

public class UserService : IUserService
{
    private readonly IUserProvider _userProvider;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IAuthHelperService _authHelperService;

    public UserService(IUserProvider userProvider, IHttpContextAccessor httpContextAccessor,
        IAuthHelperService authHelperService)
    {
        _userProvider = userProvider ?? throw new ArgumentNullException(nameof(userProvider));
        _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        _authHelperService = authHelperService ?? throw new ArgumentNullException(nameof(authHelperService));
    }


    public int GetAuthorizedUserId()
    {
        return _userProvider.GetUserIdWithUsername(GetAuthorizedUsername());
    }

    public string? GetAuthorizedUsername()
    {
        var username = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (username == null)
        {
            throw new SecurityException("Authentication Error.");
        }

        return username;
    }

    public User GetAuthenticatedUser()
    {
        var user = _userProvider.GetUserWithUsername(GetAuthorizedUsername()).FirstOrDefault();
        if (user == null)
        {
            throw new SecurityException("Authorization Error.");
        }

        return user;
    }

    public User? GetUserWithUsernameAndPassword(User user)
    {
        if (string.IsNullOrWhiteSpace(user.Username))
        {
            throw new ArgumentNullException(nameof(user.Username));
        }

        if (string.IsNullOrWhiteSpace(user.Password))
        {
            throw new ArgumentNullException(nameof(user.Password));
        }

        var existingUser = _userProvider.GetUserWithUsername(user.Username).FirstOrDefault();

        if (existingUser == null || string.IsNullOrWhiteSpace(existingUser.Password))
        {
            return null;
        }

        if (_authHelperService.VerifyPassword(existingUser.Password, user.Password))
        {
            return existingUser;
        }

        return null;
    }

    public User CreateUser(User user)
    {
        if (string.IsNullOrWhiteSpace(user.Username))
        {
            throw new ArgumentNullException(nameof(user.Username));
        }

        if (string.IsNullOrWhiteSpace(user.Password))
        {
            throw new ArgumentNullException(nameof(user.Password));
        }

        if (string.IsNullOrWhiteSpace(user.Email))
        {
            throw new ArgumentNullException(nameof(user.Email));
        }

        user.Password = _authHelperService.HashPassword(user.Password);
        _userProvider.CreateUser(user);

        return user;
    }

    public bool IsExistingUser(string username)
    {
        return _userProvider.IsExistingUser(username);
    }
}
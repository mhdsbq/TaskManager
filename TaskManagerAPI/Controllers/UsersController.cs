using System.ComponentModel.DataAnnotations;
using System.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManagerAPI.Controllers.Contracts;
using TaskManagerAPI.Controllers.Contracts.Converters;
using TaskManagerAPI.Services;

namespace TaskManagerAPI.Controllers;

public interface IUsersController
{
    public string Login(UserLoginContract user);

    public string Signup(UserSignupContract user);
}

[ApiController]
[Route("users")]
public class UsersController : ControllerBase, IUsersController
{
    private readonly ILogger<SessionsController> _logger;
    private readonly IUserService _userService;
    private readonly IAuthHelperService _authHelperService;

    public UsersController(ILogger<SessionsController> logger, IUserService userService, IAuthHelperService authHelperService)
    {
        _logger = logger;
        _userService = userService;
        _authHelperService = authHelperService;
    }

    [Authorize]
    [HttpGet]
    public UserInfo GetUser()
    {
        return _userService.GetAuthenticatedUser().ToUserInfo();
    }

    [AllowAnonymous]
    [HttpPost]
    [Route("login")]
    public string Login([FromBody] UserLoginContract userLogin)
    {
        if (string.IsNullOrWhiteSpace(userLogin.Username))
        {
            throw new ValidationException("Username is required.");
        }

        if (string.IsNullOrWhiteSpace(userLogin.Password))
        {
            throw new ValidationException("Password is required");
        }

        var user = _userService.GetUserWithUsernameAndPassword(userLogin.ToUserModel());
        if (user == null)
        {
            throw new SecurityException("Invalid username or password.");
        }
       
        return _authHelperService.GenerateJwtToken(user);
    }

    [AllowAnonymous]
    [HttpPost]
    [Route("signup")]
    public string Signup([FromBody] UserSignupContract user)
    {
        if (string.IsNullOrWhiteSpace(user.Username))
        {
            throw new ValidationException("Username is required.");
        }

        if (string.IsNullOrWhiteSpace(user.Password))
        {
            throw new ValidationException("Password is required.");
        }

        if (string.IsNullOrWhiteSpace(user.Email))
        {
            throw new ValidationException("Email is required.");
        }
        
        // TODO: Add extra validation.
        if (_userService.IsExistingUser(user.Username))
        {
            throw new ValidationException("User with username already exist");
        }

        _userService.CreateUser(user.ToUserModel());
        return _authHelperService.GenerateJwtToken(user.ToUserModel());
    }
}
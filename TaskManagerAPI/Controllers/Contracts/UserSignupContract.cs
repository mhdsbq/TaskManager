using TaskManagerAPI.Models;

namespace TaskManagerAPI.Controllers.Contracts;

public class UserSignupContract
{
    public UserSignupContract(string username, string email, string password)
    {
        Username = username;
        Email = email;
        Password = password;
    }

    public string Username { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }

    public User ToUserModel()
    {
        return new User()
        {
            Username = Username,
            Email = Email,
            Password = Password
        };
    }
}
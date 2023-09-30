using TaskManagerAPI.Models;

namespace TaskManagerAPI.Controllers.Contracts;

public class UserLoginContract
{
    public UserLoginContract(string username, string password)
    {
        Username = username;
        Password = password;
    }

    public string Username { get; set; }
    public string Password { get; set; }

    public User ToUserModel()
    {
        return new User()
        {
            Username = Username,
            Password = Password
        };
    }
}
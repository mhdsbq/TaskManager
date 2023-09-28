using TaskManagerAPI.Models;

namespace TaskManagerAPI.Controllers.Contracts.Converters;

public static class UserConverter
{
    public static UserInfo ToUserInfo(this User user)
    {
        return new UserInfo()
        {
            Username = user.Username,
            Email = user.Email
        };
    }
}
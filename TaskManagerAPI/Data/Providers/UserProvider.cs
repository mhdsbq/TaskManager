using System.Data.SqlClient;
using Dapper;
using TaskManagerAPI.Models;

namespace TaskManagerAPI.Data.Providers;

public interface IUserProvider
{
    public IEnumerable<User> GetUserWithUsername(string username);
    public void CreateUser(User user);
    public bool IsExistingUser(string username);
}

public class UserProvider:IUserProvider
{
    private readonly string _connectionString;
    public UserProvider(string connectionString)
    {
        _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
    }

    public IEnumerable<User> GetUserWithUsername(string username)
    {
        string sql = "SELECT UserID, Username, Email, Password FROM Users WHERE Username=@Username";
        
        using var connection = new SqlConnection(_connectionString);
        var user = connection.Query<User>(sql, new {Username = username}); 
        return user;
    }

    public void CreateUser(User user)
    {
        string sql = "Insert into Users (Username, Email, Password) values(@Username, @Email, @Password)";

        using var connection = new SqlConnection(_connectionString);
        var result = connection.Execute(sql, new {Username = user.Username, Email=user.Email, Password=user.Password});
    }

    public bool IsExistingUser(string username)
    {
        string sql = "SELECT UserID FROM Users WHERE Username=@Username";

        using var connection = new SqlConnection(_connectionString);
        int id = connection.Query<int>(sql, new { Username = username }).FirstOrDefault();

        return id > 0;
    }
}
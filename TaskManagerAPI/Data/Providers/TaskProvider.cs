using System.Data;
using Dapper;
using Microsoft.Data.Sqlite;
using Task = TaskManagerAPI.Models.Task;

namespace TaskManagerAPI.Data.Providers;

public interface ITaskProvider
{
    public Task GetTask(int taskId);
    public void UpdateTask(Task task);
    public void DeleteTask(int taskId);
    public IEnumerable<Task> GetTasksForUser(int userId);
    public int CreateTaskForUser(Task task, int userId);
}

public class TaskProvider : ITaskProvider
{
    private readonly string _connectionString;

    public TaskProvider(string connectionString)
    {
        _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
    }


    public Task GetTask(int taskId)
    {
        string sql = "SELECT UserId,TaskId,Title,Description,Completed FROM Tasks WHERE TaskId=@TaskId";
        using var connection = new SqliteConnection(_connectionString);
        return connection.Query<Task>(sql, new { TaskId = taskId }).Single();
    }

    public void UpdateTask(Task task)
    {
        string sql = "UPDATE Tasks SET Title = @Title, Description = @Description, Completed = @Completed WHERE TaskId = @TaskId";
        using var connection = new SqliteConnection(_connectionString);
        connection.Execute(sql, task);
    }

    public void DeleteTask(int taskId)
    {
        string sql = "DELETE FROM Tasks WHERE TaskId = @TaskId";
        using var connection = new SqliteConnection(_connectionString);
        connection.Execute(sql, new { TaskId = taskId });
    }

    public IEnumerable<Task> GetTasksForUser(int userId)
    {
        string sql = "SELECT TaskId,Title,Description,Completed FROM Tasks WHERE UserId=@UserId";
        using var connection = new SqliteConnection(_connectionString);
        return connection.Query<Task>(sql, new { UserId = userId });
    }

    public int CreateTaskForUser(Task task, int userId)
    {
        string sql = "INSERT INTO Tasks(Title, Description, Completed, UserId) values(@Title, @Description, @Completed, @UserId)";
        using var connection = new SqliteConnection(_connectionString);
        connection.Execute(sql, new { Title = task.Title, Description = task.Description, Completed = task.Completed, UserId = userId });
        return GetLastInsertRowId(connection);
    }

    private int GetLastInsertRowId(SqliteConnection connection)
    {
        var sql = "SELECT last_insert_rowid();";
        return (int)connection.ExecuteScalar<long>(sql);
    }
}
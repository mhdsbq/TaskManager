using Microsoft.Data.Sqlite;
using System.Security;
using Dapper;

namespace TaskManagerAPI.Data;

public interface IDbInitializer
{
    public void InitializeDb();
}

public class DbInitializer : IDbInitializer
{
    private const string CreateTableFilePath = "Data/Database/Tables";
    private const string DbFilePath = "Data/Database/TaskManager.db";
    private const string SqlExtension = ".sql";
    private readonly SqliteConnection _connection;

    public DbInitializer(string connectionString)
    {
        if (connectionString == null)
        {
            throw new ArgumentNullException(nameof(connectionString));
        }

        _connection = new SqliteConnection(connectionString);
        InitializeDb();
    }

    /// <summary>
    /// Initialize database tables and stored procedures.
    /// </summary>
    public void InitializeDb()
    {
        _connection.Open();
        if (IsTaskManagerDbExisting())
        {
            _connection.Close();
            return;
        }
        // CreateDatabase();
        CreateTables();
        _connection.Close();
    }

    /// <summary>
    /// Create Required database tables using sql files
    /// </summary>
    /// <exception cref="SecurityException"></exception>
    private void CreateTables()
    {
        foreach (var file in Directory.EnumerateFiles(CreateTableFilePath))
        {
            if (!file.EndsWith(SqlExtension))
            {
                throw new SecurityException("A non SQL file should not be allowed");
            }
            ExecuteSqlFile(file);
        }
    }

    /// <summary>
    ///  Execute a non-query sql command from a sql file.
    /// </summary>
    private void ExecuteSqlFile(string filePath)
    {
        FileInfo file = new FileInfo(filePath);
        string script = file.OpenText().ReadToEnd();
        using SqliteCommand command = new SqliteCommand(script, _connection);
        command.ExecuteNonQuery();
    }

    private bool IsTaskManagerDbExisting()
    {
        string sql = "SELECT name FROM sqlite_master WHERE type='table' AND name='Users'";
        var existingTableName = _connection.Query<string>(sql).FirstOrDefault();
        return !string.IsNullOrEmpty(existingTableName);
    }
}
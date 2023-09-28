using System.Data.SqlClient;
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
    private const string CreateDbFilePath = "Data/Database/TaskManagerDB.sql";
    private const string ExistingTaskManagerDBFilePath = "Data/Database/ExistingTaskManagerDB.sql";
    private const string SqlExtension = ".sql";
    private readonly SqlConnection _connection;

    public DbInitializer(IConfiguration configuration)
    {
        if (configuration == null)
        {
            throw new ArgumentNullException(nameof(configuration));
        }

        var connectionString = configuration["ConnectionStrings:AdminConnection"];
        _connection = new SqlConnection(connectionString);
        // TODO: ADD validation for connection
    }

    /// <summary>
    /// Initialize database tables and stored procedures.
    /// </summary>
    public void InitializeDb()
    {
        // TODO: Add connection validation.
        _connection.Open();
        if (IsTaskManagerDbExisting())
        {
            _connection.Close();
            return;
        }
        CreateDatabase();
        CreateTables();
        _connection.Close();
    }

    /// <summary>
    /// Create TaskManagerDB if it doesn't already exist
    /// </summary>
    private void CreateDatabase()
    {
        ExecuteSqlFile(CreateDbFilePath);
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
            UseTaskManagerDb();
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
        using SqlCommand command = new SqlCommand(script, _connection);
        command.ExecuteNonQuery();
    }

    private void UseTaskManagerDb()
    {
        using SqlCommand command = new SqlCommand("USE TaskManagerDB", _connection);
        command.ExecuteNonQuery();
    }

    private bool IsTaskManagerDbExisting()
    {
        FileInfo file = new FileInfo(ExistingTaskManagerDBFilePath);
        string script = file.OpenText().ReadToEnd();
        int taskManagerDbAlreadyExist =  _connection.Query<int>(script).FirstOrDefault();

        return taskManagerDbAlreadyExist > 0;
    }
}
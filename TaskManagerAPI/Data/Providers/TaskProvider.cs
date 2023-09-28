// using System.Data.SqlClient;
// using Task = TaskManagerAPI.Models.Task;
//
// namespace TaskManagerAPI.Data.Providers;
//
// public class TaskProvider
// {
//     private readonly SqlConnection _connection;
//     public TaskProvider(IDb db)
//     {
//         _connection = db.GetDbConnection();
//     }
//
//     public IEnumerable<Task> GetAllTasks()
//     {
//         using SqlCommand command = new SqlCommand("SELECT * FROM TASKS", _connection);
//         _connection.Open();
//         command.ExecuteNonQuery();
//     }
// }
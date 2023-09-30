using System.Data;
using System.Security;
using System.Security.Claims;
using TaskManagerAPI.Data.Providers;
using TaskManagerAPI.Models;
using Task = TaskManagerAPI.Models.Task;

namespace TaskManagerAPI.Services;

public interface ITaskService
{
    public IEnumerable<Task> GetTasksForUser(int userId);
    public Task CreateTaskForUser(Task task, int userId);
    public Task UpdateTaskForUser(Task task, int userId);
    public void DeleteTaskForUser(int taskId, int userId);
}

public class TaskService : ITaskService
{
    private readonly ITaskProvider _taskProvider;

    public TaskService(ITaskProvider taskProvider)
    {
        _taskProvider = taskProvider ?? throw new ArgumentNullException(nameof(taskProvider));
    }

    public IEnumerable<Task> GetTasksForUser(int userId)
    {
        if (userId < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(userId));
        }

        return _taskProvider.GetTasksForUser(userId);
    }

    public Task CreateTaskForUser(Task task, int userid)
    {
        if (userid < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(userid));
        }

        task.Completed = false;
        var taskId = _taskProvider.CreateTaskForUser(task, userid);
        return _taskProvider.GetTask(taskId);
    }

    public Task UpdateTaskForUser(Task task, int userId)
    {
        var existingTask = _taskProvider.GetTask(task.TaskId);
        if (existingTask.UserId != userId)
        {
            throw new UnauthorizedAccessException("Authorization Error.");
        }

        _taskProvider.UpdateTask(task);
        return _taskProvider.GetTask(task.TaskId);
    }

    public void DeleteTaskForUser(int taskId, int userId)
    {
        var existingTask = _taskProvider.GetTask(taskId);
        if ( existingTask.UserId != userId)
        {
            throw new UnauthorizedAccessException("Authorization Error.");
        }
        _taskProvider.DeleteTask(taskId);
    }
}
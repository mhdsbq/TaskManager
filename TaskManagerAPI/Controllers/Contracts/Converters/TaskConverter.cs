using Task = TaskManagerAPI.Models.Task;

namespace TaskManagerAPI.Controllers.Contracts.Converters;

public static class TaskConverter
{
    public static TaskInfo ToTaskInfo(this Task task)
    {
        return new TaskInfo()
        {
            TaskId = task.TaskId,
            Title = task.Title,
            Description = task.Description,
            SubTasks = task.SubTasks,
            Sessions = task.Sessions,
            Completed = task.Completed
        };
    }
}
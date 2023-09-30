using Task = TaskManagerAPI.Models.Task;

namespace TaskManagerAPI.Controllers.Contracts;

public class EditTaskContract
{
    public int TaskId { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public bool? Completed { get; set; }

    public Task ToModel()
    {
        return new Task()
        {
            TaskId = TaskId,
            Title = Title,
            Description = Description,
            Completed = Completed
        };
    }
}
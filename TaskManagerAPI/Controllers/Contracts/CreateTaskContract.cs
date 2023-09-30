using Task = TaskManagerAPI.Models.Task;

namespace TaskManagerAPI.Controllers.Contracts;

public class CreateTaskContract
{
    public CreateTaskContract(string title, string description)
    {
        Title = title;
        Description = description;
    }

    public string Title { get; set; }
    public string Description { get; set; }
    public Task ToModel()
    {
        return new Task()
        {
            Title = Title,
            Description = Description,
            Completed = false
        };
    }
}
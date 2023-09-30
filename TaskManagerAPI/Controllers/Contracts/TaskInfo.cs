using TaskManagerAPI.Models;

namespace TaskManagerAPI.Controllers.Contracts;

public class TaskInfo
{
    public int TaskId { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public IEnumerable<SubTask>? SubTasks { get; set; }
    public IEnumerable<Session>? Sessions { get; set; }
    public bool? Completed { get; set; }
}
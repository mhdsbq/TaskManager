namespace TaskManagerAPI.Models;

public class Task
{
    public int Id { get; set; }
    public string TaskName { get; set; }
    public string Description { get; set; }
    public IEnumerable<SubTask> SubTasks { get; set; }
    public IEnumerable<Session> Sessions { get; set; }
    public bool IsDone { get; set; }
}
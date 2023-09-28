namespace TaskManagerAPI.Models;

public class SubTask
{
    public int Id { get; set; }
    public string SubTaskName { get; set; }
    public IEnumerable<Session> Sessions { get; set; }
    public bool IsdDone { get; set; }
    public int TotalTimeElapsedInSeconds { get; set; }
    public DateTime DateCreated { get; set; }
}
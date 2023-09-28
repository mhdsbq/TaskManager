namespace TaskManagerAPI.Models;

public class Session
{
    public int Id { get; set; }
    public string Description { get; set; }
    public bool IsFinisher { get; set; }
    public DateTime StartTime { get; set; }
    public int ElapsedTimeInSeconds { get; set; }
}
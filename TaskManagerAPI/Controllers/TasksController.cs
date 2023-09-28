using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Task = TaskManagerAPI.Models.Task;

namespace TaskManagerAPI.Controllers;

public interface ITasksController
{
    public IEnumerable<Task> GetTasks();

    public Task CreateTask();

    public Task EditTask();

    public void DeleteTask();
}

[ApiController]
[Authorize]
[Route("task")]
public class TasksController : ControllerBase, ITasksController
{
    private readonly ILogger<TasksController> _logger;

    public TasksController(ILogger<TasksController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    public IEnumerable<Task> GetTasks()
    {
        throw new NotImplementedException();
    }

    [HttpPost]
    public Task CreateTask()
    {
        throw new NotImplementedException();
    }
    
    [HttpPut]
    public Task EditTask()
    {
        throw new NotImplementedException();
    }

    [HttpDelete]
    public void DeleteTask()
    {
        throw new NotImplementedException();
    }
}
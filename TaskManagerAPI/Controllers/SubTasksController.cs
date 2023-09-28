using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManagerAPI.Models;

namespace TaskManagerAPI.Controllers;

public interface ISubTaskController
{
    public SubTask CreateSubTask(int taskId);

    public SubTask EditSubTask();

    public void DeleteSubTask();
}

[ApiController]
[Authorize]
[Route("subtask")]
public class SubTasksController : ControllerBase, ISubTaskController
{
    private readonly ILogger<SubTasksController> _logger;

    public SubTasksController(ILogger<SubTasksController> logger)
    {
        _logger = logger;
    }

    [HttpPost]
    public SubTask CreateSubTask(int taskId)
    {
        throw new NotImplementedException();
    }

    [HttpPut]
    public SubTask EditSubTask()
    {
        throw new NotImplementedException();
    }

    [HttpDelete]
    public void DeleteSubTask()
    {
        throw new NotImplementedException();
    }
}
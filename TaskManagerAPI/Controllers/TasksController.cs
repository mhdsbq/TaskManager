using System.ComponentModel.DataAnnotations;
using System.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManagerAPI.Controllers.Contracts;
using TaskManagerAPI.Services;
using TaskManagerAPI.Controllers.Contracts.Converters;

namespace TaskManagerAPI.Controllers;

public interface ITasksController
{
    public IEnumerable<TaskInfo> GetTasks();

    public TaskInfo CreateTask(CreateTaskContract task);

    public TaskInfo UpdateTask(EditTaskContract task);

    public void DeleteTask(int taskId);
}

[ApiController]
[Authorize]
[Route("task")]
public class TasksController : ControllerBase, ITasksController
{
    private readonly ILogger<TasksController> _logger;
    private readonly IUserService _userService;
    private readonly ITaskService _taskService;

    public TasksController(ILogger<TasksController> logger, IUserService userService, ITaskService taskService)
    {
        _logger = logger;
        _userService = userService ?? throw new ArgumentNullException(nameof(userService));
        _taskService = taskService;
    }

    [HttpGet]
    public IEnumerable<TaskInfo> GetTasks()
    {
        var userId = _userService.GetAuthorizedUserId();
        return _taskService.GetTasksForUser(userId).Select(task => task.ToTaskInfo());
    }

    [HttpPost]
    public TaskInfo CreateTask(CreateTaskContract task)
    {
        var userId = _userService.GetAuthorizedUserId();
        return _taskService.CreateTaskForUser(task.ToModel(), userId).ToTaskInfo();
    }

    [HttpPut]
    public TaskInfo UpdateTask(EditTaskContract task)
    {
        if (task.TaskId <= 0)
        {
            throw new ValidationException("Task id should be a positive integer.");
        }

        var userId = _userService.GetAuthorizedUserId();
        try
        {
            return _taskService.UpdateTaskForUser(task.ToModel(), userId).ToTaskInfo();
        }
        catch (InvalidOperationException ex)
        {
            if (ex.Message == "Sequence contains no elements")
            {
                throw new SecurityException("Task does not exist.");
            }

            throw;
        }
    }

    [HttpDelete]
    public void DeleteTask(int taskId)
    {
        if (taskId <= 0)
        {
            throw new ValidationException("Task id should be a positive integer.");
        }

        var userId = _userService.GetAuthorizedUserId();
        try
        {
            _taskService.DeleteTaskForUser(taskId, userId);
        }
        catch (InvalidOperationException ex)
        {
            if (ex.Message == "Sequence contains no elements")
            {
                throw new SecurityException("Task does not exist.");
            }

            throw;
        }
    }
}
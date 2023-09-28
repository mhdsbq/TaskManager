using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManagerAPI.Models;

namespace TaskManagerAPI.Controllers;

public interface ISessionsController
{
    public Session CreateSession();

    public Session EditSession();

    public void DeleteSession();
}

[ApiController]
[Authorize]
[Route("session")]
public class SessionsController : ControllerBase, ISessionsController
{
    private readonly ILogger<SessionsController> _logger;

    public SessionsController(ILogger<SessionsController> logger)
    {
        _logger = logger;
    }

    [HttpPost]
    public Session CreateSession()
    {
        throw new NotImplementedException();
    }

    [HttpPut]
    public Session EditSession()
    {
        throw new NotImplementedException();
    }

    [HttpDelete]
    public void DeleteSession()
    {
        throw new NotImplementedException();
    }
}
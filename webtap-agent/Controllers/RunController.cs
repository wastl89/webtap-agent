using Microsoft.AspNetCore.Mvc;
using OpenTap;
using webtap_agent.Models;
using webtap_agent.Services;

namespace webtap_agent.Controllers;

[ApiController]
[Route("[controller]")]
public class RunController : ControllerBase
{

    private readonly ILogger<RunController> _logger;
    
    private readonly TestRunService _runController;

    public RunController(ILogger<RunController> logger, TestRunService runController)
    {
        _runController = runController;
        _logger = logger;
    }
    [HttpGet]
    public bool Get()
    {
        return _runController.Running;
    }
    [HttpPost()]
    public IActionResult Post()
    {
        _runController.Run();
        return Ok();
    }
    [HttpPost("stop")]
    public IActionResult Stop()
    {
        _runController.Stop();
        return Ok();
    }
}

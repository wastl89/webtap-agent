using Microsoft.AspNetCore.Mvc;
using webtap_agent.Models;
using webtap_agent.Services;

namespace webtap_agent.Controllers;

[ApiController]
[Route("[controller]")]
public class TestPlanController : ControllerBase
{

    private readonly ILogger<TestPlanController> _logger;
    private readonly TestplanService _testplanService;

    public TestPlanController(ILogger<TestPlanController> logger, TestplanService testplanService)
    {
        _testplanService = testplanService;
        _logger = logger;
    }
    [HttpGet]
    public IEnumerable<string> Get()
    {
        return _testplanService.GetTestplansInFolder();
    }
    [HttpPost("{name}")]
    public IActionResult Post([FromRoute] string name)
    {
        _testplanService.LoadTestPlan(name);
        return Ok();
    }
}

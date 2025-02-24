using OpenTap;
using webtap_agent.Controllers;
using webtap_agent.Exceptions;
using webtap_agent.Models;

namespace webtap_agent.Services;

public class TestRunService(TestplanService testplanService)
{
    private readonly TestplanService _testplanService = testplanService;
    
    private TestPlanRun? _planRun;
    public TestPlanRun? PlanRun => _planRun;
    private bool _running;
    public bool Running => _running;

    private CancellationTokenSource? _cancellationTokenSource = new CancellationTokenSource();


    //Test für WS Logs

    public async void Run() {
        if(Running) return; 
        if(_testplanService.TestPlan == null) throw new Exception("No Plan loaded");
        _running = true;
        var cts = _cancellationTokenSource!.Token;
        await LogController.BroadcastAsync("Testplan starting");
        _planRun = await _testplanService.TestPlan!.ExecuteAsync(cts);
        await LogController.BroadcastAsync("Testplan finished");
        _running = false;
    }

    public void Stop() {
        if(!Running) return;
        _cancellationTokenSource!.Cancel();
    }

    

}
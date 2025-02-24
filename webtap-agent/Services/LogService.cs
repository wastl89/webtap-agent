using OpenTap;
using OpenTap.Diagnostic;
using webtap_agent.Controllers;

namespace webtap_agent.Services;

public class LogService : ILogListener, IHostedService
{
    public async void EventsLogged(IEnumerable<Event> Events)
    {
        foreach (var @event in Events)
        {
            await LogController.BroadcastAsync(@event.ToString());
        }
    }

    public void Flush()
    {
        throw new NotImplementedException();
    }

    public async Task Notify(string message) {
        await LogController.BroadcastAsync(message);
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        Log.AddListener(this);
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        Log.RemoveListener(this);
        return Task.CompletedTask;
    }
}
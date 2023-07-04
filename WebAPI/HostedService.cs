using Library;
using Logic;
using System.Diagnostics;

namespace WebAPI;

public class HostedService : IHostedService
{
    private readonly IWorker _worker;
    private readonly IHelloWorld _helloWorld;

    public HostedService(
        IWorker worker,
        IHelloWorld helloWorld)
    {
        _worker = worker;
        _helloWorld = helloWorld;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        _ = await _worker.DoWorkAsync();
        Debug.WriteLine(_helloWorld.SayHelloWorld());
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}

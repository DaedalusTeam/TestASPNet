using System.Diagnostics;

namespace Logic;
public class Worker : IWorker
{
    public Task<string> DoWorkAsync()
    {
        var result = "WORKER DO WORK";

        Debug.WriteLine(result);
        
        return Task.FromResult(result);
    }
}

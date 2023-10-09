using ContactApp.Contacts;
using ContactApp.Pages.Contacts;
using System.Text.Json;

namespace ContactApp.Utils;

public class Archiver
{
    private readonly Random _random = new();
    private readonly IServiceProvider _serviceProvider;
    private readonly IWebHostEnvironment _env;

    private ArchiveStatus _status = ArchiveStatus.Waiting;
    private int _progress = 0;
    private Task? _task;

    public string Status => _status.ToString();
    public int Progress => _progress;

    public Archiver(IServiceProvider serviceProvider, IWebHostEnvironment env)
    {
        _serviceProvider = serviceProvider;
        _env = env;
    }

    public Task? Run()
    {
        if (_status == ArchiveStatus.Waiting)
        {
            _status = ArchiveStatus.Running;
            _progress = 0;

            _task = Task.Run(RunCore);
        }

        return _task;
    }

    private async Task RunCore()
    {
        for (var i = 0; i < 10; i++)
        {
            await Task.Delay(TimeSpan.FromSeconds(_random.Next(0, 3)));
            if (_status != ArchiveStatus.Running)
                return;

            _progress = (i + 1) * 10;
            Console.WriteLine($"Here... {_progress}");
        }

        await Task.Delay(TimeSpan.FromSeconds(1));

        if (_status != ArchiveStatus.Running)
            return;

        using var scope = _serviceProvider.CreateScope();
        var repository = scope.ServiceProvider.GetService(typeof(ContactRepository)) as ContactRepository
            ?? throw new InvalidOperationException();
        var contacts = repository.All(1);
        var json = JsonSerializer.Serialize(contacts);
        var filePath = Path.Combine(_env.WebRootPath, "contacts.json");
        File.WriteAllText(filePath, json);
    

        _status = ArchiveStatus.Complete;
    }

    public string ArchiveFile()
    {
        return "contacts.json";
    }

    public void Reset() => _status = ArchiveStatus.Waiting;
}

public enum ArchiveStatus
{
    Waiting = 0,
    Running = 1,
    Complete = 2
}

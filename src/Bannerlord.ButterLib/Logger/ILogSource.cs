using Serilog.Sinks.File;

namespace Bannerlord.ButterLib.Logger;

internal interface ILogSource
{
    string Name { get; }
}

internal interface IFileLogSource : ILogSource
{
    string Path { get; }
    IFlushableFileSink[] Sinks { get; }

    void Deconstruct(out string name, out string path, out IFlushableFileSink[] sinks);
}

internal record RollingFileLogSource : IFileLogSource
{
    public string Name { get; init; }
    public string Path { get; init; }
    public IFlushableFileSink[] Sinks { get; init; }

    public RollingFileLogSource(string path, IFlushableFileSink[] sinks)
    {
        var date = System.DateTimeOffset.Now.ToString("yyyyMMdd");
        var directory = System.IO.Path.GetDirectoryName(path);
        var filename = System.IO.Path.GetFileName(path);
        var filenameFixed = filename.Contains("{Date}")
            ? filename.Replace("{Date}", date)
            : $"{System.IO.Path.GetFileNameWithoutExtension(filename)}{date}{System.IO.Path.GetExtension(filename)}";

        Name = System.IO.Path.GetFileNameWithoutExtension(filenameFixed);
        Path = System.IO.Path.Combine(directory!, filenameFixed);
        Sinks = sinks;
    }

    public void Deconstruct(out string name, out string path, out IFlushableFileSink[] sinks)
    {
        name = Name;
        path = Path;
        sinks = Sinks;
    }
}

internal record FileLogSource : IFileLogSource
{
    public string Name { get; init; }
    public string Path { get; init; }
    public IFlushableFileSink[] Sinks { get; init; }

    public FileLogSource(string path, IFlushableFileSink[] sinks)
    {
        Name = System.IO.Path.GetFileNameWithoutExtension(path);
        Path = path;
        Sinks = sinks;
    }

    public void Deconstruct(out string name, out string path, out IFlushableFileSink[] sinks)
    {
        name = Name;
        path = Path;
        sinks = Sinks;
    }
}
using System.Diagnostics;

namespace ThFnsc.RemoteControl;

public class ServiceManager(string serviceName, string startCommand)
{
    public void InstallService()
    {
        Execute("sc", true, false, "stop", serviceName);
        Execute("sc", true, false, "delete", serviceName);
        Execute("sc", false, true, "create", serviceName, $"binPath={startCommand}", "start=auto");
        Execute("sc", false, true, "failure", serviceName, "reset=", "86400", "actions=", "restart/60000/restart/60000//1000");
        Execute("sc", false, true, "start", serviceName);
    }

    public void UninstallService()
    {
        Execute("sc", true, true, "stop", serviceName);
        Execute("sc", false, true, "delete", serviceName);
    }

    private static void Execute(string command, bool allowedToFail, bool redirectOutput, params string?[] arguments)
    {
        Console.WriteLine($" > {command} {string.Join(' ', arguments)}");
        var processInfo = new ProcessStartInfo
        {
            FileName = command,
            CreateNoWindow = true,
            UseShellExecute = false,
            RedirectStandardError = redirectOutput,
            RedirectStandardOutput = redirectOutput
        };
        foreach (var argument in arguments)
            if (argument != null)
                processInfo.ArgumentList.Add(argument);

        var process = Process.Start(processInfo);

        if (process is null)
            throw new InvalidOperationException("Process did not initialize");

        if (redirectOutput)
        {
            process.EnableRaisingEvents = true;
            process.OutputDataReceived += (_, data) => Console.WriteLine(data.Data);
            process.ErrorDataReceived += (_, data) => Console.WriteLine(data.Data);
            process.BeginOutputReadLine();
            process.BeginErrorReadLine();
        }

        process.WaitForExit();
        if (process.ExitCode != 0 && !allowedToFail)
            Environment.Exit(process.ExitCode);
    }
}

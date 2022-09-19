using System.Diagnostics;

namespace ThFnsc.RemoteControl;

public class ServiceManager
{
	private readonly string _serviceName;
	private readonly string _startCommand;

	public ServiceManager(string serviceName, string startCommand)
	{
		_serviceName = serviceName;
		_startCommand = startCommand;
	}

    public void InstallService()
    {
        Execute("sc", true, false, "stop", _serviceName);
        Execute("sc", true, false, "delete", _serviceName);
        Execute("sc", true, true, "create", _serviceName, $"binPath={_startCommand}", "start=auto");
        Execute("sc", true, true, "failure", _serviceName, "reset=", "86400", "actions=", "restart/60000/restart/60000//1000");
        Execute("sc", true, true, "start", _serviceName);
    }

    public void UninstallService()
    {
        Execute("sc", true, true, "stop", _serviceName);
        Execute("sc", false, true, "delete", _serviceName);
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

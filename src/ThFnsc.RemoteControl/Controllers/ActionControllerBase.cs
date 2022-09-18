using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Text;

namespace ThFnsc.RemoteControl.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize]
public abstract class ActionControllerBase : ControllerBase
{
    protected IServiceProvider Services => HttpContext.RequestServices;

    protected Task<IActionResult> ExecuteProcessAsync(string fileName, params string?[] arguments) =>
        ExecuteProcessAsync(fileName, arguments.AsEnumerable());

    protected async Task<IActionResult> ExecuteProcessAsync(string fileName, IEnumerable<string?> arguments)
    {
        var processInfo = new ProcessStartInfo
        {
            FileName = fileName,
            CreateNoWindow = true,
            UseShellExecute = false,
            RedirectStandardError = true,
            RedirectStandardOutput = true
        };
        foreach (var argument in arguments)
            if (argument != null)
                processInfo.ArgumentList.Add(argument);

        StringBuilder stdout = new(), stderr = new();
        var logger = Services.GetRequiredService<ILoggerFactory>().CreateLogger(GetType());
        using var loggerScope = logger.BeginScope("Process: {File} {Arguments}", fileName, string.Join(' ', arguments));

        logger.LogInformation("Starting");
        var process = Process.Start(processInfo);
        if (process is null)
            throw new InvalidOperationException("Process did not initialize");
        process.EnableRaisingEvents = true;
        process.OutputDataReceived += (_, data) =>
        {
            if (data.Data != null)
                stdout.AppendLine(data.Data);
        };
        process.ErrorDataReceived += (_, data) =>
        {
            if (data.Data != null)
                stderr.AppendLine(data.Data);
        };
        process.BeginOutputReadLine();
        process.BeginErrorReadLine();
        await process.WaitForExitAsync();

        if (stdout.Length > 0)
            logger.LogInformation("STDOUT: {stdout}", stdout.ToString());
        if (stderr.Length > 0)
            logger.LogError("STDERR: {stderr}", stderr.ToString());

        logger.Log(process.ExitCode == 0 ? LogLevel.Information : LogLevel.Error, "Exited with code {ExitCode}", process.ExitCode);

        return process.ExitCode == 0
            ? Ok()
            : Problem(
                detail: $"Process finished with a non-zero exit code: {process.ExitCode}",
                statusCode: 500);
    }
}

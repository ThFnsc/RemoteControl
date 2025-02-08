using Microsoft.AspNetCore.Mvc;

namespace ThFnsc.RemoteControl.Util;

public static class PowerUtils
{
    public static Task<IResult> ShutdownAsync([FromServices] ILogger<RemoteControlService> logger, [FromQuery] bool hybrid, [FromQuery] int seconds) =>
       ProcessUtils.ExecuteProcessAsync(
           fileName: "shutdown",
           logger,
           "/s",
           "/d", "p:0:0",
           "/t", seconds.ToString(),
           "/c", "Remote shutdown requested",
           hybrid ? "/hybrid" : null);

    public static Task<IResult> LockAsync([FromServices] ILogger<RemoteControlService> logger)
    {
        try
        {
            ProcessExtensions.StartProcessAsCurrentUser("Rundll32.exe", "Rundll32.exe user32.dll,LockWorkStation"); //As service
            return Task.FromResult(Results.Ok());
        }
        catch
        {
            return ProcessUtils.ExecuteProcessAsync("Rundll32.exe", logger, "user32.dll,LockWorkStation"); //User process fallback
        }
    }

    public static Task<IResult> AbortAsync([FromServices] ILogger<RemoteControlService> logger) =>
        ProcessUtils.ExecuteProcessAsync("shutdown", logger, "/a");

    public static Task<IResult> RebootAsync([FromServices] ILogger<RemoteControlService> logger) =>
        ProcessUtils.ExecuteProcessAsync("shutdown", logger, "/r");

    public static Task<IResult> LogoffAsync([FromServices] ILogger<RemoteControlService> logger) =>
        ProcessUtils.ExecuteProcessAsync("shutdown", logger, "/l");

    public static Task<IResult> SleepAsync([FromServices] ILogger<RemoteControlService> logger) =>
        ProcessUtils.ExecuteProcessAsync("rundll32.exe", logger, "powrprof.dll,SetSuspendState", "0,1,0");
}
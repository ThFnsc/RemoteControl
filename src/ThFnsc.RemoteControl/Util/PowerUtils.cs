using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;

namespace ThFnsc.RemoteControl.Util;

public static class PowerUtils
{
    public static Task<IResult> ShutdownAsync(
        [FromServices] ILogger<RemoteControlService> logger,
        [FromQuery] bool hybrid = true,
        [FromQuery] int seconds = 30,
        [FromQuery] string message = "Remote shutdown requested") =>
       SystemProcessUtils.ExecuteProcessAsync(
           fileName: "shutdown",
           logger,
           "/s",
           "/d", "p:0:0",
           "/t", seconds.ToString(),
           "/c", message,
           hybrid ? "/hybrid" : null);

    public static Task<IResult> LockAsync([FromServices] ILogger<RemoteControlService> logger)
    {
        try
        {
            UserProcessUtils.StartProcessAsCurrentUser("Rundll32.exe", "Rundll32.exe user32.dll,LockWorkStation"); //As service
            return Task.FromResult(Results.Ok());
        }
        catch
        {
            return SystemProcessUtils.ExecuteProcessAsync("Rundll32.exe", logger, "user32.dll,LockWorkStation"); //User process fallback
        }
    }

    public static Task<IResult> AbortAsync([FromServices] ILogger<RemoteControlService> logger) =>
        SystemProcessUtils.ExecuteProcessAsync("shutdown", logger, "/a");

    public static Task<IResult> RebootAsync([FromServices] ILogger<RemoteControlService> logger) =>
        SystemProcessUtils.ExecuteProcessAsync("shutdown", logger, "/r");

    public static Task<IResult> LogoffAsync([FromServices] ILogger<RemoteControlService> logger) =>
        SystemProcessUtils.ExecuteProcessAsync("shutdown", logger, "/l");

    public static Task<IResult> SleepAsync([FromServices] ILogger<RemoteControlService> logger) =>
        SystemProcessUtils.ExecuteProcessAsync("rundll32.exe", logger, "powrprof.dll,SetSuspendState", "0,1,0");
}
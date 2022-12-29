using Microsoft.AspNetCore.Mvc;
using murrayju.ProcessExtensions;

namespace ThFnsc.RemoteControl.Controllers;

public class PowerController : ActionControllerBase
{
    [HttpGet("Shutdown")]
    public Task<IActionResult> ShutdownAsync(bool hybrid = true, int seconds = 30) =>
        ExecuteProcessAsync(
            fileName: "shutdown",
            "/s",
            "/d", "p:0:0",
            "/t", seconds.ToString(),
            "/c", "Remote shutdown requested",
            hybrid ? "/hybrid" : null);

    [HttpGet("Abort")]
    public Task<IActionResult> AbortAsync() =>
        ExecuteProcessAsync("shutdown", "/a");

    [HttpGet("Reboot")]
    public Task<IActionResult> RebootAsync() =>
        ExecuteProcessAsync("shutdown", "/r");

    [HttpGet("Logoff")]
    public Task<IActionResult> LogoffAsync() =>
        ExecuteProcessAsync("shutdown", "/l");

    [HttpGet("Sleep")]
    public Task<IActionResult> SleepAsync() =>
        ExecuteProcessAsync("rundll32.exe", "powrprof.dll,SetSuspendState", "0,1,0");

    [HttpGet("Lock")]
    public IActionResult LockAsync()
    {
        try
        {
            ProcessExtensions.StartProcessAsCurrentUser("Rundll32.exe", "Rundll32.exe user32.dll,LockWorkStation"); //As service
        }
        catch
        {
            ExecuteProcessAsync("Rundll32.exe", "user32.dll,LockWorkStation"); //User process fallback
        }
        return Ok();
    }
}

using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;

namespace ThFnsc.RemoteControl.AuthHandlers.AntiBruteforceWrapper;

public class AntiBruteforceWrapper<THandler>(THandler handler, IOptionsMonitor<AntiBruteforceOptions> options) : IAuthenticationHandler where THandler : IAuthenticationHandler
{
    private static SemaphoreSlim? _semaphore;
    private static int _currentMax = -1;

    public async Task<AuthenticateResult> AuthenticateAsync()
    {
        var currentOptions = options.CurrentValue;
        if(_semaphore == null || _currentMax != currentOptions.Concurrent)
        {
            _semaphore?.Dispose();
            _semaphore = new(_currentMax = currentOptions.Concurrent);
        }

        var result = await handler.AuthenticateAsync();
        await _semaphore.WaitAsync();
        if (result is { Succeeded: false, Failure: not null })
            await Task.Delay(currentOptions.Timeout);
        _semaphore.Release();
        return result;
    }

    public Task ChallengeAsync(AuthenticationProperties? properties) => handler.ChallengeAsync(properties);

    public Task ForbidAsync(AuthenticationProperties? properties) => handler.ForbidAsync(properties);

    public Task InitializeAsync(AuthenticationScheme scheme, HttpContext context) => handler.InitializeAsync(scheme, context);
}

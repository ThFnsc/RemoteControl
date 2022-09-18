using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;

namespace ThFnsc.RemoteControl.AuthHandlers.AntiBruteforceWrapper;

public class AntiBruteforceWrapper<THandler> : IAuthenticationHandler where THandler : IAuthenticationHandler
{
    private static SemaphoreSlim? _semaphore;
    private static int _currentMax = -1;

    private readonly THandler _handler;
    private readonly IOptionsMonitor<AntiBruteforceOptions> _options;

    public AntiBruteforceWrapper(THandler handler, IOptionsMonitor<AntiBruteforceOptions> options)
    {
        _handler = handler;
        _options = options;
    }

    public async Task<AuthenticateResult> AuthenticateAsync()
    {
        var options = _options.CurrentValue;
        if(_semaphore == null || _currentMax != options.Concurrent)
        {
            _semaphore?.Dispose();
            _semaphore = new(_currentMax = options.Concurrent);
        }

        var result = await _handler.AuthenticateAsync();
        await _semaphore.WaitAsync();
        if (result is { Succeeded: false, Failure: not null })
            await Task.Delay(options.Timeout);
        _semaphore.Release();
        return result;
    }

    public Task ChallengeAsync(AuthenticationProperties? properties) => _handler.ChallengeAsync(properties);

    public Task ForbidAsync(AuthenticationProperties? properties) => _handler.ForbidAsync(properties);

    public Task InitializeAsync(AuthenticationScheme scheme, HttpContext context) => _handler.InitializeAsync(scheme, context);
}

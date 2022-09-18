using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;

namespace ThFnsc.RemoteControl.AuthHandlers.AntiBruteforceWrapper;

public class AntiBruteforceWrapper<THandler> : IAuthenticationHandler where THandler : IAuthenticationHandler
{
    private static SemaphoreSlim? _semaphore;
    private readonly THandler _handler;
    private readonly IOptions<AntiBruteforceOptions> _options;

    public AntiBruteforceWrapper(THandler handler, IOptions<AntiBruteforceOptions> options)
    {
        _handler = handler;
        _options = options;
        _semaphore ??= new SemaphoreSlim(options.Value.Concurrent);
    }

    public async Task<AuthenticateResult> AuthenticateAsync()
    {
        var result = await _handler.AuthenticateAsync();
        await _semaphore!.WaitAsync();
        if (result is { Succeeded: false, Failure: not null })
            await Task.Delay(_options.Value.Timeout);
        _semaphore.Release();
        return result;
    }

    public Task ChallengeAsync(AuthenticationProperties? properties) => _handler.ChallengeAsync(properties);

    public Task ForbidAsync(AuthenticationProperties? properties) => _handler.ForbidAsync(properties);

    public Task InitializeAsync(AuthenticationScheme scheme, HttpContext context) => _handler.InitializeAsync(scheme, context);
}

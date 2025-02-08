using Microsoft.AspNetCore.RateLimiting;
using System.Threading.RateLimiting;

namespace ThFnsc.RemoteControl.Configurations;

public static class RateLimitConfigs
{
    public const string DefaultPolicy = nameof(DefaultPolicy);

    public static WebApplicationBuilder AddRateLimiting(this WebApplicationBuilder builder)
    {
        builder.Services.AddRateLimiter(conf =>
        {
            conf.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
            conf.AddSlidingWindowLimiter(DefaultPolicy, opt =>
            {
                opt.SegmentsPerWindow = 1;
                builder.Configuration.GetRequiredSection(nameof(SlidingWindowRateLimiterOptions)).Bind(opt);
            });
        });

        return builder;
    }
}
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging.Console;
using ThFnsc.RemoteControl.AuthHandlers.AntiBruteforceWrapper;
using ThFnsc.RemoteControl.AuthHandlers.QueryString;

var builder = WebApplication.CreateBuilder(args);

builder.Services.RemoveAll<ConsoleLoggerProvider>();
builder.Logging
    .AddSimpleConsole(options =>
    {
        options.TimestampFormat = "\n\n[dd/MM/yy HH:mm:ss 'UTC'] ";
        options.UseUtcTimestamp = true;
        options.IncludeScopes = true;
    });

builder.Host.UseWindowsService(options =>
    options.ServiceName = typeof(Program).Namespace);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

builder.Services.AddAuthentication(QueryStringAuthenticationDefaults.AuthenticationScheme)
    .UseAntiBruteforce(
        builderAction: withABF => withABF
            .AddQueryString(
                authenticationScheme: null,
                configure: builder.Configuration.GetSection(nameof(QueryStringAuthenticationOptions)).Bind),
        configureOptions: builder.Configuration.GetSection(nameof(AntiBruteforceOptions)).Bind);

builder.Services.AddHttpLogging(options => 
    options.LoggingFields = HttpLoggingFields.RequestPath | HttpLoggingFields.RequestMethod | HttpLoggingFields.ResponseStatusCode | HttpLoggingFields.RequestQuery);

var app = builder.Build();

app.UseHttpLogging();

app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

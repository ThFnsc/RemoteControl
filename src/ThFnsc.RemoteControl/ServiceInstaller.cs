using DasMulli.Win32.ServiceUtils;
using Microsoft.Extensions.Hosting.WindowsServices;
using System.ComponentModel;
using System.Reflection;

namespace ThFnsc.RemoteControl;

public class ServiceInstaller
{
    private readonly ILogger<ServiceInstaller> _logger;
    private readonly IWebHostEnvironment _env;
    private readonly Win32ServiceManager _manager;
    private readonly ServiceDefinition _definition;

    public ServiceInstaller(ILogger<ServiceInstaller> logger, IWebHostEnvironment env)
    {
        _logger = logger;
        _env = env;
        _manager = new Win32ServiceManager();
        _definition = new ServiceDefinition(Assembly.GetExecutingAssembly().GetName().Name, Environment.ProcessPath)
        {
            Credentials = Win32ServiceCredentials.LocalSystem,
            AutoStart = true
        };
    }

    public void Run()
    {
        var uninstall = Environment.GetCommandLineArgs()
            .Any(a => a is "-u" or "--uninstall");

        if (uninstall)
        {
            WrapCommand(() => _manager.DeleteService(_definition.ServiceName), "Application uninstalled! This window can now be closed.");
            return;
        }

        var isRunningAsService = WindowsServiceHelpers.IsWindowsService();

        if (isRunningAsService || !_env.IsProduction())
            return;

        try { _manager.DeleteService(_definition.ServiceName); }
        catch { }

        WrapCommand(() => _manager.CreateOrUpdateService(_definition, startImmediately: true), "Application installed as a service! This window can now be closed.");
    }

    private void WrapCommand(Action action, string success)
    {
        try
        {
            action();
            _logger.LogInformation(success);
        }
        catch (Win32Exception e) when (e.Message is "Access is denied.")
        {
            _logger.LogError("Execute again as administrator ");
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Unexpected error");
        }
        finally
        {
            Thread.Sleep(TimeSpan.FromHours(1));
            Environment.Exit(0);
        }
    }
}

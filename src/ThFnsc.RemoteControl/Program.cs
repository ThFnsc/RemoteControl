using System.CommandLine;
using ThFnsc.RemoteControl;

var startCommand = new Command("start", "Starts the service without installing");
var catchAllArgument = new Argument<string[]>("arguments", "arguments that will be passed directly to the WebApplication")
{
    Arity = ArgumentArity.ZeroOrMore
};

startCommand.SetHandler(RemoteControlService.StartServiceAsync, catchAllArgument);
startCommand.AddArgument(catchAllArgument);

var service = new ServiceManager(typeof(Program).Assembly.GetName().Name!, $"{Environment.ProcessPath} start");

var installCommand = new Command("install", "Installs and starts the service");
installCommand.SetHandler(service.InstallService);

var uninstallCommand = new Command("uninstall", "Stops and uninstalls the service");
uninstallCommand.SetHandler(service.UninstallService);

var root = new RootCommand
{
    startCommand,
    installCommand,
    uninstallCommand
};

return root.Invoke(args);

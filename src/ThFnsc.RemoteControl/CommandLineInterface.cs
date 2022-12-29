using System.CommandLine;

namespace ThFnsc.RemoteControl;

public class CommandLineInterface
{
    private static readonly ServiceManager _service = new(typeof(Program).Assembly.GetName().Name!, $"{Environment.ProcessPath} start");

    public static Task<int> Parse(string[] args)
    {
        var startCommand = new Command("start", "Starts the service without installing");
        var catchAllArgument = new Argument<string[]>("arguments", "arguments that will be passed directly to the WebApplication")
        {
            Arity = ArgumentArity.ZeroOrMore
        };
        startCommand.SetHandler(RemoteControlService.StartServiceAsync, catchAllArgument);
        startCommand.AddArgument(catchAllArgument);


        var installCommand = new Command("install", "Installs and starts the service");
        installCommand.SetHandler(_service.InstallService);


        var uninstallCommand = new Command("uninstall", "Stops and uninstalls the service");
        uninstallCommand.SetHandler(_service.UninstallService);


        var root = new RootCommand()
        {
            startCommand,
            installCommand,
            uninstallCommand
        };

        return root.InvokeAsync(args);
    }
}

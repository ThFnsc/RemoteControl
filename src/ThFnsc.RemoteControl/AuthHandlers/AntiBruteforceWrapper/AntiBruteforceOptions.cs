namespace ThFnsc.RemoteControl.AuthHandlers.AntiBruteforceWrapper;

public class AntiBruteforceOptions
{
    public int Concurrent { get; set; } = 3;

    public TimeSpan Timeout { get; set; } = TimeSpan.FromSeconds(10);
}

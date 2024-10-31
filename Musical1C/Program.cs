using ConsoleView;

namespace Musical1C;

public class Program
{
    public static async Task Main(string[] args)
    {
        var startView = new StartView();
        await startView.RunAsync();

    }
}
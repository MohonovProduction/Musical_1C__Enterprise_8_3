using System.Diagnostics.CodeAnalysis;
using ConsoleView;

namespace Musical1C;

[ExcludeFromCodeCoverage]
public class Program
{
    public static async Task Main(string[] args)
    {
        var startView = new StartView();
        await startView.RunAsync();

    }
}
namespace ConsoleView;

public class Formatter
{
    public static string InputString(string text)
    {
        return $"[bold] < {text}[/]";
    }

    public static string OutputString(string text)
    {
        return $"[bold] > {text}[/]";
    }

    public static string Heading(string text)
    {
        return $"[bold] > {text} <[/]";
    }
}
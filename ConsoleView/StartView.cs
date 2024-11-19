using System.Diagnostics.CodeAnalysis;

namespace ConsoleView;

public class StartView
{
    [ExcludeFromCodeCoverage]
    public async Task RunAsync()
    {
        Console.WriteLine("Добро пожаловать в систему 1С-Дирижер");
        var menuActions = new Dictionary<int, Func<Task>>()
        {
            { 1, async () => await FindMusician() },
            { 2, async () => await NewConcert() },
            { 3, async () => await GetHistoryAsync() },
            { 4, () => Task.Run(() => Environment.Exit(0)) }
        };

        var menuLabels = new Dictionary<int, string>()
        {
            { 1, "🔎 Поиск музыканта" },
            { 2, "📝 Планирование концерта" },
            { 3, "📜 Просмотр истории" },
            { 4, "🚪 Выход" }
        };

        var menuView = new Menu(menuActions, menuLabels);
        await menuView.ExecuteMenuChoice();
    }

    private async Task GetHistoryAsync()
    {
        var getHistoryView = new GetHistoryView();
        await getHistoryView.GetHistoryAsync();
    }

    private async Task FindMusician()
    {
        var findMusicianView = new FindMusicianView();
        await findMusicianView.FindMusician();
    }

    private async Task NewConcert()
    {
        var addConcertView = new AddConcertView();
        await addConcertView.NewConcert();
    }
}
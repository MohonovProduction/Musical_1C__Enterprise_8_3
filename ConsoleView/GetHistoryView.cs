using Presenter;

namespace ConsoleView;

public class GetHistoryView
{
    private readonly ConcertPresenter _concertPresenter = new ConcertPresenter();
    private readonly StartView _StartView = new StartView();
    
    public async Task GetHistoryAsync()
    {
        CancellationToken token = new CancellationToken();
        Console.WriteLine("История концертов:");
        var concerts = await _concertPresenter.GetConcertsAsync(token);
        foreach (var concert in concerts)
        {
            Console.WriteLine($"Название: {concert.Name} Дата: {concert.Date} Тип: {concert.Type}");
        }

        var menuActions = new Dictionary<int, Func<Task>>()
        {
            { 1, async () => await _StartView.RunAsync() },
        };

        var menuLabels = new Dictionary<int, string>()
        {
            { 1, "Назад" },
        };

        var menuView = new Menu(menuActions, menuLabels);
        await menuView.ExecuteMenuChoice();
    }
}
using Presenter;
using Storage;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleView;

public class StartView
{
    private readonly ConcertPresenter _concertPresenter = new ConcertPresenter();
    private readonly MusicianPresenter _musicianPresenter = new MusicianPresenter();
    private readonly MusicPresenter _musicPresenter = new MusicPresenter();
    private readonly InstrumentPresenter _instrumentPresenter = new InstrumentPresenter();

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
        CancellationToken token = new CancellationToken();
        Console.WriteLine("История концертов:");
        var concerts = await _concertPresenter.GetConcertsAsync(token);
        foreach (var concert in concerts)
        {
            Console.WriteLine($"Название: {concert.Name} Дата: {concert.Date} Тип: {concert.Type}");
        }

        var menuActions = new Dictionary<int, Func<Task>>()
        {
            { 1, async () => await RunAsync() },
        };

        var menuLabels = new Dictionary<int, string>()
        {
            { 1, "Назад" },
        };

        var menuView = new Menu(menuActions, menuLabels);
        await menuView.ExecuteMenuChoice();
    }

    private async Task FindMusician()
    {
        Console.Write("Введите имя музыканта для поиска: ");
        var name = Console.ReadLine();
        var token = new CancellationToken();
        var musicians = await _musicianPresenter.GetMusiciansAsync(token);
        var foundMusicians = musicians.Where(m => m.Name.Contains(name, StringComparison.OrdinalIgnoreCase)).ToList();

        if (foundMusicians.Any())
        {
            Console.WriteLine("Найденные музыканты:");
            foreach (var musician in foundMusicians)
            {
                Console.WriteLine(
                    $"Имя: {musician.Name} {musician.LastName}, Инструменты: {string.Join(", ", musician.Instruments.Select(i => i.Name))}");
            }
        }
        else
        {
            Console.WriteLine("Музыканты с таким именем не найдены.");
        }

        var menuActions = new Dictionary<int, Func<Task>>()
        {
            { 1, async () => await RunAsync() },
        };

        var menuLabels = new Dictionary<int, string>()
        {
            { 1, "Назад" },
        };

        var menuView = new Menu(menuActions, menuLabels);
        await menuView.ExecuteMenuChoice();
    }

    private async Task NewConcert()
    {
        var view = new AddConcertView();

        await view.NewConcert();
    }
}
using Presenter;

namespace ConsoleView;

public class FindMusicianView
{
    private readonly MusicianPresenter _musicianPresenter = new MusicianPresenter();
    private readonly StartView _startView = new StartView();
    
    public async Task FindMusician()
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
            { 1, async () => await _startView.RunAsync() },
        };

        var menuLabels = new Dictionary<int, string>()
        {
            { 1, "Назад" },
        };

        var menuView = new Menu(menuActions, menuLabels);
        await menuView.ExecuteMenuChoice();
    }
}
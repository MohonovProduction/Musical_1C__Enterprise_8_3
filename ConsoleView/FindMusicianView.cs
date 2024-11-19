using Presenter;

namespace ConsoleView;

public class FindMusicianView
{   
    private readonly MusicianInstrumentPresenter _musicianInstrumentPresenter = new MusicianInstrumentPresenter();
    private readonly InstrumentPresenter _instrumentPresenter = new InstrumentPresenter();
    private readonly MusicianPresenter _musicianPresenter = new MusicianPresenter();
    private readonly StartView _startView = new StartView();
    
    public async Task FindMusician()
    {
        Console.Write("Введите имя музыканта для поиска: ");
        var name = Console.ReadLine();
        var token = new CancellationToken();

        // Получение списка музыкантов
        var musicians = await _musicianPresenter.GetMusiciansAsync(token);
        var foundMusicians = musicians.Where(m => m.Name.Contains(name, StringComparison.OrdinalIgnoreCase)).ToList();

        // Получение инструментов музыкантов
        var musiciansInstruments = await _musicianInstrumentPresenter.GetMusicianInstrumentAsync(token);
    
        // Создание множества идентификаторов найденных музыкантов
        var foundMusicianIds = new HashSet<Guid>(foundMusicians.Select(m => m.Id));

        // Получение всех инструментов
        var instruments = await _instrumentPresenter.GetInstrumentsAsync(token);

        // Фильтрация инструментов по найденным музыкантам
        var foundInstruments = musiciansInstruments
            .Where(e => foundMusicianIds.Contains(e.MusicianId))
            .ToList();

        if (foundMusicians.Any())
        {
            Console.WriteLine("Найденные музыканты:");
            foreach (var musician in foundMusicians)
            {
                // Получение имен инструментов, которые использует текущий музыкант
                var musicianInstruments = instruments
                    .Where(l => foundInstruments.Any(j => l.Id == j.InstrumentId && j.MusicianId == musician.Id))
                    .Select(l => l.Name)
                    .ToList();

                Console.WriteLine($"Имя: {musician.Name} {musician.LastName}, Инструменты: {string.Join(", ", musicianInstruments)}");
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
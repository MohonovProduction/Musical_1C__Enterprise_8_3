using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using System.Collections.Generic;
using Presenter;
using Spectre.Console;
using Storage;

public class StartView
{
    private readonly MusicianPresenter _musicianPresenter;
    private readonly MusicianInstrumentPresenter _musicianInstrumentPresenter;
    private readonly InstrumentPresenter _instrumentPresenter;
    private readonly ConcertPresenter _concertPresenter;
    private readonly SoundPresenter _soundPresenter;

    // Конструктор, где передаются зависимости через DI
    public StartView(
        ConcertPresenter concertPresenter, 
        MusicianPresenter musicianPresenter, 
        MusicianInstrumentPresenter musicianInstrumentPresenter, 
        InstrumentPresenter instrumentPresenter, SoundPresenter soundPresenter)
    {
        _concertPresenter = concertPresenter ?? throw new ArgumentNullException(nameof(concertPresenter));
        _musicianPresenter = musicianPresenter ?? throw new ArgumentNullException(nameof(musicianPresenter));
        _musicianInstrumentPresenter = musicianInstrumentPresenter ?? throw new ArgumentNullException(nameof(musicianInstrumentPresenter));
        _instrumentPresenter = instrumentPresenter ?? throw new ArgumentNullException(nameof(instrumentPresenter));
        _soundPresenter = soundPresenter ?? throw new ArgumentNullException(nameof(soundPresenter));
    }

    [ExcludeFromCodeCoverage]
    public async Task RunAsync()
    {
        AnsiConsole.MarkupLine("[black on lightgoldenrod2 bold] > Добро пожаловать в систему [darkred]1С-Дирижер[/] < [/]");
        
        var menuActions = new Dictionary<int, Func<Task>>()
        {
            { 1, async () => await FindMusicianAsync() },
            { 2, async () => await NewConcertAsync() },
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

    public async Task GetHistoryAsync()
    {
        var token = new CancellationToken();
        AnsiConsole.MarkupLine("[bold] > История концертов: [/]");
            
        // Получаем список концертов
        var concerts = await _concertPresenter.GetConcertsAsync(token);
            
        if (concerts.Any())
        {
            foreach (var concert in concerts)
            {
                Console.WriteLine($"Название: {concert.Name} | Дата: {concert.Date} | Тип: {concert.Type}");
            }
        }
        else
        {
            AnsiConsole.MarkupLine("[bold] > Нет данных о концертах.[/]");
        }

        // Меню для возврата
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

    public async Task FindMusicianAsync()
        {
            AnsiConsole.MarkupLine("[bold] < Введите имя музыканта для поиска:[/]");
            var name = Console.ReadLine();
            var token = new CancellationToken();

            var musicians = await _musicianPresenter.GetMusiciansAsync(token);
            var foundMusicians = musicians.Where(m => m.Name.Contains(name, StringComparison.OrdinalIgnoreCase)).ToList();

            if (!foundMusicians.Any())
            {
                AnsiConsole.MarkupLine("[bold] > Музыканты с таким именем не найдены.[/]");
                await ShowMenuAsync();
                return;
            }

            // Получаем инструменты для найденных музыкантов
            var musiciansInstruments = await GetMusiciansInstrumentsAsync(foundMusicians, token);
            var instruments = await _instrumentPresenter.GetInstrumentsAsync(token);

            // Выводим информацию о музыкантах и их инструментах
            AnsiConsole.MarkupLine("[bold] > Найденные музыканты:[/]");
            foreach (var musician in foundMusicians)
            {
                var musicianInstruments = GetMusicianInstruments(musician, musiciansInstruments, instruments);
                Console.WriteLine($" · Имя: {musician.Name} {musician.Lastname} / Инструменты: {string.Join(", ", musicianInstruments)}");
            }

            // Меню для перехода назад
            await ShowMenuAsync();
        }

        // Получаем инструменты для найденных музыкантов
        private async Task<IReadOnlyCollection<MusicianInstrument>> GetMusiciansInstrumentsAsync(List<Musician> foundMusicians, CancellationToken token)
        {
            var musicianIds = foundMusicians.Select(m => m.Id).ToList();
            var allMusicianInstruments = await _musicianInstrumentPresenter.GetMusicianInstrumentAsync(token);
            return allMusicianInstruments.Where(m => musicianIds.Contains(m.MusicianId)).ToList();
        }

        // Получаем имена инструментов для конкретного музыканта
        private List<string> GetMusicianInstruments(Musician musician, IReadOnlyCollection<MusicianInstrument> musicianInstruments, IReadOnlyCollection<Instrument> instruments)
        {
            var musicianInstrumentIds = musicianInstruments.Where(m => m.MusicianId == musician.Id).Select(m => m.InstrumentId).ToList();
            return instruments.Where(i => musicianInstrumentIds.Contains(i.Id)).Select(i => i.Name).ToList();
        }

        // Метод для отображения меню
        private async Task ShowMenuAsync()
        {
            var menuActions = new Dictionary<int, Func<Task>>()
            {
                { 1, async () => await RunAsync() }
            };

            var menuLabels = new Dictionary<int, string>()
            {
                { 1, "Назад" }
            };

            var menuView = new Menu(menuActions, menuLabels);
            await menuView.ExecuteMenuChoice();
        }

    public async Task NewConcertAsync()
        {
            var menuActions = new Dictionary<int, Func<Task>>()
            {
                { 1, async () => await SelectTypeConcertAsync() },
                { 2, async () => await AddMusicToConcertAsync() },
                { 3, async () => await AddMusiciansToConcertAsync() },
                { 4, async () => await AddDateToConcertAsync() },
                { 5, async () => await SaveConcertAsync() },
                { 6, () => Task.Run(() => RunAsync()) },
            };

            var menuLabels = new Dictionary<int, string>()
            {
                { 1, "Выбрать тип концерта" },
                { 2, "Добавить произведения" },
                { 3, "Добавить музыкантов" },
                { 4, "Добавить дату" },
                { 5, "Сохранить концерт" },
                { 6, "Назад" }
            };

            var menuView = new Menu(menuActions, menuLabels);
            await menuView.ExecuteMenuChoice();
        }

        private async Task SelectTypeConcertAsync()
        {
            Console.WriteLine("Выберите тип концерта:");
            Console.WriteLine("1. Групповой");
            Console.WriteLine("2. Общий");
            Console.Write("\nВведите номер: ");

            if (int.TryParse(Console.ReadLine(), out int type) && (type == 1 || type == 2))
            {
                var concertType = type == 1 ? "Групповой" : "Общий";
                _concertPresenter.SetConcertType(concertType);
                Console.WriteLine("Тип концерта успешно выбран.");
                await NewConcertAsync();
            }
            else
            {
                Console.WriteLine("Некорректный выбор. Попробуйте снова.");
                await SelectTypeConcertAsync();
            }
        }

        private async Task AddMusicToConcertAsync()
        {
            Console.WriteLine("Выберите действие:");
            Console.WriteLine("1. Добавить существующее произведение");
            Console.WriteLine("2. Добавить новое произведение");

            if (Console.ReadLine() == "1")
            {
                await AddExistingMusicToConcertAsync();
            }
            else
            {
                await AddNewMusicAndAddToConcertAsync();
            }
            await NewConcertAsync();
        }

        private async Task AddExistingMusicToConcertAsync()
        {
            var token = new CancellationToken();
            var musicList = await _soundPresenter.GetMusicAsync(token);

            if (musicList.Any())
            {
                Console.WriteLine("Доступные произведения:");
                for (var index = 0; index < musicList.Count; index++)
                {
                    var music = musicList.ElementAt(index);
                    Console.WriteLine($"{index + 1}. {music.Name} - {music.Author}");
                }

                Console.Write("\nВведите номер произведения для добавления в концерт: ");
                if (int.TryParse(Console.ReadLine(), out int selectedIndex) && selectedIndex >= 1 && selectedIndex <= musicList.Count)
                {
                    _concertPresenter.AddMusicToConcert(musicList.ElementAt(selectedIndex - 1));
                    Console.WriteLine("Произведение добавлено в концерт.");
                }
                else
                {
                    Console.WriteLine("Некорректный выбор.");
                }
            }
            else
            {
                Console.WriteLine("Произведений не найдено. Добавьте новое.");
                await AddNewMusicAndAddToConcertAsync();
            }
        }

        private async Task AddNewMusicAndAddToConcertAsync()
        {
            var token = new CancellationToken();
            Console.Write("Введите название произведения: ");
            var name = Console.ReadLine();

            Console.Write("Введите автора произведения: ");
            var author = Console.ReadLine();
            
            var newMusic = await _soundPresenter.AddMusicAsync(name, author, token); // Сохраняем в систему
            _concertPresenter.AddMusicToConcert(newMusic); // Добавляем в концерт
            Console.WriteLine("Новое произведение добавлено.");
        }

        private async Task AddMusiciansToConcertAsync()
        {
            Console.WriteLine("Выберите действие:");
            Console.WriteLine("1. Добавить существующего музыканта");
            Console.WriteLine("2. Добавить нового музыканта");

            if (Console.ReadLine() == "1")
            {
                await AddExistingMusicianToConcertAsync();
            }
            else
            {
                await AddNewMusicianAndAddToConcertAsync();
            }
            await NewConcertAsync();
        }

        private async Task AddExistingMusicianToConcertAsync()
        {
            var token = new CancellationToken();
            var musicians = await _musicianPresenter.GetMusiciansAsync(token);

            if (musicians.Any())
            {
                Console.WriteLine("Список музыкантов:");
                for (var index = 0; index < musicians.Count; index++)
                {
                    var musician = musicians.ElementAt(index);
                    Console.WriteLine($"{index + 1}. {musician.Name} {musician.Lastname} {musician.Surname}");
                }

                Console.Write("\nВведите номер музыканта для добавления в концерт: ");
                if (int.TryParse(Console.ReadLine(), out int selectedIndex) && selectedIndex >= 1 && selectedIndex <= musicians.Count)
                {
                    _concertPresenter.AddMusicianToConcert(musicians.ElementAt(selectedIndex - 1));
                    Console.WriteLine("Музыкант добавлен в концерт.");
                }
                else
                {
                    Console.WriteLine("Некорректный выбор.");
                }
            }
            else
            {
                Console.WriteLine("Нет доступных музыкантов для добавления.");
            }
        }

        private async Task AddNewMusicianAndAddToConcertAsync()
        {
            var token = new CancellationToken();

            Console.Write("Введите имя музыканта: ");
            var name = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(name))
            {
                Console.WriteLine("Имя музыканта не может быть пустым.");
                return;
            }

            Console.Write("Введите фамилию музыканта: ");
            var lastName = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(lastName))
            {
                Console.WriteLine("Фамилия музыканта не может быть пустой.");
                return;
            }

            Console.Write("Введите отчество музыканта: ");
            var surname = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(surname))
            {
                Console.WriteLine("Отчество музыканта не может быть пустым.");
                return;
            }

            Console.WriteLine("Введите инструменты (через запятую): ");
            var instrumentNames = Console.ReadLine()?.Split(",").Select(i => i.Trim()).ToList();
            if (instrumentNames == null || instrumentNames.Count == 0)
            {
                Console.WriteLine("Список инструментов не может быть пустым.");
                return;
            }

            var instruments = instrumentNames.Select(i => new Instrument(Guid.NewGuid(), i)).ToList();

            try
            {
                var newMusician = await _musicianPresenter.AddMusicianAsync(name, lastName, surname, instruments, token);
                _concertPresenter.AddMusicianToConcert(newMusician);
                Console.WriteLine("Новый музыкант добавлен.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Произошла ошибка: {ex.Message}");
            }
        }

        private async Task AddDateToConcertAsync()
        {
            Console.Write("Введите дату концерта (формат: ГГГГ-ММ-ДД): ");
            if (DateTime.TryParse(Console.ReadLine(), out var concertDate))
            {
                _concertPresenter.SetConcertDate(concertDate.ToString("yyyy-MM-dd"));
                Console.WriteLine("Дата концерта успешно добавлена.");
            }
            else
            {
                Console.WriteLine("Некорректный формат даты.");
            }
            await NewConcertAsync();
        }

        private async Task SaveConcertAsync()
        {
            CancellationToken token = new CancellationToken();
            var concInf = await _concertPresenter.GetConcertBuilderAsync();
            Console.WriteLine("Выбранные произведения:");
            foreach (var concInfMusic in concInf.Music)
            {
                Console.WriteLine(concInfMusic.Name);
            }
            Console.WriteLine("Выбранные музыканты:");
            foreach (var concInfMusicians in concInf.Musicians)
            {
                Console.WriteLine(concInfMusicians.Name + " " + concInfMusicians.Lastname);
            }
            Console.Write("Введите название концерта: ");
            var concertName = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(concertName))
            {
                await _concertPresenter.AddConcertAsync(concertName, token);
                Console.WriteLine("Концерт успешно сохранен.");
            }
            else
            {
                Console.WriteLine("Название концерта не может быть пустым.");
            }

            await RunAsync();
        }

        private string FormatInputString(string text)
        {
            return $"[bold] > {text}[/]";
        }

        private string FormatOutputString(string text)
        {
            return $"[bold] < {text}[/]";
        }
}

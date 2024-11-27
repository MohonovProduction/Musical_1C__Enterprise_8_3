using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using System.Collections.Generic;
using ConsoleView;
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
        AnsiConsole.MarkupLine(Formatter.Heading("История концертов"));
        
        // Получаем список концертов
        var concerts = await _concertPresenter.GetConcertsAsync(token);
        
        if (concerts.Any())
        {
            // Создаем таблицу
            var table = new Table();
            table.AddColumn("Название");
            table.AddColumn("Дата");
            table.AddColumn("Тип");

            // Добавляем строки в таблицу для каждого концерта
            foreach (var concert in concerts)
            {
                table.AddRow(concert.Name, concert.Date, concert.Type);
            }

            // Отображаем таблицу
            AnsiConsole.Write(table);
        }
        else
        {
            AnsiConsole.MarkupLine(Formatter.OutputString("Нет данных о концертах"));
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
        AnsiConsole.MarkupLine(Formatter.InputString("Введите имя музыканта для поиска:"));
        var name = Console.ReadLine();
        var token = new CancellationToken();

        var musicians = await _musicianPresenter.GetMusiciansAsync(token);
        var foundMusicians = musicians.Where(m => m.Name.Contains(name, StringComparison.OrdinalIgnoreCase)).ToList();

        if (!foundMusicians.Any())
        {
            AnsiConsole.MarkupLine(Formatter.OutputString("Музыканты с таким именем не найдены"));
            await ShowMenuAsync();
            return;
        }

        // Получаем инструменты для найденных музыкантов
        var musiciansInstruments = await GetMusiciansInstrumentsAsync(foundMusicians, token);
        var instruments = await _instrumentPresenter.GetInstrumentsAsync(token);

        // Создаем таблицу для отображения информации о музыкантах и их инструментах
        var table = new Table();
        table.AddColumn("Имя");
        table.AddColumn("Фамилия");
        table.AddColumn("Инструменты");

        // Заполняем таблицу данными о найденных музыкантах
        foreach (var musician in foundMusicians)
        {
            var musicianInstruments = GetMusicianInstruments(musician, musiciansInstruments, instruments);
            table.AddRow(musician.Name, musician.Lastname, string.Join(", ", musicianInstruments));
        }

        // Выводим таблицу
        AnsiConsole.Write(table);

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
            AnsiConsole.MarkupLine(Formatter.InputString("Выберите тип концерта:"));
            AnsiConsole.MarkupLine("1. Групповой");
            AnsiConsole.MarkupLine("2. Общий");
            AnsiConsole.MarkupLine("\nВведите номер: ");

            if (int.TryParse(Console.ReadLine(), out int type) && (type == 1 || type == 2))
            {
                var concertType = type == 1 ? "Групповой" : "Общий";
                _concertPresenter.SetConcertType(concertType);
                AnsiConsole.MarkupLine(Formatter.OutputString("Тип концерта успешно выбран"));
                await NewConcertAsync();
            }
            else
            {
                AnsiConsole.MarkupLine(Formatter.OutputString("Некорректный выбор. Попробуйте снова"));
                await SelectTypeConcertAsync();
            }
        }

        private async Task AddMusicToConcertAsync()
        {
            AnsiConsole.MarkupLine(Formatter.InputString("Выберите действие:"));
            AnsiConsole.MarkupLine("[bold]1. Добавить существующее произведение[/]");
            AnsiConsole.MarkupLine("[bold]2. Добавить новое произведение[/]");

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
                AnsiConsole.MarkupLine(Formatter.Heading("Доступные произведения"));
                for (var index = 0; index < musicList.Count; index++)
                {
                    var music = musicList.ElementAt(index);
                    AnsiConsole.MarkupLine($"[bold]{index + 1}. {music.Name} - {music.Author}[/]");
                }

                AnsiConsole.MarkupLine(Formatter.InputString("Введите номер произведения для добавления в концерт:"));
                if (int.TryParse(Console.ReadLine(), out int selectedIndex) && selectedIndex >= 1 && selectedIndex <= musicList.Count)
                {
                    _concertPresenter.AddMusicToConcert(musicList.ElementAt(selectedIndex - 1));
                    AnsiConsole.MarkupLine(Formatter.OutputString("Произведение добавлено в концерт"));
                }
                else
                {
                    AnsiConsole.MarkupLine(Formatter.OutputString("Некорректный выбор"));
                }
            }
            else
            {
                AnsiConsole.MarkupLine("Произведений не найдено. Добавьте новое.");
                await AddNewMusicAndAddToConcertAsync();
            }
        }

        private async Task AddNewMusicAndAddToConcertAsync()
        {
            var token = new CancellationToken();
            AnsiConsole.MarkupLine(Formatter.InputString("Введите название произведения:"));
            var name = Console.ReadLine();

            AnsiConsole.MarkupLine(Formatter.InputString("Введите автора произведения:"));
            var author = Console.ReadLine();

            var newMusic = await _soundPresenter.AddMusicAsync(name, author, token); // Сохраняем в систему
            _concertPresenter.AddMusicToConcert(newMusic); // Добавляем в концерт
            AnsiConsole.MarkupLine(Formatter.OutputString("Новое произведение добавлено."));
        }

        private async Task AddMusiciansToConcertAsync()
        {
            AnsiConsole.MarkupLine(Formatter.InputString("Выберите действие:"));
            AnsiConsole.MarkupLine("1. Добавить существующего музыканта");
            AnsiConsole.MarkupLine("2. Добавить нового музыканта");

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
                AnsiConsole.MarkupLine(Formatter.Heading("Список музыкантов:"));
                for (var index = 0; index < musicians.Count; index++)
                {
                    var musician = musicians.ElementAt(index);
                    AnsiConsole.MarkupLine($"{index + 1}. {musician.Name} {musician.Lastname} {musician.Surname}");
                }

                AnsiConsole.MarkupLine(Formatter.InputString("\nВведите номер музыканта для добавления в концерт:"));
                if (int.TryParse(Console.ReadLine(), out int selectedIndex) && selectedIndex >= 1 && selectedIndex <= musicians.Count)
                {
                    _concertPresenter.AddMusicianToConcert(musicians.ElementAt(selectedIndex - 1));
                    AnsiConsole.MarkupLine(Formatter.OutputString("Музыкант добавлен в концерт."));
                }
                else
                {
                    AnsiConsole.MarkupLine(Formatter.OutputString("Некорректный выбор."));
                }
            }
            else
            {
                AnsiConsole.MarkupLine(Formatter.OutputString("Нет доступных музыкантов для добавления."));
            }
        }

        private async Task AddNewMusicianAndAddToConcertAsync()
        {
            var token = new CancellationToken();

            AnsiConsole.MarkupLine(Formatter.InputString("Введите имя музыканта: "));
            var name = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(name))
            {
                AnsiConsole.MarkupLine(Formatter.OutputString("Имя музыканта не может быть пустым."));
                return;
            }

            AnsiConsole.MarkupLine(Formatter.InputString("Введите фамилию музыканта: "));
            var lastName = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(lastName))
            {
                AnsiConsole.MarkupLine(Formatter.OutputString("Фамилия музыканта не может быть пустой."));
                return;
            }

            AnsiConsole.MarkupLine(Formatter.InputString("Введите отчество музыканта: "));
            var surname = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(surname))
            {
                AnsiConsole.MarkupLine(Formatter.OutputString("Отчество музыканта не может быть пустым."));
                return;
            }

            AnsiConsole.MarkupLine(Formatter.InputString("Введите инструменты (через запятую): "));
            var instrumentNames = Console.ReadLine()?.Split(",").Select(i => i.Trim()).ToList();
            if (instrumentNames == null || instrumentNames.Count == 0)
            {
                AnsiConsole.MarkupLine(Formatter.OutputString("Список инструментов не может быть пустым."));
                return;
            }

            var instruments = instrumentNames.Select(i => new Instrument(Guid.NewGuid(), i)).ToList();

            try
            {
                var newMusician = await _musicianPresenter.AddMusicianAsync(name, lastName, surname, instruments, token);
                _concertPresenter.AddMusicianToConcert(newMusician);
                AnsiConsole.MarkupLine(Formatter.OutputString("Новый музыкант добавлен."));
            }
            catch (Exception ex)
            {
                AnsiConsole.MarkupLine(Formatter.OutputString($"Произошла ошибка: {ex.Message}"));
            }
        }


        private async Task AddDateToConcertAsync()
        {
            DateTime concertDate;
            bool validDate = false;

            while (!validDate)
            {
                AnsiConsole.MarkupLine(Formatter.InputString("Введите дату концерта (формат: ГГГГ-ММ-ДД): "));
                var input = Console.ReadLine();

                if (DateTime.TryParse(input, out concertDate))
                {
                    _concertPresenter.SetConcertDate(concertDate.ToString("yyyy-MM-dd"));
                    AnsiConsole.MarkupLine(Formatter.OutputString("Дата концерта успешно добавлена."));
                    validDate = true;  // Выход из цикла, если дата введена правильно
                }
                else
                {
                    AnsiConsole.MarkupLine(Formatter.OutputString("Некорректный формат даты. Пожалуйста, попробуйте снова."));
                }
            }

            await NewConcertAsync();
        }


        private async Task SaveConcertAsync()
        {
            CancellationToken token = new CancellationToken();
            var concInf = await _concertPresenter.GetConcertBuilderAsync();

            AnsiConsole.MarkupLine(Formatter.Heading("Выбранные произведения:"));
            foreach (var concInfMusic in concInf.Music)
            {
                AnsiConsole.MarkupLine($" · {concInfMusic.Name}");
            }

            AnsiConsole.MarkupLine(Formatter.Heading("Выбранные музыканты:"));
            foreach (var concInfMusicians in concInf.Musicians)
            {
                AnsiConsole.MarkupLine($" · {concInfMusicians.Name} {concInfMusicians.Lastname}");
            }

            AnsiConsole.MarkupLine(Formatter.InputString("Введите название концерта:"));
            var concertName = Console.ReadLine();

            if (!string.IsNullOrWhiteSpace(concertName))
            {
                var res = await _concertPresenter.AddConcertAsync(concertName, token);

                if (res)
                {
                    AnsiConsole.MarkupLine(Formatter.OutputString("Концерт успешно сохранен."));  
                    await RunAsync();
                }
                else
                {
                    AnsiConsole.MarkupLine(Formatter.OutputString("Не все поля концерта заполнены."));
                    await NewConcertAsync();
                }

                
            }
            else
            {
                AnsiConsole.MarkupLine(Formatter.OutputString("Название концерта не может быть пустым."));
            }
        }

}

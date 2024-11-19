using System.Diagnostics.CodeAnalysis;
using Presenter;
using Storage;

namespace ConsoleView;

[ExcludeFromCodeCoverage]
public class AddConcertView
{
    private readonly ConcertPresenter _concertPresenter = new ConcertPresenter();
    private readonly MusicianPresenter _musicianPresenter = new MusicianPresenter();
    private readonly MusicPresenter _musicPresenter = new MusicPresenter();
    private readonly StartView _StartView = new StartView();
    
    public async Task NewConcert()
    {
        var menuActions = new Dictionary<int, Func<Task>>()
        {
            { 1, async () => await SelectTypeConcertAsync() },
            { 2, async () => await AddMusicToConcertAsync() },
            { 3, async () => await AddMusiciansToConcertAsync() },
            { 4, async () => await AddDateToConcertAsync() },
            { 5, async () => await SaveConcertAsync() },
            { 6, async () => await _StartView.RunAsync() },
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
            await NewConcert();
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
        await NewConcert();
    }

    private async Task AddExistingMusicToConcertAsync()
    {
        var token = new CancellationToken();
        var musicList = await _musicPresenter.GetMusicAsync(token);

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
        
       var newMusic = await _musicPresenter.AddMusicAsync( name, author, token); // Сохраняем в систему
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
        await NewConcert();
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
                Console.WriteLine($"{index + 1}. {musician.Name} {musician.LastName} {musician.Surname}");
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

        Console.Write("Введите фамилию музыканта: ");
        var lastName = Console.ReadLine();

        Console.Write("Введите отчество музыканта: ");
        var surname = Console.ReadLine();

        Console.WriteLine("Введите инструменты (через запятую): ");
        var instrumentNames = Console.ReadLine()?.Split(",").Select(i => i.Trim()).ToList();

        var instruments = instrumentNames?.Select(i => new Instrument(Guid.NewGuid(),i)).ToList() ?? new List<Instrument>();
        var newMusician = await _musicianPresenter.AddMusicianAsync(name, lastName, surname, instruments, token);
        
        _concertPresenter.AddMusicianToConcert(newMusician);
        Console.WriteLine("Новый музыкант добавлен.");
    }

    private async Task AddDateToConcertAsync()
    {
        Console.Write("Введите дату концерта (формат: ГГГГ-ММ-ДД): ");
        if (DateTime.TryParse(Console.ReadLine(), out var concertDate))
        {
            _concertPresenter.SetConcertDate(concertDate.ToString());
            Console.WriteLine("Дата концерта успешно добавлена.");
        }
        else
        {
            Console.WriteLine("Некорректный формат даты.");
        }
        await NewConcert();
    }

    private async Task SaveConcertAsync()
    {
        CancellationToken token = new CancellationToken();
        var concInf =await _concertPresenter.GetConcertBuilderAsync();
        Console.WriteLine("Выбранные произведения");
        foreach (var concInfMusic in concInf.Music)
        {
            Console.WriteLine(concInfMusic.Name);
        }
        Console.WriteLine("Выбранные музыканты");
        foreach (var concInfMusicians in concInf.Musicians)
        {
            Console.WriteLine(concInfMusicians.Name + " " + concInfMusicians.LastName);
        }
        Console.Write("Введите название концерта: ");
        var concertName = Console.ReadLine();
        if (!string.IsNullOrWhiteSpace(concertName))
        {

            await _concertPresenter.AddConcertAsync(concertName,token);
            Console.WriteLine("Концерт успешно сохранен.");
        }
        else
        {
            Console.WriteLine("Название концерта не может быть пустым.");
        }

        await _StartView.RunAsync();
    }
}
//CONNECTION STRING: Host=localhost;Port=5432;Username=postgres;Password=1111;Database=musical1c

using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Presenter;
using Storage;

namespace Musical1C
{
    [ExcludeFromCodeCoverage]
    public class Program
    {
        public static async Task Main(string[] args)
        {
            // Настройка зависимостей и DI контейнера
            var serviceProvider = new ServiceCollection()
                .AddDbContext<ApplicationDbContext>(options => 
                    options.UseNpgsql("Host=localhost;Port=5432;Username=postgres;Password=1111;Database=musical1c"))
                // Регистрация для ConcertStorage и других зависимостей
                .AddScoped<IConcertStorage, ConcertStorage>()
                .AddScoped<IInstrumentStorage, InstrumentStorage>()
                .AddScoped<IMusicianInstrumentStorage, MusicianInstrumentStorage>()
                .AddScoped<IMusicianOnConcertStorage, MusicianOnConcertStorage>()
                .AddScoped<IMusicianStorage, MusicianStorage>()
                .AddScoped<ISoundOnConcertStorage, SoundOnConcertStorage>()
                .AddScoped<ISoundStorage, SoundStorage>()

                // Регистрация презентеров
                .AddScoped<ConcertPresenter>()
                .AddScoped<InstrumentPresenter>()
                .AddScoped<MusicianInstrumentPresenter>()
                .AddScoped<MusicianOnConcertPresenter>()
                .AddScoped<MusicianPresenter>()
                .AddScoped<SoundOnConcertPresenter>()
                .AddScoped<SoundPresenter>()

                // Регистрация представлений
                .AddScoped<StartView>()
                
                .BuildServiceProvider();

            // Получаем StartView из контейнера зависимостей и запускаем его
            var startView = serviceProvider.GetRequiredService<StartView>();
            await startView.RunAsync();
        }
    }
}
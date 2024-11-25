using System;
using System.Diagnostics.CodeAnalysis;
using ConsoleView;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
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
                    options.UseNpgsql("Host=localhost;Port=5432;Username=postgres;Password=1111;Database=musical1c")) // Замените на ваш строку подключения
                .AddScoped<IConcertStorage, ConcertStorage>()
                .AddScoped<StartView>()
                .BuildServiceProvider();

            // Получаем StartView из контейнера зависимостей и запускаем его
            var startView = serviceProvider.GetRequiredService<StartView>();
            await startView.RunAsync();
        }
    }
}
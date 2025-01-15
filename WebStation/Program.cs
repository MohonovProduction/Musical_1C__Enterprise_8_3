using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Presenter;
using Storage;

namespace WebStation
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.ConfigureServices((context, services) =>
                    {
                        // Add database context and other dependencies
                        services.AddDbContext<ApplicationDbContext>(options =>
                             options.UseNpgsql(context.Configuration.GetConnectionString("DatabaseConnection")));

                        services.AddDbContextFactory<ApplicationDbContext>(options =>
                            options.UseNpgsql(context.Configuration.GetConnectionString("DatabaseConnection")));
                        
                        // Storages
                        services.AddSingleton<IConcertStorage, ConcertStorage>();
                        services.AddSingleton<IInstrumentStorage, InstrumentStorage>();
                        services.AddSingleton<ISoundStorage, SoundStorage>();
                        services.AddSingleton<IMusicianInstrumentStorage, MusicianInstrumentStorage>();
                        services.AddSingleton<IMusicianOnConcertStorage, MusicianOnConcertStorage>();
                        services.AddSingleton<IMusicianStorage, MusicianStorage>();
                        services.AddSingleton<ISoundOnConcertStorage, SoundOnConcertStorage>();

                        // Presenters
                        services.AddSingleton<IInstrumentPresenter, InstrumentPresenter>();
                        services.AddSingleton<IMusicianInstrumentPresenter, MusicianInstrumentPresenter>();
                        services.AddSingleton<IMusicianOnConcertPresenter, MusicianOnConcertPresenter>();
                        services.AddSingleton<IMusicianPresenter, MusicianPresenter>();
                        services.AddSingleton<ISoundOnConcertPresenter, SoundOnConcertPresenter>();
                        services.AddSingleton<ISoundPresenter, SoundPresenter>();
                        services.AddSingleton<IConcertPresenter, ConcertPresenter>();

                        // Controllers
                        services.AddControllers()
                            .AddJsonOptions(options =>
                            {
                                // Configure JSON serialization to handle cyclic references
                                options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
                                options.JsonSerializerOptions.MaxDepth = 64; // Optionally, you can set a max depth to prevent stack overflow
                            });

                        // Add Swagger
                        services.AddSwaggerGen(c =>
                        {
                            c.SwaggerDoc("v1", new OpenApiInfo
                            {
                                Title = "Musical 1C API",
                                Version = "v1",
                                Description = "API for managing concerts, musicians, and sounds."
                            });
                        });
                    });

                    webBuilder.Configure(app =>
                    {
                        var env = app.ApplicationServices.GetRequiredService<IHostEnvironment>();

                        // Enable middleware to serve generated Swagger as a JSON endpoint.
                        app.UseSwagger();

                        // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.).
                        app.UseSwaggerUI(c =>
                        {
                            c.SwaggerEndpoint("/swagger/v1/swagger.json", "Musical 1C API v1");
                            c.RoutePrefix = string.Empty; // Set Swagger UI at the root (optional)
                        });

                        if (env.IsDevelopment())
                        {
                            app.UseDeveloperExceptionPage();
                        }

                        app.UseRouting();
                        app.UseEndpoints(endpoints =>
                        {
                            endpoints.MapControllers();
                        });
                    });
                });
    }
}

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Storage;
using Presenter;
using Swashbuckle.AspNetCore.Swagger;

namespace WebAPI
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
                    webBuilder.ConfigureServices(services =>
                    {
                        // Add database context and other dependencies
                        services.AddDbContext<ApplicationDbContext>(options =>
                            options.UseNpgsql("Host=localhost;Port=5432;Username=postgres;Password=1111;Database=musical1c"));

                        // Add your storages
                        services.AddScoped<IConcertStorage, ConcertStorage>();
                        services.AddScoped<IInstrumentStorage, InstrumentStorage>();
                        services.AddScoped<IMusicianInstrumentStorage, MusicianInstrumentStorage>();
                        services.AddScoped<IMusicianOnConcertStorage, MusicianOnConcertStorage>();
                        services.AddScoped<IMusicianStorage, MusicianStorage>();
                        services.AddScoped<ISoundOnConcertStorage, SoundOnConcertStorage>();
                        services.AddScoped<ISoundStorage, SoundStorage>();

                        // Register presenters
                        services.AddScoped<IInstrumentPresenter, InstrumentPresenter>();
                        services.AddScoped<IMusicianInstrumentPresenter, MusicianInstrumentPresenter>();
                        services.AddScoped<IMusicianOnConcertPresenter, MusicianOnConcertPresenter>();
                        services.AddScoped<IMusicianPresenter, MusicianPresenter>();
                        services.AddScoped<ISoundOnConcertPresenter, SoundOnConcertPresenter>();
                        services.AddScoped<ISoundPresenter, SoundPresenter>();
                        services.AddScoped<IConcertPresenter, ConcertPresenter>();

                        // Register controllers
                        services.AddControllers();

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

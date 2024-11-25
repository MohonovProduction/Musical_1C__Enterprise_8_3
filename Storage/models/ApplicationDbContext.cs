using Microsoft.EntityFrameworkCore;

namespace Storage;

public class ApplicationDbContext : DbContext
{
    public DbSet<Instrument> Instruments { get; set; }
    public DbSet<Musician> Musicians { get; set; }
    public DbSet<Concert> Concerts { get; set; }
    public DbSet<Sound> Sounds { get; set; }
    public DbSet<MusicianOnConcert> MusicianOnConcerts { get; set; }
    public DbSet<SoundOnConcert> SoundOnConcerts { get; set; }
    public DbSet<MusicianInstrument> MusicianInstruments { get; set; }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Настройка связей "многие ко многим" через промежуточные таблицы
        modelBuilder.Entity<MusicianOnConcert>()
            .HasKey(mc => new { mc.ConcertId, mc.MusicianId });

        modelBuilder.Entity<SoundOnConcert>()
            .HasKey(sc => new { sc.ConcertId, sc.SoundId });

        modelBuilder.Entity<MusicianInstrument>()
            .HasKey(mi => new { mi.MusicianId, mi.InstrumentId });

        // Дополнительные настройки, если они нужны для индексов или ограничений
        // modelBuilder.Entity<Concert>()
        //     .Property(c => c.Date)
        //     .HasColumnType("text"); // Или другой тип, если требуется
    }
}
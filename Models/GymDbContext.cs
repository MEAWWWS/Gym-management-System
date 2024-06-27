using Microsoft.EntityFrameworkCore;

namespace Gym.Models;

public partial class GymAppDbContext : DbContext
{
    private static GymAppDbContext _context;
    public static GymAppDbContext GetContext() => _context ?? (_context = new GymAppDbContext());
    public GymAppDbContext()
    {
    }

    public GymAppDbContext(DbContextOptions<GymAppDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Aboniments> Aboniments { get; set; }

    public virtual DbSet<Clients> Clients { get; set; }

    public virtual DbSet<Equipments> Equipment { get; set; }

    public virtual DbSet<Sessions> Sessions { get; set; }

    public virtual DbSet<TrainersInfo> TrainerInfos { get; set; }

    public virtual DbSet<Users> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Server=localhost;Database=GymAppDb;Trusted_Connection=True;Integrated Security=True;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Aboniments>(entity =>
        {
            entity.HasKey(e => e.AbonimentId).HasName("PK__Abonimen__3BA71E0AE07D2342");

            entity.ToTable("Aboniment");

            entity.Property(e => e.Price).HasColumnType("money");
        });

        modelBuilder.Entity<Clients>(entity =>
        {
            entity.HasKey(e => e.ClientId).HasName("PK__Client__E67E1A24D2B6C70F");

            entity.ToTable("Client");

            entity.Property(e => e.Email).HasMaxLength(1000);
            entity.Property(e => e.Name).HasMaxLength(500);
            entity.Property(e => e.Phone).HasMaxLength(20);

            entity.HasOne(d => d.Aboniment).WithMany(p => p.Clients)
                .HasForeignKey(d => d.AbonimentId)
                .HasConstraintName("FK__Client__Abonimen__403A8C7D");

            entity.HasOne(d => d.Trainer).WithMany(p => p.Clients)
                .HasForeignKey(d => d.TrainerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Client__TrainerI__412EB0B6");
        });

        modelBuilder.Entity<Equipments>(entity =>
        {
            entity.HasKey(e => e.EquipmentId).HasName("PK__Equipmen__344744790D55033D");

            entity.Property(e => e.Description).HasMaxLength(2000);
            entity.Property(e => e.Title).HasMaxLength(1000);
        });

        modelBuilder.Entity<Sessions>(entity =>
        {
            entity.HasKey(e => e.SessionId).HasName("PK__Session__C9F4929006325B7E");

            entity.ToTable("Session");

            entity.Property(e => e.SessionStartDateTime).HasColumnType("datetime");

            entity.HasOne(d => d.Client).WithMany(p => p.Sessions)
                .HasForeignKey(d => d.ClientId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Session__ClientI__440B1D61");

            entity.HasOne(d => d.Trainer).WithMany(p => p.Sessions)
                .HasForeignKey(d => d.TrainerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Session__Trainer__44FF419A");
        });

        modelBuilder.Entity<TrainersInfo>(entity =>
        {
            entity.HasKey(e => e.TrainerId).HasName("PK__TrainerI__366A1A7CE11C721A");

            entity.ToTable("TrainerInfo");

            entity.Property(e => e.Name).HasMaxLength(500);
            entity.Property(e => e.Schedule).HasMaxLength(200);
            entity.Property(e => e.Specialization).HasMaxLength(1000);

            entity.HasOne(d => d.User).WithMany(p => p.TrainerInfos)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__TrainerIn__UserI__398D8EEE");
        });

        modelBuilder.Entity<Users>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__User__1788CC4CBF6F978C");

            entity.ToTable("User");

            entity.Property(e => e.Login).HasMaxLength(256);
            entity.Property(e => e.PasswordHash).HasMaxLength(100);
            entity.Property(e => e.Role).HasMaxLength(50);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

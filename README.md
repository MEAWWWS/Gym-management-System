# Разработка Системы управления тренажерным залом (C#)
![1](https://github.com/MEAWWWS/Gym-management-System/assets/114382568/436fea03-c419-453e-a339-c09f44d64eca)

Окно "Администратора"

![2](https://github.com/MEAWWWS/Gym-management-System/assets/114382568/6569101d-f81f-45c1-83e4-d3189df91f8b)

Окно "Тренера"

![3](https://github.com/MEAWWWS/Gym-management-System/assets/114382568/64b05fad-3832-4967-9382-80523d652e9c)

Окно "Клиента"

## Функциональность
1. Добавление, редактирование и удаление данных о клиентах, тренерах, тренажерах.
2. Запись клиентов на тренировки к конкретным тренерам.
3. Ввод информации о посещаемости клиентов (дата и время тренировки).
4. Поиск клиентов, тренеров, тренажеров по различным критериям.
5. Генерация отчетов:
    -	Список клиентов с абонементами.
    -	Популярные тренеры.
    -	Загруженность зала по дням недели.
    -	Статистика посещаемости клиентов.
6. Система авторизации пользователей (администраторы, тренеры, менеджеры) с различными правами доступа:
    -	Администраторы могут добавлять/удалять/редактировать клиентов, тренеров, тренажеры, настраивать систему, просматривать все отчеты.
    -	Тренеры могут просматривать своих клиентов, записывать их на тренировки, вносить данные о посещаемости, просматривать отчеты по своим клиентам.
    -	Менеджеры могут просматривать информацию о клиентах, тренерах, тренажерах, генерировать отчеты, управлять абонементами.

## Технические детали 
  - **Разработанно на**: WPF
  - **База данных**: MSSQL
  - **Шаблон проектирования**: MVVM

## Создание классов Aboniments, Clients, Equipments, GymDbContext, Sessions, TrainersInfo и Users для связи с БД

``` C#
namespace Gym.Models;

public partial class Aboniments
{
    public int AbonimentId { get; set; }

    public DateOnly PurchaseDate { get; set; }

    public DateOnly DeadlineDate { get; set; }

    public decimal Price { get; set; }

    //in sql script set that fk as pk
    public virtual ICollection<Clients> Clients { get; set; } = new List<Clients>();
}
namespace Gym.Models;

public partial class Clients
{
    public int ClientId { get; set; }

    public string Name { get; set; } = null!;

    public string Phone { get; set; } = null!;

    public string? Email { get; set; }

    public int AbonimentId { get; set; }

    public int TrainerId { get; set; }

    public virtual Aboniments Aboniment { get; set; } = null!;

    public virtual ICollection<Sessions> Sessions { get; set; } = new List<Sessions>();

    public virtual TrainersInfo Trainer { get; set; } = null!;
}
namespace Gym.Models;

public partial class Equipments
{
    public int EquipmentId { get; set; }

    public string Title { get; set; } = null!;

    public string? Description { get; set; }
}
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
namespace Gym.Models;

public partial class Sessions
{
    public int SessionId { get; set; }

    public int ClientId { get; set; }

    public int TrainerId { get; set; }

    public DateTime SessionStartDateTime { get; set; }

    public DateOnly SessionDate { get; set; }

    public TimeOnly SessionTime { get; set; }

    public virtual Clients Client { get; set; } = null!;

    public virtual TrainersInfo Trainer { get; set; } = null!;
}
namespace Gym.Models;

public partial class TrainersInfo
{
    public int TrainerId { get; set; }

    public int UserId { get; set; }

    public string Name { get; set; } = null!;

    public string Specialization { get; set; } = null!;

    public string Schedule { get; set; } = null!;

    public virtual ICollection<Clients> Clients { get; set; } = new List<Clients>();

    public virtual ICollection<Sessions> Sessions { get; set; } = new List<Sessions>();

    public virtual Users User { get; set; } = null!;
}
  namespace Gym.Models;

public partial class Users
{
    public int UserId { get; set; }

    public string Login { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public string Role { get; set; } = null!;

    public virtual ICollection<TrainersInfo> TrainerInfos { get; set; } = new List<TrainersInfo>();
}

```

## Код создания таблиц SQL
``` SQL
CREATE TABLE [User]
(
	[UserId] INT NOT NULL PRIMARY KEY IDENTITY,
	[Login] NVARCHAR(256) NOT NULL,
	[PasswordHash] NVARCHAR(100) NOT NULL,
	[Role] NVARCHAR(50) NOT NULL
)
CREATE TABLE [TrainerInfo] 
(
	[TrainerId] INT NOT NULL PRIMARY KEY IDENTITY,
	[UserId] INT NOT NULL REFERENCES [User](UserId) ON DELETE CASCADE ON UPDATE NO ACTION,
	[Name] NVARCHAR(500) NOT NULL,
	[Specialization] NVARCHAR(1000) NOT NULL,
	[Schedule] NVARCHAR(200) NOT NULL
)
CREATE TABLE [Aboniment] 
(
	[AbonimentId] INT NOT NULL PRIMARY KEY IDENTITY,
	[PurchaseDate] DATE NOT NULL,
	[DeadlineDate] DATE NOT NULL,
	[Price] MONEY NOT NULL
)
CREATE TABLE [Equipment] 
(
	[EquipmentId] INT NOT NULL PRIMARY KEY IDENTITY,
	[Title] NVARCHAR(1000) NOT NULL,
	[Description] NVARCHAR(2000) NULL
)
CREATE TABLE [Client]
(
	[ClientId] INT NOT NULL PRIMARY KEY IDENTITY,
	[Name] NVARCHAR(500) NOT NULL,
	[Phone] NVARCHAR(20) NOT NULL,
	[Email] NVARCHAR(1000) NULL,
	[AbonimentId] INT NOT NULL REFERENCES [Aboniment](AbonimentId) ON DELETE CASCADE ON UPDATE NO ACTION,
	[TrainerId] INT NOT NULL REFERENCES [TrainerInfo](TrainerId) ON DELETE NO ACTION ON UPDATE NO ACTION
)
CREATE TABLE [Session]
(
	[SessionId] INT NOT NULL PRIMARY KEY IDENTITY,
	[ClientId] INT NOT NULL REFERENCES [Client](ClientId) ON DELETE NO ACTION ON UPDATE NO ACTION,
	[TrainerId] INT NOT NULL REFERENCES [TrainerInfo] ON DELETE NO ACTION ON UPDATE NO ACTION,
	[SessionStartDateTime] DATETIME NOT NULL,
	[SessionDate] DATE NOT NULL,
	[SessionTime] TIME NOT NULL
)

--default password for admin = password
INSERT INTO [User] values
('admin', '5e884898da28047151d0e56f8dc6292773603d0d6aabbdd62a11ef721d1542d8', 'Admin')
```

## База данных SQL
![image](https://github.com/MEAWWWS/Gym-management-System/assets/114382568/5a29c62a-898a-4801-bd1d-80fb03c251c9)

## Автор программы 
### Зимин.М

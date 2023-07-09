using Microsoft.EntityFrameworkCore;

namespace Logic;

public sealed class MyDbContext : DbContext
{
    public MyDbContext(DbContextOptions<MyDbContext> options)
        : base(options)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
    }

    public DbSet<SomeModel> SomeModels { get; set; } = default!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        CreateSomeModel(modelBuilder);
    }

    private void CreateSomeModel(ModelBuilder modelBuilder)
    {
        modelBuilder
            .Entity<SomeModel>()
            .ToTable("some_model_table")
            .HasKey(x => x.Id);
    }
}

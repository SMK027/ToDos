using Microsoft.EntityFrameworkCore;
using ToDos.Models;

namespace ToDos.Services
{
    public class AppDbContext : DbContext
    {
        public DbSet<TodosVM> Todos { get; set; } = null!;

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TodosVM>(entity =>
            {
                entity.ToTable("todos2");

                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("id").ValueGeneratedOnAdd();

                entity.Property(e => e.Libelle).HasColumnName("libelle").IsRequired();
                entity.Property(e => e.Commentaire).HasColumnName("commentaire");

                entity.Property(e => e.Date_planif)
                    .HasColumnName("date_planif")
                    .HasColumnType("datetime2");

                entity.Property(e => e.Date_realisation)
                    .HasColumnName("date_realisation")
                    .HasColumnType("datetime2")
                    .IsRequired(false);
            });
        }
    }
}

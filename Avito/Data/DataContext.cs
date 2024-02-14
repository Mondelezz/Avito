using Avito.Migrations;
using Avito.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGeneration.EntityFrameworkCore;

namespace Avito.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }
        public DbSet<Persons> Users { get; set; }
        public DbSet<AdModels> AdModels { get; set; }
        public DbSet<CategoryModel> Category { get; set; }
        public DbSet<FavoriteModel> Favorites { get; set; }
        public DbSet<Photos> Photos { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<CodeDb> Codes { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=Avito;Username=postgres;Password=26032005");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Persons>()
                .HasMany(e => e.AdModels)
                .WithOne(e => e.Person)
                .HasForeignKey(e => e.PersonId)
                .IsRequired();

            modelBuilder.Entity<FavoriteModel>()
                .HasKey(fa => new { fa.UserId, fa.AdModelId });

            modelBuilder.Entity<FavoriteModel>()
                .HasMany(x => x.User)
                .WithMany(x => x.Favorites);

            modelBuilder.Entity<AdModelPhoto>()
                .HasOne(e => e.AdModel)
                .WithMany(e => e.Photos)
                .HasForeignKey(e => e.AdModelId); 

            modelBuilder.Entity<AdModelPhoto>()
                .HasOne(e => e.Photo)
                .WithMany(e => e.AdModels)
                .HasForeignKey(e => e.PhotoId);

            modelBuilder.Entity<AdModelPhoto>()
                .HasKey(e => new {e.AdModelId, e.PhotoId});

            modelBuilder.Entity<Review>()
                .HasOne(a => a.Persons)
                .WithMany(a => a.Reviews)
                .HasForeignKey(a => a.PersonId);

        }
       
    }

}

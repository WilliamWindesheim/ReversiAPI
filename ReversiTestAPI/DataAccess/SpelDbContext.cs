using Microsoft.EntityFrameworkCore;
using ReversiTestAPI.Model;

namespace ReversiAPI.DataAccess
{
    public class SpelDbContext : DbContext
    {
        public SpelDbContext(DbContextOptions<SpelDbContext> options) : base(options)
        {

        }

        public DbSet<Spel> Spellen { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //EntityFramework Fluent API
            modelBuilder.Entity<Spel>(s => {
                s.HasKey(s => s.ID);
                s.Property(s => s.Omschrijving)
                .IsRequired()
                .HasMaxLength(100);
                s.Property(s => s.Bord)
                .HasConversion(
                    s => Spel.ConvertBordToString(s),
                    s => Spel.ConvertStringToBord(s));
            });
        }

    }
}

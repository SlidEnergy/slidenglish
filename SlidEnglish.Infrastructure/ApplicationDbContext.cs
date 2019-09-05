using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SlidEnglish.App;
using SlidEnglish.Domain;

namespace SlidEnglish.Infrastructure
{
    public class ApplicationDbContext : IdentityDbContext<SlidEnglish.Domain.User>, IApplicationDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<LexicalUnit>()
                .HasIndex(x => x.Text);

            modelBuilder.Entity<LexicalUnitToLexicalUnitRelation>()
                .HasKey(key => new { key.LexicalUnitId, key.RelatedLexicalUnitId });

            modelBuilder.Entity<LexicalUnitToLexicalUnitRelation>()
                .HasOne(e => e.RelatedLexicalUnit)
                .WithMany(e => e.RelatedLexicalUnitsOf)
                .HasForeignKey(e => e.RelatedLexicalUnitId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<LexicalUnitToLexicalUnitRelation>()
                .HasOne(e => e.LexicalUnit)
                .WithMany(e => e.RelatedLexicalUnits)
                .HasForeignKey(e => e.LexicalUnitId);

            modelBuilder.Entity<ExampleOfUse>()
                .HasKey(key => new { key.Example, key.LexicalUnitId });

            modelBuilder.Entity<LexicalUnit>()
                .HasMany(x => x.ExamplesOfUse)
                .WithOne(x => x.LexicalUnit)
                .HasForeignKey(x => x.LexicalUnitId)
                .OnDelete(DeleteBehavior.Cascade);
        }

        public DbSet<LexicalUnit> LexicalUnits { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SlidEnglish.Domain;

namespace SlidEnglish.Infrastructure
{
	public class ApplicationDbContext : IdentityDbContext<SlidEnglish.Domain.User>
	{
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
			: base(options)
		{
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			modelBuilder.Entity<WordSinonym>()
				.HasKey(key => new { key.WordId, key.SinonymId });

			modelBuilder.Entity<WordSinonym>()
				.HasOne(e => e.Sinonym)
				.WithMany(e => e.SinonymOf)
				.HasForeignKey(e => e.SinonymId)
				.OnDelete(DeleteBehavior.Cascade);

			modelBuilder.Entity<WordSinonym>()
				.HasOne(e => e.Word)
				.WithMany(e => e.Sinonyms)
				.HasForeignKey(e => e.WordId);
		}

		public DbSet<SlidEnglish.Domain.Word> Words { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
    }
}

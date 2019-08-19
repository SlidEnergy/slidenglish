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

			modelBuilder.Entity<WordSynonym>()
				.HasKey(key => new { key.WordId, key.SynonymId });

			modelBuilder.Entity<WordSynonym>()
				.HasOne(e => e.Synonym)
				.WithMany(e => e.SynonymOf)
				.HasForeignKey(e => e.SynonymId)
				.OnDelete(DeleteBehavior.Cascade);

			modelBuilder.Entity<WordSynonym>()
				.HasOne(e => e.Word)
				.WithMany(e => e.Synonyms)
				.HasForeignKey(e => e.WordId);
		}

		public DbSet<SlidEnglish.Domain.Word> Words { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
    }
}

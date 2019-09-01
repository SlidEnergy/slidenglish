using Microsoft.EntityFrameworkCore;
using SlidEnglish.Domain;

namespace SlidEnglish.App
{
    public interface IApplicationDbContext
    {
        DbSet<LexicalUnit> LexicalUnits { get; set; }
        DbSet<RefreshToken> RefreshTokens { get; set; }
        DbSet<User> Users { get; set; }
    }
}
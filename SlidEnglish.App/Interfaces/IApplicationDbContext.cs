using Microsoft.EntityFrameworkCore;
using SlidEnglish.Domain;
using System.Threading;
using System.Threading.Tasks;

namespace SlidEnglish.App
{
    public interface IApplicationDbContext
    {
        DbSet<LexicalUnit> LexicalUnits { get; set; }
        DbSet<RefreshToken> RefreshTokens { get; set; }
        DbSet<User> Users { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
using Microsoft.EntityFrameworkCore;
using SlidEnglish.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlidEnglish.App
{
    public static class QueryableExtensions
    {
        public static IQueryable<LexicalUnit> ByUser(this IQueryable<LexicalUnit> lexicalUnits, string userId) =>
            lexicalUnits.Where(x => x.User.Id == userId);

        public static IQueryable<RefreshToken> ByUser(this IQueryable<RefreshToken> refreshTokens, string userId) =>
            refreshTokens.Where(x => x.User.Id == userId);

        public static LexicalUnit Find(this IQueryable<LexicalUnit> lexicalUnits, int id) =>
            lexicalUnits.First(x => x.Id == id);

        public static Task<LexicalUnit> FindAsync(this IQueryable<LexicalUnit> lexicalUnits, int id) =>
           lexicalUnits.FirstAsync(x => x.Id == id);
    }
}

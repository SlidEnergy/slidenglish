using SlidEnglish.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SlidEnglish.UnitTests
{
    public static class QueryableExtensions
    {
        public static IQueryable<LexicalUnit> ByUser(this IQueryable<LexicalUnit> lexicalUnits, string userId) =>
            lexicalUnits.Where(x => x.User.Id == userId);
    }
}

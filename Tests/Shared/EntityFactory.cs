using SlidEnglish.Domain;
using SlidEnglish.Infrastructure;
using System;

namespace SlidEnglish.Web.Tests
{
    public class EntityFactory
    {
        private readonly ApplicationDbContext _context;

        public EntityFactory(ApplicationDbContext context)
        {
            _context = context;
        }

        public LexicalUnit CreateLexicalUnit(User user)
        {
            var lexicalUnit = new LexicalUnit() { Text = Guid.NewGuid().ToString(), User = user };
            _context.LexicalUnits.Add(lexicalUnit);
            _context.SaveChanges();
            return lexicalUnit;
        }
    }
}

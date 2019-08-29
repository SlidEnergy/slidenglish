using SlidEnglish.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SlidEnglish.App
{
    public interface ILexicalUnitsRepository : IRepositoryWithAccessCheck<LexicalUnit>
    {
        Task<LexicalUnit> GetByTextWithAccessCheck(string userId, string text);
    }
}

using SlidEnglish.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SlidEnglish.App
{
    public interface IWordsRepository : IRepositoryWithAccessCheck<Word>
    {
        Task<Word> GetByTextWithAccessCheck(string userId, string text);
    }
}

using System.Threading.Tasks;
using SlidEnglish.App.Dto;

namespace SlidEnglish.App
{
    public interface ITranslateService
    {
        Task AddAsync(string userId, string lexicalUnit);
        Task<TranslateData> ProcessTranslate(string userId, string text);
        string[] Split(string text);
    }
}
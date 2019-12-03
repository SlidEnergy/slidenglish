using System.Threading.Tasks;
using SlidEnglish.App.Dto;

namespace SlidEnglish.App
{
    public interface ITranslateService
    {
        Task<TranslateData> ProcessTranslate(string userId, string text);
    }
}
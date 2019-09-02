using SlidEnglish.App.Dto;
using System.Threading.Tasks;

namespace SlidEnglish.App
{
    public interface ITranslator
    {
        Task<string> TranslateAsync(string text);
    }
}

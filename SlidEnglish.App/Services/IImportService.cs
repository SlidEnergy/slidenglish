using System.Threading.Tasks;

namespace SlidEnglish.App
{
    public interface IImportService
    {
        Task ImportMultiple(string userId, string text);
        Task ImportSingle(string userId, string lexicalUnit);
    }
}
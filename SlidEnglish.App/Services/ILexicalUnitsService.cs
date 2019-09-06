using System.Threading.Tasks;
using SlidEnglish.App.Dto;
using SlidEnglish.Domain;

namespace SlidEnglish.App
{
    public interface ILexicalUnitsService
    {
        Task<Dto.LexicalUnit> AddAsync(string userId, Dto.LexicalUnit lexicalUnit);
        Task DeleteAsync(string userId, int id);
        Task<Dto.LexicalUnit[]> GetListAsync(string userId);
        Task<Dto.LexicalUnit> UpdateAsync(string userId, Dto.LexicalUnit lexicalUnit);
    }
}
using SlidEnglish.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SlidEnglish.App
{
    public interface IRepositoryWithAccessCheck<T> : IRepository<T, int> where T : class, IUniqueObject<int>
    {
        Task<List<T>> GetListWithAccessCheck(string userId);

		Task<T> GetByIdWithAccessCheck(string userId, int id);
    }
}

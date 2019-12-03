using Microsoft.AspNetCore.Identity;
using SlidEnglish.Domain;
using System.Security.Authentication;
using System.Threading.Tasks;

namespace SlidEnglish.App
{
    public class UsersService : IUsersService
    {
        private readonly UserManager<User> _userManager;

        public UsersService(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async Task<User> GetById(string userId)
        {
            return await _userManager.FindByIdAsync(userId);
        }

        public async Task<IdentityResult> Register(User user, string password)
        {
            return await _userManager.CreateAsync(user, password);
        }
    }
}

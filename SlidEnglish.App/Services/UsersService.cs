using Microsoft.AspNetCore.Identity;
using SlidEnglish.Domain;
using System.Security.Authentication;
using System.Threading.Tasks;

namespace SlidEnglish.App
{
    public class UsersService : IUsersService
    {
        private readonly UserManager<User> _userManager;
        private readonly ITokenGenerator _tokenGenerator;
        private readonly ITokenService _tokenService;

        public UsersService(UserManager<User> userManager, ITokenGenerator tokenGenerator, ITokenService tokenService)
        {
            _tokenService = tokenService;
            _userManager = userManager;
            _tokenGenerator = tokenGenerator;
        }

        public async Task<User> GetById(string userId)
        {
            return await _userManager.FindByIdAsync(userId);
        }

        public async Task<IdentityResult> Register(User user, string password)
        {
            return await _userManager.CreateAsync(user, password);
        }

        public async Task<TokensCortage> Login(string email, string password)
        {
            var user = await _userManager.FindByNameAsync(email);

            if (user == null)
                throw new AuthenticationException();

            var checkResult = await _userManager.CheckPasswordAsync(user, password);

            if (!checkResult)
                throw new AuthenticationException();

            var refreshToken = _tokenGenerator.GenerateRefreshToken();
            await _tokenService.AddRefreshToken(refreshToken, user);

            return new TokensCortage()
            {
                Token = _tokenGenerator.GenerateAccessToken(user),
                RefreshToken = refreshToken
            };
        }
    }
}

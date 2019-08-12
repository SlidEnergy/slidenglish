using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SlidEnglish.App;
using System.Threading.Tasks;

namespace SlidEnglish.Web
{
	[Route("api/v1/[controller]")]
    [ApiController]
    public sealed class TokenController : ControllerBase
    {
        private readonly TokenService _tokenService;

        public TokenController(TokenService tokenService)
        {
            _tokenService = tokenService;
        }

        [HttpPost("refresh")]
        public async Task<ActionResult<Dto.TokenInfo>> Refresh(string token, string refreshToken)
        {
            try
            {
                var tokens = await _tokenService.RefreshToken(token, refreshToken);

				return new Dto.TokenInfo() { Token = tokens.Token, RefreshToken = tokens.RefreshToken };
            }
            catch (SecurityTokenException)
            {
                return BadRequest();
            }
        }
    }
}

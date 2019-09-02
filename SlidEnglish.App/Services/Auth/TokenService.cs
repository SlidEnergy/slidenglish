using Microsoft.IdentityModel.Tokens;
using SlidEnglish.Domain;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace SlidEnglish.App
{
    public class TokenService : ITokenService
    {
        private readonly IApplicationDbContext _context;
        private readonly ITokenGenerator _tokenGenerator;
        private readonly AuthSettings _authSettings;

        public TokenService(IApplicationDbContext context, ITokenGenerator tokenGenerator, AuthSettings authSettings)
        {
            _context = context;
            _tokenGenerator = tokenGenerator;
            _authSettings = authSettings;
        }

        public async Task<TokensCortage> RefreshToken(string token, string refreshToken)
        {
            var principal = GetPrincipalFromExpiredToken(token);
            var userId = principal.GetUserId();
            var savedToken = await _context.RefreshTokens.ByUser(userId).FirstAsync();

            if (savedToken == null || savedToken.Token != refreshToken)
                throw new SecurityTokenException("Invalid refresh token");

            var newToken = _tokenGenerator.GenerateAccessToken(principal.Claims);
            var newRefreshToken = _tokenGenerator.GenerateRefreshToken();

            await UpdateRefreshToken(userId, newRefreshToken);

            return new TokensCortage() { Token = newToken, RefreshToken = newRefreshToken };
        }

        public async Task<RefreshToken> AddRefreshToken(string refreshToken, User user)
        {
            var token = new RefreshToken("any", refreshToken, user);
            _context.RefreshTokens.Add(token);
            await _context.SaveChangesAsync();
            return token;
        }

        private async Task UpdateRefreshToken(string userId, string refreshToken)
        {
            var token = await _context.RefreshTokens.ByUser(userId).FirstAsync();
            token.Update("any", refreshToken);
            _context.RefreshTokens.Update(token);
            await _context.SaveChangesAsync();
        }

        private ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false, //you might want to validate the audience and issuer depending on your use case
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = _authSettings.GetSymmetricSecurityKey(),
                ValidateLifetime = false //here we are saying that we don't care about the token's expiration date
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken;
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);
            var jwtSecurityToken = securityToken as JwtSecurityToken;
            if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("Invalid token");

            return principal;
        }
    }
}

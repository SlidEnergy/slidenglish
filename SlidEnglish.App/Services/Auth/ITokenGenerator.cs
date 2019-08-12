using SlidEnglish.Domain;
using System.Collections.Generic;
using System.Security.Claims;

namespace SlidEnglish.App
{
    public interface ITokenGenerator
    {
        string GenerateAccessToken(User user);
        string GenerateAccessToken(IEnumerable<Claim> claims);
        string GenerateRefreshToken();
    }
}

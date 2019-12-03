using SlidEnglish.Domain;
using System.Collections.Generic;
using System.Security.Claims;

namespace SlidEnglish.Web
{
    public interface ITokenGenerator
    {
        string GenerateAccessToken(User user);
        string GenerateAccessToken(IEnumerable<Claim> claims);
        string GenerateRefreshToken();
    }
}

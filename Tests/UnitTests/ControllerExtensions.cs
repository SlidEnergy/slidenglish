using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SlidEnglish.App;
using SlidEnglish.Domain;
using System.Collections.Generic;
using System.Security.Claims;

namespace SlidEnglish.Web.UnitTests
{
    public static class ControllerExtensions
    {
        public static void AddControllerContext(this ControllerBase controller, User user)
        {
            controller.ControllerContext = CreateControllerContext(user);
        }

        private static ControllerContext CreateControllerContext(User user)
        {
            return new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = CreatePrincipal(user)
                }
            };
        }

        private static ClaimsPrincipal CreatePrincipal(User user)
        {
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Email)
            };
            var identity = new ClaimsIdentity(claims, "Test");
            var claimsPrincipal = new ClaimsPrincipal(identity);

            return claimsPrincipal;
        }
    }
}

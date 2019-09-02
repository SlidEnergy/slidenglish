using NUnit.Framework;
using SlidEnglish.App;
using System.Threading.Tasks;
using System.Web;

namespace SlidEnglish.Web.IntegrationTests
{
    public class TokenTests : ControllerTestBase
    {
        [Test]
        public async Task RefreshToken_ShouldReturnTokens()
        {
            var request = CreateAuthJsonRequest("POST", 
                $"/api/v1/token/refresh/?token={HttpUtility.UrlEncode(_accessToken)}&refreshToken={HttpUtility.UrlEncode(_refreshToken)}");
            var response = await SendRequest(request);

            Assert.True(response.IsSuccessStatusCode);
            Assert.NotNull(response.Content);
            var data = await response.ToObject<Dto.TokenInfo>();

            Assert.NotNull(data.Token);
            Assert.NotNull(data.RefreshToken);
            Assert.IsNotEmpty(data.Token);
            Assert.IsNotEmpty(data.RefreshToken);
            Assert.IsNotEmpty(data.Email);
            Assert.AreNotEqual(_accessToken, data.Token);
            Assert.IsNotEmpty(_refreshToken, data.RefreshToken);
        }
    }
}

using Moq;
using NUnit.Framework;
using SlidEnglish.App;
using System.Threading.Tasks;

namespace SlidEnglish.Web.UnitTests
{
    public class TokenControllerTests : TestsBase
    {
        Mock<ITokenService> _service;
		TokenController _controller;
		TokenGenerator _tokenGenerator;

		[SetUp]
        public void Setup()
        {
			var authSettings = SettingsFactory.CreateAuth();
			_tokenGenerator = new TokenGenerator(authSettings);

            _service = new Mock<ITokenService>();

			_controller = new TokenController(_service.Object);
			_controller.AddControllerContext(_user);
		}

        [Test]
        public async Task RefreshToken_ShouldCallMethod()
        {
			var token = _tokenGenerator.GenerateAccessToken(_user);
			var refreshToken = _tokenGenerator.GenerateRefreshToken();

            _service.Setup(x => x.RefreshToken(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(new TokensCortage());
			
			var result = await _controller.Refresh(token, refreshToken);

            _service.Verify(x => x.RefreshToken(It.Is<string>(a => a == token), It.Is<string>(a => a == refreshToken)), Times.Once);
		}
	}
}
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SlidEnglish.App;
using NUnit.Framework;
using System.Threading.Tasks;
using SlidEnglish.Domain;

namespace SlidEnglish.Web.UnitTests
{
    public class UsersControllerTests : TestsBase
    {
        Mock<UserManager<User>> _manager;
		private UsersController _controller;
		private Mock<IUsersService> _usersService;
		private Mock<ITokenService> _tokenService;

		[SetUp]
        public void Setup()
        {
			var authSettings = SettingsFactory.CreateAuth();
			var tokenGenerator = new TokenGenerator(authSettings);
            var store = new Mock<IUserStore<User>>();

            _manager = new Mock<UserManager<User>>(store.Object, null, null, null, null, null, null, null, null);
			_tokenService = new Mock<ITokenService>();
            _usersService = new Mock<IUsersService>();

			_controller = new UsersController(_autoMapper.Create(_db), _usersService.Object, _tokenService.Object);
			_controller.AddControllerContext(_user);
		}

        [Test]
        public async Task GetCurrentUser_ShouldReturnUser()
        {
            _usersService.Setup(x => x.GetById(It.IsAny<string>())).ReturnsAsync(_user);

            var result = await _controller.GetCurrentUser();

            Assert.IsInstanceOf<ActionResult<Dto.User>>(result);

			_usersService.Verify(x => x.GetById(It.Is<string>(u => u == _user.Id)));
		}

        [Test]
        public async Task Login_ShouldReturnTokenAndEmail()
        {
            _tokenService.Setup(x => x.Login(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(new TokensCortage());

            var userBindingModel = new LoginBindingModel() { Email = _user.Email, Password = "Password #1" };

            var result = await _controller.Login(userBindingModel);

			_tokenService.Verify(x => x.Login(It.Is<string>(e => e == userBindingModel.Email), It.Is<string>(p => p == userBindingModel.Password)));
		}

		[Test]
		public async Task Register_ShouldReturnUser()
		{
			var password = "Password #2";
			var email = "test2@email.com";

			_usersService.Setup(x => x.Register(It.IsAny<User>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Success);

			var registerBindingModel = new RegisterBindingModel() { Email = email, Password = password, ConfirmPassword = password };

			var result = await _controller.Register(registerBindingModel);

			Assert.NotNull(((CreatedResult)result.Result).Value);
			_usersService.Setup(x => x.Register(It.Is<User>(u => u.Email == registerBindingModel.Email && u.UserName == registerBindingModel.Email),
				It.Is<string>(p => p == registerBindingModel.Password)));
		}
	}
}
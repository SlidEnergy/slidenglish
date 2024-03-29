﻿using Microsoft.AspNetCore.Identity;
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

		[SetUp]
        public void Setup()
        {
			var authSettings = SettingsFactory.CreateAuth();
			var tokenGenerator = new TokenGenerator(authSettings);
            var store = new Mock<IUserStore<User>>();

            _manager = new Mock<UserManager<User>>(store.Object, null, null, null, null, null, null, null, null);
			var tokenService = new Mock<ITokenService>();
            var service = new UsersService(_manager.Object, tokenGenerator, tokenService.Object);

	        _controller = new UsersController(_autoMapper.Create(_db), service);
			_controller.AddControllerContext(_user);
		}

        [Test]
        public async Task GetCurrentUser_ShouldReturnUser()
        {
            _manager.Setup(x => x.FindByIdAsync(It.IsAny<string>())).Returns(Task.FromResult(_user));

            var result = await _controller.GetCurrentUser();

            Assert.IsInstanceOf<ActionResult<Dto.User>>(result);

            Assert.AreEqual(_user.Id, result.Value.Id);
            Assert.AreEqual(_user.Email, result.Value.Email);
        }

        [Test]
        public async Task Login_ShouldReturnTokenAndEmail()
        {
            _manager.Setup(x => x.CheckPasswordAsync(It.IsAny<User>(), It.IsAny<string>())).Returns(Task.FromResult(true));
            _manager.Setup(x => x.FindByNameAsync(It.IsAny<string>())).Returns(Task.FromResult(_user));

            var userBindingModel = new LoginBindingModel() { Email = _user.Email, Password = "Password #1" };

            var result = await _controller.Login(userBindingModel);

            Assert.NotNull(result.Value.Token);
            Assert.IsNotEmpty(result.Value.Token);
            Assert.AreEqual(_user.Email, result.Value.Email);
        }

		[Test]
		public async Task Register_ShouldReturnUser()
		{
			var password = "Password #2";
			var email = "test2@email.com";

			_manager.Setup(x => x.CreateAsync(It.IsAny<User>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Success);

			var registerBindingModel = new RegisterBindingModel() { Email = email, Password = password, ConfirmPassword = password };

			var result = await _controller.Register(registerBindingModel);

			Assert.NotNull(((CreatedResult)result.Result).Value);
			Assert.AreEqual(email, ((Dto.User)((CreatedResult)result.Result).Value).Email);
		}
	}
}
﻿using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Moq;
using SlidEnglish.App;
using NUnit.Framework;
using System.Threading.Tasks;
using SlidEnglish.Domain;

namespace SlidEnglish.Web.UnitTests
{
	public class UsersServiceTests: TestsBase
    {
		UsersService _service;
		Mock<UserManager<User>> _manager;

		[SetUp]
        public void Setup()
        {
			var authSettings = SettingsFactory.CreateAuth();
			var tokenGenerator = new TokenGenerator(authSettings);
			var store = new Mock<IUserStore<User>>();

			_manager = new Mock<UserManager<User>>(store.Object, null, null, null, null, null, null, null, null);
			var tokenService = new Mock<ITokenService>();
			_service = new UsersService(_manager.Object, tokenGenerator, tokenService.Object);
		}

        [Test]
        public async Task Register_ShouldBeCallAddMethodWithRightArguments()
        {
            _manager.Setup(x => x.CreateAsync(It.IsAny<User>(), It.IsAny<string>())).ReturnsAsync(new IdentityResult());

            var password = "Password1#";

            var result = await _service.Register(_user, password);

            _manager.Verify(x => x.CreateAsync(
                It.Is<User>(u=> u.UserName == _user.UserName && u.Email == _user.Email), 
                It.Is<string>(p=>p == password)), Times.Exactly(1));
        }

        [Test]
        public async Task Login_ShouldBeCallAddMethodWithRightArguments()
        {
            var password = "Password1#";

            _manager.Setup(x => x.CheckPasswordAsync(It.IsAny<User>(), It.IsAny<string>())).Returns(Task.FromResult(true));
            _manager.Setup(x => x.FindByNameAsync(It.IsAny<string>())).Returns(Task.FromResult(_user));

            var result = await _service.Login(_user.Email, password);

            _manager.Verify(x => x.CheckPasswordAsync(
              It.Is<User>(u => u.UserName == _user.UserName && u.Email == _user.Email),
              It.Is<string>(p => p == password)), Times.Exactly(1));
        }
    }
}
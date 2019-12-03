using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using SlidEnglish.Web;
using SlidEnglish.Infrastructure;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Moq;
using SlidEnglish.App;
using SlidEnglish.Domain;

namespace SlidEnglish.Web.UnitTests
{
    public class TokenServiceTests: TestsBase
	{
		Mock<UserManager<User>> _manager;
		TokenService _service;

		[SetUp]
        public void Setup()
        {
			var authSettings = SettingsFactory.CreateAuth();
			var tokenGenerator = new TokenGenerator(authSettings);
			var store = new Mock<IUserStore<User>>();

			_manager = new Mock<UserManager<User>>(store.Object, null, null, null, null, null, null, null, null);
			var authTokenService = new Mock<IAuthTokenService>();
			_service = new TokenService(tokenGenerator, authSettings, authTokenService.Object, _manager.Object);
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
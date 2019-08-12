using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using SlidEnglish.App;
using SlidEnglish.Infrastructure;
using Newtonsoft.Json;
using NUnit.Framework;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using SlidEnglish.Domain;

namespace SlidEnglish.Web.IntegrationTests
{
	public class ControllerTestBase
	{
		protected WebApiApplicationFactory<Startup> _factory;
		protected HttpClient _client;
		protected string _accessToken;
		protected ApplicationDbContext _db;
		protected UserManager<User> _manager;
		protected User _user;
		protected DataAccessLayer _dal;

		[OneTimeSetUp]
		public async Task HttpClientSetup()
		{
			_factory = new WebApiApplicationFactory<Startup>();
			_client = _factory.CreateClient();
			Assert.NotNull(_client);

			var scope = _factory.Server.Host.Services.CreateScope();
			Assert.NotNull(scope);

			_db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
			_manager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
			Assert.NotNull(_db);
			Assert.NotNull(_manager);
			
			_dal = new DataAccessLayer(
                _db,
                new EfRepository<User, string>(_db),
                new EfWordsRepository(_db),
				new EfRefreshTokensRepository(_db));

			_user = new User() { Email = "test1@email.com", UserName = "test1@email.com" };
			var result = await _manager.CreateAsync(_user, "Password123#");
			Assert.IsTrue(result.Succeeded);
			await Login();
		}

		protected virtual async Task Login()
		{
			var request = CreateJsonRequest("POST", "/api/v1/users/login", 
				new { Email = "test1@email.com", Password = "Password123#", ConfirmPassword = "Password123#" });
			var response = await SendRequest(request);

			Assert.IsTrue(response.IsSuccessStatusCode);
			var dict = await response.ToDictionary();

			Assert.IsTrue(dict.ContainsKey("token"));
			_accessToken = (string)dict["token"];
			Assert.IsTrue(_accessToken.Length > 32);
		}

		protected virtual HttpRequestMessage CreateAuthJsonRequest(string method, string url, object content = null)
		{
			var request = new HttpRequestMessage(new HttpMethod(method), url);
			request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
			request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);

			if (content != null)
				request.Content = new StringContent(JsonConvert.SerializeObject(content), Encoding.UTF8, "application/json");

			return request;
		}

		protected virtual HttpRequestMessage CreateJsonRequest(string method, string url, object content = null)
		{
			var request = new HttpRequestMessage(new HttpMethod(method), url);
			request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

			if (content != null)
				request.Content = new StringContent(JsonConvert.SerializeObject(content), Encoding.UTF8, "application/json");

			return request;
		}

		protected virtual Task<HttpResponseMessage> SendRequest(HttpRequestMessage request) => _client.SendAsync(request, CancellationToken.None);
	}
}

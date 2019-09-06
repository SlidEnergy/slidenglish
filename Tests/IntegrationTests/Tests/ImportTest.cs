using NUnit.Framework;
using SlidEnglish.Domain;
using System.Threading.Tasks;

namespace SlidEnglish.Web.IntegrationTests
{
	[TestFixture]
	public class ImportTest : ControllerTestBase
	{
		[Test]
		public async Task Import_ShouldReturnSuccess()
		{
			var request = CreateAuthJsonRequest("POST", "/api/v1/import/", new { Text = "Text for translate" });
			var response = await SendRequest(request);

			Assert.True(response.IsSuccessStatusCode);
		}
	}
}

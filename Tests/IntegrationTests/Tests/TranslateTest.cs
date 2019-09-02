using NUnit.Framework;
using SlidEnglish.Domain;
using System.Threading.Tasks;

namespace SlidEnglish.Web.IntegrationTests
{
	[TestFixture]
	public class TranslateTest : ControllerTestBase
	{
		[Test]
		public async Task Translate_ShouldReturnTranslationString()
		{
			var request = CreateAuthJsonRequest("POST", "/api/v1/translate/", new { Text = "Text for translate" });
			var response = await SendRequest(request);

			Assert.True(response.IsSuccessStatusCode);
            Assert.NotNull(response.Content);
            var data = await response.ToObject<App.Dto.TranslateData>();
            Assert.AreEqual("Текст для перевода", data.Text);
		}
	}
}

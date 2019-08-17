using NUnit.Framework;
using SlidEnglish.Domain;
using System.Threading.Tasks;

namespace SlidEnglish.Web.IntegrationTests
{
	[TestFixture]
	public class WordsTest : ControllerTestBase
	{
		[Test]
		public async Task GetWordsList_ShouldReturnContent()
		{
			var word1 = new Word() { Text = "Word #1", User = _user };
            await _dal.Words.Add(word1);
			var word2 = new Word() { Text = "Word #2", User = _user };
            await _dal.Words.Add(word2);

			var request = CreateAuthJsonRequest("GET", "/api/v1/words/");
			var response = await SendRequest(request);

			Assert.True(response.IsSuccessStatusCode);
			Assert.NotNull(response.Content);
			var array = await response.ToArrayOfDictionaries();
			Assert.GreaterOrEqual(array.Length, 2);
		}

		[Test]
		public async Task AddWord_ShouldReturnContent()
		{
			var request = CreateAuthJsonRequest("POST", "/api/v1/words/", new App.Dto.Word () {  Text = "Word #1" });
			var response = await SendRequest(request);

			Assert.True(response.IsSuccessStatusCode);
			Assert.NotNull(response.Content);
			var dict = await response.ToDictionary();
			Assert.IsTrue(dict.ContainsKey("id"));
		}

		[Test]
		public async Task UpdateWord_ShouldReturnContent()
		{
			var word = new Word() { Text = "Word #1", User = _user };
			await _dal.Words.Add(word);

			var request = CreateAuthJsonRequest("PUT", "/api/v1/words/" + word.Id, new App.Dto.EditWordDto
			{
				Id = word.Id,
				Text = "Word #2",
				Synonyms = new App.Dto.EditSynonymDto[] { }
			});
			
			var response = await SendRequest(request);

			Assert.True(response.IsSuccessStatusCode);
			Assert.NotNull(response.Content);
			var dict = await response.ToDictionary();
			Assert.IsTrue(dict.ContainsKey("id"));
		}

		[Test]
		public async Task DeleteWord_ShouldNoContent()
		{
			var word = new Word() { Text = "Word #1", User = _user };
			await _dal.Words.Add(word);

			var request = CreateAuthJsonRequest("DELETE", "/api/v1/words/" + word.Id);
			var response = await SendRequest(request);

			Assert.True(response.IsSuccessStatusCode);
		}
	}
}

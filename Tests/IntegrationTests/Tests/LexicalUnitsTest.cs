using NUnit.Framework;
using SlidEnglish.Domain;
using System.Threading.Tasks;

namespace SlidEnglish.Web.IntegrationTests
{
	[TestFixture]
	public class LexicalUnitsTest : ControllerTestBase
	{
		[Test]
		public async Task GetWordsList_ShouldReturnContent()
		{
			var word1 = new LexicalUnit() { Text = "Word #1", User = _user };
            await _dal.LexicalUnits.Add(word1);
			var word2 = new LexicalUnit() { Text = "Word #2", User = _user };
            await _dal.LexicalUnits.Add(word2);

			var request = CreateAuthJsonRequest("GET", "/api/v1/lexicalunits/");
			var response = await SendRequest(request);

			Assert.True(response.IsSuccessStatusCode);
			Assert.NotNull(response.Content);
			var array = await response.ToArrayOfDictionaries();
			Assert.GreaterOrEqual(array.Length, 2);
		}

		[Test]
		public async Task AddWord_ShouldReturnContent()
		{
			var request = CreateAuthJsonRequest("POST", "/api/v1/lexicalunits/", new App.Dto.LexicalUnit () {  Text = "Word #1" });
			var response = await SendRequest(request);

			Assert.True(response.IsSuccessStatusCode);
			Assert.NotNull(response.Content);
			var dict = await response.ToDictionary();
			Assert.IsTrue(dict.ContainsKey("id"));
		}

		[Test]
		public async Task UpdateWord_ShouldReturnContent()
		{
			var word = new LexicalUnit() { Text = "Word #1", User = _user };
			await _dal.LexicalUnits.Add(word);

			var request = CreateAuthJsonRequest("PUT", "/api/v1/lexicalunits/" + word.Id, new App.Dto.LexicalUnit
			{
				Id = word.Id,
				Text = "Word #2",
				RelatedLexicalUnits = new App.Dto.LexicalUnitRelation[] { }
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
			var word = new LexicalUnit() { Text = "Word #1", User = _user };
			await _dal.LexicalUnits.Add(word);

			var request = CreateAuthJsonRequest("DELETE", "/api/v1/lexicalunits/" + word.Id);
			var response = await SendRequest(request);

			Assert.True(response.IsSuccessStatusCode);
		}
	}
}

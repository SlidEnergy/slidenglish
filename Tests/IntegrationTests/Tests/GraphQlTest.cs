using Newtonsoft.Json.Linq;
using NUnit.Framework;
using SlidEnglish.Domain;
using SlidEnglish.Web.Graphql;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SlidEnglish.Web.IntegrationTests
{
	[TestFixture]
	public class GraphQlTest : ControllerTestBase
	{
		[Test]
		[TestCase("/schemafirstgraphql")]
		[TestCase("/typefirstgraphql")]
		public async Task GetWordsList_ShouldReturnContent(string url)
		{
			var word1 = new Word() { Text = "Word #1", User = _user };
            await _dal.Words.Add(word1);
			var word2 = new Word() { Text = "Word #2", User = _user };
            await _dal.Words.Add(word2);

			var request = CreateAuthJsonRequest("POST", url, new { Query = "{words{id,text,association,description,synonyms{id,text}}}" });
			var response = await SendRequest(request);

			Assert.True(response.IsSuccessStatusCode);
			Assert.NotNull(response.Content);
			var array = (await response.ToGraphQlResponse<Word[]>("words"));
			Assert.GreaterOrEqual(array.Length, 2);
		}

		[Test]
		[TestCase("/schemafirstgraphql")]
		[TestCase("/typefirstgraphql")]
		public async Task AddWord_ShouldReturnContent(string url)
		{
			var request = CreateAuthJsonRequest("POST", url, new GraphQlQuery()
			{
				Query = "mutation($word: wordInput!){addWord(word: $word){id,text,association,description,synonyms{id,text}}}",
				Variables = JObject.FromObject(new { word = new { text = "Word #1", association = "Association #1", description = "Description #1", synonyms = new int[] { } } })
			});
			var response = await SendRequest(request);

			Assert.True(response.IsSuccessStatusCode);
			Assert.NotNull(response.Content);
			var addedWord = await response.ToGraphQlResponse<Word>("addWord");
			Assert.IsNotNull(addedWord);
		}

		[Test]
		[TestCase("/schemafirstgraphql")]
		[TestCase("/typefirstgraphql")]
		public async Task UpdateWord_ShouldReturnContent(string url)
		{
			var word = new Word() { Text = "Word #1", User = _user };
			await _dal.Words.Add(word);

			var request = CreateAuthJsonRequest("POST", url, new GraphQlQuery()
			{
				Query = "mutation($word: wordInput!){updateWord(word: $word){id,text,association,description,synonyms{id,text}}}",
				Variables = JObject.FromObject(new { word = new { id = word.Id, text = "Word #1", association = "Association #1", description = "Description #1", synonyms = new int[] { } } })
			});

			var response = await SendRequest(request);

			Assert.True(response.IsSuccessStatusCode);
			Assert.NotNull(response.Content);
			var updatedWord = await response.ToGraphQlResponse<Word>("updateWord");
			Assert.IsNotNull(updatedWord);
		}

		[Test]
		[TestCase("/schemafirstgraphql")]
		[TestCase("/typefirstgraphql")]
		public async Task DeleteWord_ShouldNoContent(string url)
		{
			var word = new Word() { Text = "Word #1", User = _user };
			await _dal.Words.Add(word);

			var request = CreateAuthJsonRequest("POST", url, new GraphQlQuery()
			{
				Query = "mutation($id: Int!){deleteWord(id: $id)}",
				Variables = JObject.FromObject(new { id = word.Id })
			});
			var response = await SendRequest(request);

			Assert.True(response.IsSuccessStatusCode);
			var result = await response.ToGraphQlResponse<bool>("deleteWord");
			Assert.IsTrue(result);
		}
	}
}

using NUnit.Framework;
using SlidEnglish.Domain;
using SlidEnglish.Web.Tests;
using System.Threading.Tasks;

namespace SlidEnglish.Web.IntegrationTests
{
	[TestFixture]
	public class LexicalUnitsTest : ControllerTestBase
	{
        private EntityFactory _entityFactory;

        [SetUp]
        public void Setup()
        {
            _entityFactory = new EntityFactory(_db);
        }

        [Test]
		public async Task GetWordsList_ShouldReturnContent()
		{
            _entityFactory.CreateLexicalUnit(_user);
			_entityFactory.CreateLexicalUnit(_user);

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
			var word = _entityFactory.CreateLexicalUnit(_user);

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
			var word = _entityFactory.CreateLexicalUnit(_user);

            var request = CreateAuthJsonRequest("DELETE", "/api/v1/lexicalunits/" + word.Id);
			var response = await SendRequest(request);

			Assert.True(response.IsSuccessStatusCode);
		}
	}
}

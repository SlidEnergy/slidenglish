using Moq;
using SlidEnglish.Web;
using SlidEnglish.App;
using NUnit.Framework;
using System.Linq;
using System.Threading.Tasks;
using SlidEnglish.Domain;
using SlidEnglish.Web.Controllers;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using GraphQL;

namespace SlidEnglish.Web.UnitTests
{
    public class GraphQlTests : TestsBase
    {
		private WordsService _service;

		[SetUp]
        public void Setup()
        {
            _service = new WordsService(_mockedDal, _autoMapper.Create(_db));
		}

        [Test]
        public async Task SchemaFirst_GetWords_ShouldReturnListWithSynonyms()
        {
			var word1 = new Word() { Text = "Word #1", User = _user };
			await _dal.Words.Add(word1);
			var word2 = new Word() { Text = "Word #2", User = _user };
			await _dal.Words.Add(word2);

			word1.Synonyms = new List<WordSynonym>() { new WordSynonym(word1, word2) };
			await _dal.Words.Update(word1);

			_words.Setup(x => x.GetListWithAccessCheck(It.IsAny<string>())).ReturnsAsync(_user.Words.ToList());

			var query = new Graphql.GraphQlQuery()
			{
				Query = "{words{id,text,association,description,synonyms{id,text}}}"
			};

            var controller = new SchemaFirstGraphQlController(_service, null);
            controller.AddControllerContext(_user);
            var result = await controller.Post(query);
			var data = ((Dictionary<string, object>)((ExecutionResult)((OkObjectResult)result.Result).Value).Data).First().Value;

            Assert.IsTrue(data.As<List<object>>()[0].As<Dictionary<string, object>>()["synonyms"].As<List<object>>().Count == 1);
            Assert.IsTrue(data.As<List<object>>()[1].As<Dictionary<string, object>>()["synonyms"].As<List<object>>().Count == 1);
        }

        [Test]
        public async Task TypeFirst_GetWords_ShouldReturnListWithSynonyms()
        {
            var word1 = new Word() { Text = "Word #1", User = _user };
            await _dal.Words.Add(word1);
            var word2 = new Word() { Text = "Word #2", User = _user };
            await _dal.Words.Add(word2);

            word1.Synonyms = new List<WordSynonym>() { new WordSynonym(word1, word2) };
            await _dal.Words.Update(word1);

            _words.Setup(x => x.GetListWithAccessCheck(It.IsAny<string>())).ReturnsAsync(_user.Words.ToList());

            var query = new Graphql.GraphQlQuery()
            {
                Query = "{words{id,text,association,description,synonyms{id,text}}}"
            };

            var controller = new TypeFirstGraphQlController(_service, null);
            controller.AddControllerContext(_user);
            var result = await controller.Post(query);
            var data = ((Dictionary<string, object>)((ExecutionResult)((OkObjectResult)result.Result).Value).Data).First().Value;

            Assert.IsTrue(data.As<List<object>>()[0].As<Dictionary<string, object>>()["synonyms"].As<List<object>>().Count == 1);
            Assert.IsTrue(data.As<List<object>>()[1].As<Dictionary<string, object>>()["synonyms"].As<List<object>>().Count == 1);
        }
    }
}
using Moq;
using SlidEnglish.Web;
using SlidEnglish.App;
using NUnit.Framework;
using System.Linq;
using System.Threading.Tasks;
using SlidEnglish.Domain;

namespace SlidEnglish.Web.UnitTests
{
    public class WordsControllerTests : TestsBase
    {
		private WordsController _controller;

		[SetUp]
        public void Setup()
        {
            var service = new WordsService(_mockedDal);
			_controller = new WordsController(_autoMapper.Create(_db), service);
			_controller.AddControllerContext(_user);
		}

        [Test]
        public async Task GetWords_ShouldReturnList()
        {
            await _dal.Words.Add(new Word()
            {
                Text = "Word #1",
                User = _user
            });
            await _dal.Words.Add(new Word()
            {
                Text = "Word #2",
                User = _user
            });

            _words.Setup(x => x.GetListWithAccessCheck(It.IsAny<string>())).ReturnsAsync(_user.Words.ToList());

            var result = await _controller.GetList();

            Assert.AreEqual(2, result.Value.Count());
        }

        [Test]
        public async Task GetEmptyWordsList_ShouldBeEmptyListReturned()
        {
            _words.Setup(x => x.GetListWithAccessCheck(It.IsAny<string>())).ReturnsAsync(_user.Words.ToList());

            var result = await _controller.GetList();

            Assert.AreEqual(0, result.Value.Count());
        }
    }
}
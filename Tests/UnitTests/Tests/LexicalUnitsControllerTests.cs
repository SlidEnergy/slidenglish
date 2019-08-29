using Moq;
using SlidEnglish.Web;
using SlidEnglish.App;
using NUnit.Framework;
using System.Linq;
using System.Threading.Tasks;
using SlidEnglish.Domain;

namespace SlidEnglish.Web.UnitTests
{
    public class LexicalUnitsControllerTests : TestsBase
    {
		private LexicalUnitsController _controller;

		[SetUp]
        public void Setup()
        {
            var service = new LexicalUnitsService(_mockedDal, _autoMapper.Create(_db));
			_controller = new LexicalUnitsController(_autoMapper.Create(_db), service);
			_controller.AddControllerContext(_user);
		}

        [Test]
        public async Task GetWords_ShouldReturnList()
        {
            await _dal.LexicalUnits.Add(new LexicalUnit()
            {
                Text = "Word #1",
                User = _user
            });
            await _dal.LexicalUnits.Add(new LexicalUnit()
            {
                Text = "Word #2",
                User = _user
            });

            _words.Setup(x => x.GetListWithAccessCheck(It.IsAny<string>())).ReturnsAsync(_user.LexicalUnits.ToList());

            var result = await _controller.GetList();

            Assert.AreEqual(2, result.Value.Count());
        }

        [Test]
        public async Task GetEmptyWordsList_ShouldBeEmptyListReturned()
        {
            _words.Setup(x => x.GetListWithAccessCheck(It.IsAny<string>())).ReturnsAsync(_user.LexicalUnits.ToList());

            var result = await _controller.GetList();

            Assert.AreEqual(0, result.Value.Count());
        }
    }
}
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
        private Mock<ILexicalUnitsService> _service;

		[SetUp]
        public void Setup()
        {
            _service = new Mock<ILexicalUnitsService>();
            
			_controller = new LexicalUnitsController(_autoMapper.Create(_db), _service.Object);
			_controller.AddControllerContext(_user);
		}

        [Test]
        public async Task GetWords_ShouldCallMethod()
        {
            _service.Setup(x => x.GetListAsync(It.IsAny<string>())).ReturnsAsync(new App.Dto.LexicalUnit[] { });

            var result = await _controller.GetList();

            _service.Verify(x => x.GetListAsync(It.Is<string>(a => a == _user.Id)));
        }

        [Test]
        public async Task AddWords_ShouldCallMethod()
        {
            _service.Setup(x => x.AddAsync(It.IsAny<string>(), It.IsAny<App.Dto.LexicalUnit>())).ReturnsAsync(new App.Dto.LexicalUnit { });

            var result = await _controller.Add(new App.Dto.LexicalUnit { Id = 1 });

            _service.Verify(x => x.AddAsync(It.Is<string>(a => a == _user.Id), It.Is<App.Dto.LexicalUnit>(a => a.Id == 1)));
        }

        [Test]
        public async Task UpdateWords_ShouldCallMethod()
        {
            _service.Setup(x => x.UpdateAsync(It.IsAny<string>(), It.IsAny<App.Dto.LexicalUnit>())).ReturnsAsync(new App.Dto.LexicalUnit { });

            var result = await _controller.Update(1, new App.Dto.LexicalUnit { Id = 1 });

            _service.Verify(x => x.UpdateAsync(It.Is<string>(a => a == _user.Id), It.Is<App.Dto.LexicalUnit>(a => a.Id == 1)));
        }

        [Test]
        public async Task DeleteWords_ShouldCallMethod()
        {
            _service.Setup(x => x.DeleteAsync(It.IsAny<string>(), It.IsAny<int>())).Returns(Task.CompletedTask);

            var result = await _controller.Delete(1);

            _service.Verify(x => x.DeleteAsync(It.Is<string>(a => a == _user.Id), It.Is<int>(a => a == 1)), Times.Once);
        }
    }
}
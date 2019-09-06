using Moq;
using SlidEnglish.Web;
using SlidEnglish.App;
using NUnit.Framework;
using System.Linq;
using System.Threading.Tasks;
using SlidEnglish.Domain;

namespace SlidEnglish.Web.UnitTests
{
    public class ImportControllerTests : TestsBase
    {
		private ImportController _controller;
        private Mock<IImportService> _service;

		[SetUp]
        public void Setup()
        {
            _service = new Mock<IImportService>();
            
			_controller = new ImportController(_service.Object);
			_controller.AddControllerContext(_user);
		}

        [Test]
        public async Task Import_ShouldCallMethod()
        {
            _service.Setup(x => x.ImportMultiple(It.IsAny<string>(), It.IsAny<string>())).Returns(Task.CompletedTask);

            var text = "Text to import";
            await _controller.Import(new ImportDataBindingModel { Text = text });

            _service.Verify(x => x.ImportMultiple(It.Is<string>(a => a == _user.Id), It.Is<string>(a => a == text)));
        }
    }
}
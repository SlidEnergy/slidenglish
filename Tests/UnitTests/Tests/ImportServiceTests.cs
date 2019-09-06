using NUnit.Framework;
using SlidEnglish.App;
using SlidEnglish.Domain;
using System.Linq;
using System.Threading.Tasks;

namespace SlidEnglish.Web.UnitTests
{
    public class ImportServiceTests : TestsBase
    {
        private ImportService _service;

		[SetUp]
        public void Setup()
        {
            _service = new ImportService(_db);
		}

        [Test]
        public async Task Import_ShouldImportedWords()
        {
            var words = new string[] { "Word #1", "Word #2", "Word #3" };

            await _service.ImportMultiple(_user.Id, string.Join('\n', words));

            Assert.AreEqual(3, _db.LexicalUnits.ByUser(_user.Id).Where(x =>
                words.Contains(x.Text) &&
                x.InputAttributes == LexicalUnitInputAttribute.UserInput).Count());
        }
    }
}
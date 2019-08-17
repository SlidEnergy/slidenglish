using Moq;
using SlidEnglish.App;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SlidEnglish.Domain;

namespace SlidEnglish.Web.UnitTests
{
    public class WordsServiceTests : TestsBase
    {
        private WordsService _service;

        [SetUp]
        public void Setup()
        {
            _service = new WordsService(_mockedDal, _autoMapper.Create(_db));
        }

        [Test]
        public async Task AddWord_ShouldCallAddMethodWithRightArguments()
        {
            _users.Setup(x => x.GetById(It.IsAny<string>())).ReturnsAsync(_user);
            _words.Setup(x => x.Add(It.IsAny<Word>())).ReturnsAsync(new Word());

            var category1 = await _service.AddAsync(_user.Id, new App.Dto.Word() { Text = "Word #1" });

            _words.Verify(x => x.Add(
                It.Is<Word>(c => c.Text == "Word #1" && c.User.Id == _user.Id)), Times.Exactly(1));
        }

        [Test]
        public async Task DeleteWord_ShouldCallAddMethodWithRightArguments()
        {
            var word = await _dal.Words.Add(new Word()
            {
                Text = "Word #1",
                User = _user
            });

            _users.Setup(x => x.GetById(It.IsAny<string>())).ReturnsAsync(_user);
            _words.Setup(x => x.GetByIdWithAccessCheck(It.IsAny<string>(), It.IsAny<int>())).ReturnsAsync(word);
            _words.Setup(x => x.Delete(It.IsAny<Word>())).Returns(Task.CompletedTask);

            await _service.DeleteAsync(_user.Id, word.Id);

            _words.Verify(x => x.Delete(
                It.Is<Word>(c => c.Text == word.Text && word.User.Id == word.User.Id)),
                Times.Exactly(1));
        }

        [Test]
        public async Task GetWords_ShouldCallGetListMethodWithRightArguments()
        {
            _words.Setup(x => x.GetListWithAccessCheck(It.IsAny<string>())).ReturnsAsync(_user.Words.ToList());

            var result = await _service.GetListAsync(_user.Id);

            _words.Verify(x => x.GetListWithAccessCheck(
                    It.Is<string>(c => c == _user.Id)),
                Times.Exactly(1));
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

            var result = await _service.GetListAsync(_user.Id);
            
            Assert.AreEqual(2, result.Count());
        }
    }
}
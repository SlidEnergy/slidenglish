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

			var word = new App.Dto.Word() {
				Text = "Word #1",
				Association = "Association #1",
				Description = "Description #1"
			};

			await _service.AddAsync(_user.Id, word);

            _words.Verify(x => x.Add(It.Is<Word>(c =>
				c.Id == word.Id &&
				c.Text == word.Text && 
				c.Association == word.Association &&
				c.Description == word.Description &&
				c.User.Id == _user.Id)), Times.Exactly(1));
        }

		[Test]
		public async Task AddWordWithSynonym_ShouldCallAddMethodWithRightArguments()
		{
			var word1 = await _dal.Words.Add(new Word()
			{
				Text = "Word #1",
				User = _user
			});

			var word2 = new App.Dto.Word()
			{
				Text = "Word #1",
				Synonyms = new int[] { word1.Id }
			};

			_users.Setup(x => x.GetById(It.IsAny<string>())).ReturnsAsync(_user);
			_words.Setup(x => x.Add(It.IsAny<Word>())).ReturnsAsync(new Word());
			_words.Setup(x => x.GetByIdWithAccessCheck(It.IsAny<string>(), It.Is<int>(id => id == word1.Id))).ReturnsAsync(word1);

			await _service.AddAsync(_user.Id, word2);

			_words.Verify(x => x.Add(It.Is<Word>(c => 
				c.Id == word2.Id &&
				c.Text == word2.Text && 
				c.Synonyms != null && c.Synonyms.Count() > 0 && c.Synonyms[0].SynonymId == word1.Id &&
				c.User.Id == _user.Id)), Times.Exactly(1));
		}

		[Test]
		public async Task UpdateWord_ShouldCallUpdateMethodWithRightArguments()
		{
			var word = await _dal.Words.Add(new Word()
			{
				Text = "Word #1",
				User = _user
			});

			_users.Setup(x => x.GetById(It.IsAny<string>())).ReturnsAsync(_user);
			_words.Setup(x => x.GetByIdWithAccessCheck(It.IsAny<string>(), It.IsAny<int>())).ReturnsAsync(word);
			_words.Setup(x => x.Update(It.IsAny<Word>())).ReturnsAsync(new Word());

			var updatedWord = new App.Dto.Word()
			{
				Id = word.Id,
				Text = "Word #2",
				Association = "Association #2",
				Description = "Description #2",
			};

			var category1 = await _service.UpdateAsync(_user.Id, updatedWord);

			_words.Verify(x => x.Update(
				It.Is<Word>(c =>
				c.Id == updatedWord.Id &&
				c.Text == updatedWord.Text &&
				c.Association == updatedWord.Association &&
				c.Description == updatedWord.Description &&
				c.User.Id == _user.Id)), Times.Exactly(1));
		}

		[Test]
		public async Task UpdateWordWithDirectSynonyms_ShouldCallUpdateMethodWithRightArguments()
		{
			var word1 = await _dal.Words.Add(new Word()
			{
				Text = "Word #1",
				User = _user
			});
			var word2 = await _dal.Words.Add(new Word()
			{
				Text = "Word #2",
				User = _user
			});
			var word3 = await _dal.Words.Add(new Word()
			{
				Text = "Word #3",
				User = _user,
			});

			_users.Setup(x => x.GetById(It.IsAny<string>())).ReturnsAsync(_user);
			_words.Setup(x => x.GetByIdWithAccessCheck(It.IsAny<string>(), It.Is<int>(id => id == word1.Id))).ReturnsAsync(word1);
			_words.Setup(x => x.GetByIdWithAccessCheck(It.IsAny<string>(), It.Is<int>(id => id == word2.Id))).ReturnsAsync(word2);
			_words.Setup(x => x.Update(It.IsAny<Word>())).ReturnsAsync(new Word());

			var updatedWord = new App.Dto.Word()
			{
				Id = word1.Id,
				Synonyms = new [] { word2.Id }
			};

			var category1 = await _service.UpdateAsync(_user.Id, updatedWord);

			_words.Verify(x => x.Update(
				It.Is<Word>(c => 
				c.Id == updatedWord.Id && 
				c.Synonyms != null && c.Synonyms.Count() > 0 && c.Synonyms[0].SynonymId == word2.Id && c.Synonyms[0].WordId == word1.Id &&
				c.User.Id == _user.Id)), Times.Exactly(1));
		}

		[Test]
		public async Task UpdateWordWithSynonymsOf_ShouldCallUpdateMethodWithRightArguments()
		{
			var word1 = await _dal.Words.Add(new Word()
			{
				Text = "Word #1",
				User = _user
			});
			var word2 = await _dal.Words.Add(new Word()
			{
				Text = "Word #2",
				User = _user,
			});
			word2.Synonyms = new List<WordSynonym>() { new WordSynonym(word2, word1) };

			var word3 = await _dal.Words.Add(new Word()
			{
				Text = "Word #3",
				User = _user,
			});

			_users.Setup(x => x.GetById(It.IsAny<string>())).ReturnsAsync(_user);
			_words.Setup(x => x.GetByIdWithAccessCheck(It.IsAny<string>(), It.Is<int>(id => id == word1.Id))).ReturnsAsync(word1);
			_words.Setup(x => x.GetByIdWithAccessCheck(It.IsAny<string>(), It.Is<int>(id => id == word2.Id))).ReturnsAsync(word2);
			_words.Setup(x => x.Update(It.IsAny<Word>())).ReturnsAsync(new Word());

			var updatedWord = new App.Dto.Word()
			{
				Id = word1.Id,
				Synonyms = new[] { word2.Id }
			};

			var category1 = await _service.UpdateAsync(_user.Id, updatedWord);

			_words.Verify(x => x.Update(
				It.Is<Word>(c =>
				c.Id == updatedWord.Id &&
				c.SynonymOf != null && c.SynonymOf.Count() > 0 && c.SynonymOf[0].SynonymId == word1.Id && c.SynonymOf[0].WordId == word2.Id &&
				c.User.Id == _user.Id)), Times.Exactly(1));
		}

		[Test]
		public async Task UpdateWordWithComplexSynonyms_ShouldCallUpdateMethodWithRightArguments()
		{
			var word1 = await _dal.Words.Add(new Word()
			{
				Text = "Word #1",
				User = _user
			});
			var word2 = await _dal.Words.Add(new Word()
			{
				Text = "Word #2",
				User = _user
			});

			word1.Synonyms = new List<WordSynonym>() { new WordSynonym(word1, word2) };

			var word3 = await _dal.Words.Add(new Word()
			{
				Text = "Word #3",
				User = _user,
			});
			word3.Synonyms = new List<WordSynonym>() { new WordSynonym(word3, word1) };
			var word4 = await _dal.Words.Add(new Word()
			{
				Text = "Word #4",
				User = _user,
			});
			word4.Synonyms = new List<WordSynonym>() { new WordSynonym(word4, word1) };

			var word5 = await _dal.Words.Add(new Word()
			{
				Text = "Word #5",
				User = _user,
			});

			_users.Setup(x => x.GetById(It.IsAny<string>())).ReturnsAsync(_user);
			_words.Setup(x => x.GetByIdWithAccessCheck(It.IsAny<string>(), It.Is<int>(id => id == word1.Id))).ReturnsAsync(word1);
			_words.Setup(x => x.GetByIdWithAccessCheck(It.IsAny<string>(), It.Is<int>(id => id == word2.Id))).ReturnsAsync(word2);
			_words.Setup(x => x.GetByIdWithAccessCheck(It.IsAny<string>(), It.Is<int>(id => id == word3.Id))).ReturnsAsync(word3);
			_words.Setup(x => x.GetByIdWithAccessCheck(It.IsAny<string>(), It.Is<int>(id => id == word4.Id))).ReturnsAsync(word4);
			_words.Setup(x => x.GetByIdWithAccessCheck(It.IsAny<string>(), It.Is<int>(id => id == word5.Id))).ReturnsAsync(word5);
			_words.Setup(x => x.Update(It.IsAny<Word>())).ReturnsAsync(new Word());

			var updatedWord = new App.Dto.Word()
			{
				Id = word1.Id,
				Synonyms = new[] { word5.Id, word3.Id }
			};

			await _service.UpdateAsync(_user.Id, updatedWord);

			_words.Verify(x => x.Update(
				It.Is<Word>(c =>
				c.Id == updatedWord.Id &&
				c.Synonyms != null && c.Synonyms.Count() > 0 && c.Synonyms[0].WordId == word1.Id && c.Synonyms[0].SynonymId == word5.Id &&
				c.SynonymOf != null && c.SynonymOf.Count() > 0 && c.SynonymOf[0].WordId == word3.Id && c.SynonymOf[0].SynonymId == word1.Id &&
				c.User.Id == _user.Id)), Times.Exactly(1));
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
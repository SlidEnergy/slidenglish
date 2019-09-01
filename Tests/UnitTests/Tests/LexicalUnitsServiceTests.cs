using Moq;
using SlidEnglish.App;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SlidEnglish.Domain;
using Microsoft.EntityFrameworkCore;

namespace SlidEnglish.Web.UnitTests
{
    public class LexicalUnitsServiceTests : TestsBase
    {
        private LexicalUnitsService _service;

        [SetUp]
        public void Setup()
        {
            _service = new LexicalUnitsService(_mockedDal, _autoMapper.Create(_db), _mockedContext.Object);
        }

        [Test]
        public async Task AddWord_ShouldCallAddMethodWithRightArguments()
        {
            

            _users.Setup(x => x.GetById(It.IsAny<string>())).ReturnsAsync(_user);
            _words.Setup(x => x.Add(It.IsAny<LexicalUnit>())).ReturnsAsync(new LexicalUnit());

			var word = new App.Dto.LexicalUnit()
            {
				Text = "Word #1",
				Association = "Association #1",
				Notes = "Description #1"
			};

			await _service.AddAsync(_user.Id, word);

            _words.Verify(x => x.Add(It.Is<LexicalUnit>(c =>
				c.Id == word.Id &&
				c.Text == word.Text && 
				c.Association == word.Association &&
				c.Notes == word.Notes &&
                c.InputAttributes == LexicalUnitInputAttribute.UserInput &&
				c.User.Id == _user.Id)), Times.Exactly(1));
        }

        [Test]
        public async Task AddWordWithExamplesOfUse_ShouldCallAddMethodWithRightArguments()
        {
            _users.Setup(x => x.GetById(It.IsAny<string>())).ReturnsAsync(_user);
            _words.Setup(x => x.Add(It.IsAny<LexicalUnit>())).ReturnsAsync(new LexicalUnit());

            var word = new App.Dto.LexicalUnit()
            {
                Text = "Word #1",
                ExamplesOfUse = new[] { new ExampleOfUse { Example = "Sentence #1" } }
            };

            await _service.AddAsync(_user.Id, word);

            _words.Verify(x => x.Add(It.Is<LexicalUnit>(c =>
                c.Id == word.Id &&
                c.ExamplesOfUse != null && c.ExamplesOfUse.Count == 1 && c.ExamplesOfUse[0].Example == word.ExamplesOfUse[0].Example &&
                c.User.Id == _user.Id)), Times.Exactly(1));
        }
        [Test]
		public async Task AddWordWithRelatedWord_ShouldCallAddMethodWithRightArguments()
		{
			var word1 = await _dal.LexicalUnits.Add(new LexicalUnit()
			{
				Text = "Word #1",
				User = _user
			});

			var word2 = new App.Dto.LexicalUnit()
			{
				Text = "Word #1",
				RelatedLexicalUnits = new [] { new App.Dto.LexicalUnitRelation() { LexicalUnitId = word1.Id } }
			};

			_users.Setup(x => x.GetById(It.IsAny<string>())).ReturnsAsync(_user);
			_words.Setup(x => x.Add(It.IsAny<LexicalUnit>())).ReturnsAsync(new LexicalUnit());
			_words.Setup(x => x.GetByIdWithAccessCheck(It.IsAny<string>(), It.Is<int>(id => id == word1.Id))).ReturnsAsync(word1);

			await _service.AddAsync(_user.Id, word2);

			_words.Verify(x => x.Add(It.Is<LexicalUnit>(c => 
				c.Id == word2.Id &&
				c.Text == word2.Text && 
				c.RelatedLexicalUnits != null && c.RelatedLexicalUnits.Count() == 1 && c.RelatedLexicalUnits[0].RelatedLexicalUnitId == word1.Id &&
				c.User.Id == _user.Id)), Times.Exactly(1));
		}

		[Test]
		public async Task UpdateWord_ShouldCallUpdateMethodWithRightArguments()
		{
			var word = await _dal.LexicalUnits.Add(new LexicalUnit()
			{
				Text = "Word #1",
				User = _user
			});

			_users.Setup(x => x.GetById(It.IsAny<string>())).ReturnsAsync(_user);
			_words.Setup(x => x.GetByIdWithAccessCheck(It.IsAny<string>(), It.IsAny<int>())).ReturnsAsync(word);
			_words.Setup(x => x.Update(It.IsAny<LexicalUnit>())).ReturnsAsync(new LexicalUnit());

			var updatedWord = new App.Dto.LexicalUnit()
			{
				Id = word.Id,
				Text = "Word #2",
				Association = "Association #2",
				Notes = "Description #2",
			};

			await _service.UpdateAsync(_user.Id, updatedWord);

			_words.Verify(x => x.Update(
				It.Is<LexicalUnit>(c =>
				c.Id == updatedWord.Id &&
				c.Text == updatedWord.Text &&
				c.Association == updatedWord.Association &&
				c.Notes == updatedWord.Notes &&
				c.User.Id == _user.Id)), Times.Exactly(1));
		}

        [Test]
        public async Task UpdateWordWithExamplesOfUse_ShouldCallUpdateMethodWithRightArguments()
        {
            var word = await _dal.LexicalUnits.Add(new LexicalUnit()
            {
                Text = "Word #1",
                ExamplesOfUse = new[] { new ExampleOfUse { Example = "Sentence #1" }, new ExampleOfUse { Example = "Sentence #2" } },
                User = _user
            });

            _users.Setup(x => x.GetById(It.IsAny<string>())).ReturnsAsync(_user);
            _words.Setup(x => x.GetByIdWithAccessCheck(It.IsAny<string>(), It.IsAny<int>())).ReturnsAsync(word);
            _words.Setup(x => x.Update(It.IsAny<LexicalUnit>())).ReturnsAsync(new LexicalUnit());

            var updatedWord = new App.Dto.LexicalUnit()
            {
                Id = word.Id,
                ExamplesOfUse = new[] { new ExampleOfUse { Example = "Sentence #1" }, new ExampleOfUse { Example = "Sentence #3" } },
            };

            var category1 = await _service.UpdateAsync(_user.Id, updatedWord);

            _words.Verify(x => x.Update(
                It.Is<LexicalUnit>(c =>
                c.Id == updatedWord.Id &&
                c.ExamplesOfUse != null && 
                c.ExamplesOfUse.Where(example => updatedWord.ExamplesOfUse.Select(example2 => example2.Example).Contains(example.Example)).Count() == 2 &&
                c.User.Id == _user.Id)), Times.Exactly(1));
        }

        [Test]
		public async Task UpdateWordWithDirectRelatedWords_ShouldCallUpdateMethodWithRightArguments()
		{
			var word1 = await _dal.LexicalUnits.Add(new LexicalUnit()
			{
				Text = "Word #1",
				User = _user
			});
			var word2 = await _dal.LexicalUnits.Add(new LexicalUnit()
			{
				Text = "Word #2",
				User = _user
			});
			var word3 = await _dal.LexicalUnits.Add(new LexicalUnit()
			{
				Text = "Word #3",
				User = _user,
			});

			_users.Setup(x => x.GetById(It.IsAny<string>())).ReturnsAsync(_user);
			_words.Setup(x => x.GetByIdWithAccessCheck(It.IsAny<string>(), It.Is<int>(id => id == word1.Id))).ReturnsAsync(word1);
			_words.Setup(x => x.GetByIdWithAccessCheck(It.IsAny<string>(), It.Is<int>(id => id == word2.Id))).ReturnsAsync(word2);
			_words.Setup(x => x.Update(It.IsAny<LexicalUnit>())).ReturnsAsync(new LexicalUnit());

			var updatedWord = new App.Dto.LexicalUnit()
			{
				Id = word1.Id,
				RelatedLexicalUnits = new App.Dto.LexicalUnitRelation[] { new App.Dto.LexicalUnitRelation() { LexicalUnitId = word2.Id } }
            };

			var category1 = await _service.UpdateAsync(_user.Id, updatedWord);

			_words.Verify(x => x.Update(
				It.Is<LexicalUnit>(c => 
				c.Id == updatedWord.Id && 
				c.RelatedLexicalUnits != null && c.RelatedLexicalUnits.Count() > 0 && c.RelatedLexicalUnits[0].RelatedLexicalUnitId == word2.Id && c.RelatedLexicalUnits[0].LexicalUnitId == word1.Id &&
				c.User.Id == _user.Id)), Times.Exactly(1));
		}

		[Test]
		public async Task UpdateWordWithRelatedWordsOf_ShouldCallUpdateMethodWithRightArguments()
		{
			var word1 = await _dal.LexicalUnits.Add(new LexicalUnit()
			{
				Text = "Word #1",
				User = _user
			});
			var word2 = await _dal.LexicalUnits.Add(new LexicalUnit()
			{
				Text = "Word #2",
				User = _user,
			});
			word2.RelatedLexicalUnits = new List<LexicalUnitToLexicalUnitRelation>() { new LexicalUnitToLexicalUnitRelation(word2, word1) };
            await _dal.LexicalUnits.Update(word2);

			var word3 = await _dal.LexicalUnits.Add(new LexicalUnit()
			{
				Text = "Word #3",
				User = _user,
			});

			_users.Setup(x => x.GetById(It.IsAny<string>())).ReturnsAsync(_user);
			_words.Setup(x => x.GetByIdWithAccessCheck(It.IsAny<string>(), It.Is<int>(id => id == word1.Id))).ReturnsAsync(word1);
			_words.Setup(x => x.GetByIdWithAccessCheck(It.IsAny<string>(), It.Is<int>(id => id == word2.Id))).ReturnsAsync(word2);
			_words.Setup(x => x.Update(It.IsAny<LexicalUnit>())).ReturnsAsync(new LexicalUnit());

			var updatedWord = new App.Dto.LexicalUnit()
			{
				Id = word1.Id,
				RelatedLexicalUnits = new App.Dto.LexicalUnitRelation[] { new App.Dto.LexicalUnitRelation() { LexicalUnitId = word2.Id } }
            };

			await _service.UpdateAsync(_user.Id, updatedWord);

			_words.Verify(x => x.Update(
				It.Is<LexicalUnit>(c =>
				c.Id == updatedWord.Id &&
				c.RelatedLexicalUnitsOf != null && c.RelatedLexicalUnitsOf.Count() > 0 && c.RelatedLexicalUnitsOf[0].RelatedLexicalUnitId == word1.Id && c.RelatedLexicalUnitsOf[0].LexicalUnitId == word2.Id &&
				c.User.Id == _user.Id)), Times.Exactly(1));
		}

		[Test]
		public async Task UpdateWordWithComplexRelatedWords_ShouldCallUpdateMethodWithRightArguments()
		{
			var word1 = await _dal.LexicalUnits.Add(new LexicalUnit()
			{
				Text = "Word #1",
				User = _user
			});
			var word2 = await _dal.LexicalUnits.Add(new LexicalUnit()
			{
				Text = "Word #2",
				User = _user
			});

			word1.RelatedLexicalUnits = new List<LexicalUnitToLexicalUnitRelation>() { new LexicalUnitToLexicalUnitRelation(word1, word2) };
            await _dal.LexicalUnits.Update(word1);

			var word3 = await _dal.LexicalUnits.Add(new LexicalUnit()
			{
				Text = "Word #3",
				User = _user,
			});
			word3.RelatedLexicalUnits = new List<LexicalUnitToLexicalUnitRelation>() { new LexicalUnitToLexicalUnitRelation(word3, word1) };
            await _dal.LexicalUnits.Update(word3);

			var word4 = await _dal.LexicalUnits.Add(new LexicalUnit()
			{
				Text = "Word #4",
				User = _user,
			});
			word4.RelatedLexicalUnits = new List<LexicalUnitToLexicalUnitRelation>() { new LexicalUnitToLexicalUnitRelation(word4, word1) };
            await _dal.LexicalUnits.Update(word4);

            var word5 = await _dal.LexicalUnits.Add(new LexicalUnit()
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
			_words.Setup(x => x.Update(It.IsAny<LexicalUnit>())).ReturnsAsync(new LexicalUnit());

			var updatedWord = new App.Dto.LexicalUnit()
			{
				Id = word1.Id,
				RelatedLexicalUnits = new App.Dto.LexicalUnitRelation[] {
                    new App.Dto.LexicalUnitRelation() { LexicalUnitId = word5.Id },
                    new App.Dto.LexicalUnitRelation() { LexicalUnitId = word3.Id }
                }
            };

			await _service.UpdateAsync(_user.Id, updatedWord);

			_words.Verify(x => x.Update(
				It.Is<LexicalUnit>(c =>
				c.Id == updatedWord.Id &&
				c.RelatedLexicalUnits != null && c.RelatedLexicalUnits.Count() > 0 && c.RelatedLexicalUnits[0].LexicalUnitId == word1.Id && c.RelatedLexicalUnits[0].RelatedLexicalUnitId == word5.Id &&
				c.RelatedLexicalUnitsOf != null && c.RelatedLexicalUnitsOf.Count() > 0 && c.RelatedLexicalUnitsOf[0].LexicalUnitId == word3.Id && c.RelatedLexicalUnitsOf[0].RelatedLexicalUnitId == word1.Id &&
				c.User.Id == _user.Id)), Times.Exactly(1));
		}


		[Test]
        public async Task DeleteWord_ShouldCallAddMethodWithRightArguments()
        {
            var word = await _dal.LexicalUnits.Add(new LexicalUnit()
            {
                Text = "Word #1",
                User = _user
            });

            _users.Setup(x => x.GetById(It.IsAny<string>())).ReturnsAsync(_user);
            _words.Setup(x => x.GetByIdWithAccessCheck(It.IsAny<string>(), It.IsAny<int>())).ReturnsAsync(word);
            _words.Setup(x => x.Delete(It.IsAny<LexicalUnit>())).Returns(Task.CompletedTask);

            await _service.DeleteAsync(_user.Id, word.Id);

            _words.Verify(x => x.Delete(
                It.Is<LexicalUnit>(c => c.Text == word.Text && word.User.Id == word.User.Id)),
                Times.Exactly(1));
        }

        [Test]
        public async Task GetWords_ShouldCallGetListMethodWithRightArguments()
        {
            _words.Setup(x => x.GetListWithAccessCheck(It.IsAny<string>())).ReturnsAsync(_user.LexicalUnits.ToList());

            var result = await _service.GetListAsync(_user.Id);

            _words.Verify(x => x.GetListWithAccessCheck(
                    It.Is<string>(c => c == _user.Id)),
                Times.Exactly(1));
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

            var result = await _service.GetListAsync(_user.Id);
            
            Assert.AreEqual(2, result.Count());
        }
    }
}
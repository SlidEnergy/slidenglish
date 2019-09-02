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
            _service = new LexicalUnitsService(_autoMapper.Create(_db), _db);
        }

        [Test]
        public async Task AddWord_ShouldBeAdded()
        { 
            var word = new App.Dto.LexicalUnit()
            {
                Text = "Word #1",
                Association = "Association #1",
                Notes = "Description #1"
            };

            var newWord = await _service.AddAsync(_user.Id, word);

            Assert.NotNull(_db.LexicalUnits.FirstOrDefault(x =>
                x.Id == newWord.Id &&
                x.Text == word.Text &&
                x.Association == word.Association &&
                x.Notes == word.Notes &&
                x.InputAttributes == LexicalUnitInputAttribute.UserInput &&
                x.User.Id == _user.Id));
        }

        [Test]
        public async Task AddWordWithExamplesOfUse_ShouldBeAdded()
        {
            var word = new App.Dto.LexicalUnit()
            {
                Text = "Word #1",
                ExamplesOfUse = new[] { new ExampleOfUse { Example = "Sentence #1" } }
            };

            await _service.AddAsync(_user.Id, word);
        }

        [Test]
        public async Task AddWordWithRelatedWord_ShouldBeAdded()
        {
            var word1 = _db.LexicalUnits.Add(new LexicalUnit()
            {
                Text = "Word #1",
                User = _user
            });
            _db.SaveChanges();

            var word2 = new App.Dto.LexicalUnit()
            {
                Text = "Word #2",
                RelatedLexicalUnits = new[] { new App.Dto.LexicalUnitRelation() { LexicalUnitId = word1.Entity.Id } }
            };

            var newWord = await _service.AddAsync(_user.Id, word2);

            Assert.NotNull(_db.LexicalUnits.FirstOrDefault(c =>
                c.Id == newWord.Id &&
                c.Text == word2.Text &&
                c.RelatedLexicalUnits != null && c.RelatedLexicalUnits.Count() == 1 && c.RelatedLexicalUnits[0].RelatedLexicalUnitId == word1.Entity.Id &&
                c.User.Id == _user.Id));
        }

        [Test]
        public async Task UpdateWord_ShouldBeUpdated()
        {
            var word = _db.LexicalUnits.Add(new LexicalUnit()
            {
                Text = "Word #1",
                User = _user
            }).Entity;
            _db.SaveChanges();

            var updatedWord = new App.Dto.LexicalUnit()
            {
                Id = word.Id,
                Text = "Word #2",
                Association = "Association #2",
                Notes = "Description #2",
            };

            await _service.UpdateAsync(_user.Id, updatedWord);

            Assert.NotNull(_db.LexicalUnits.FirstOrDefault(x=>
                x.Id == updatedWord.Id &&
                x.Text == updatedWord.Text &&
                x.Association == updatedWord.Association &&
                x.Notes == updatedWord.Notes &&
                x.User.Id == _user.Id));
        }

        [Test]
        public async Task UpdateWordWithExamplesOfUse_ShouldBeUpdated()
        {
            var word = _db.LexicalUnits.Add(new LexicalUnit()
            {
                Text = "Word #1",
                ExamplesOfUse = new[] { new ExampleOfUse { Example = "Sentence #1" }, new ExampleOfUse { Example = "Sentence #2" } },
                User = _user
            }).Entity;
            _db.SaveChanges();

            var updatedWord = new App.Dto.LexicalUnit()
            {
                Id = word.Id,
                ExamplesOfUse = new[] { new ExampleOfUse { Example = "Sentence #1" }, new ExampleOfUse { Example = "Sentence #3" } },
            };

            var category1 = await _service.UpdateAsync(_user.Id, updatedWord);

            Assert.NotNull(_db.LexicalUnits.FirstOrDefault(x =>
                x.Id == updatedWord.Id &&
                x.ExamplesOfUse != null &&
                x.ExamplesOfUse.Where(example => updatedWord.ExamplesOfUse.Select(example2 => example2.Example).Contains(example.Example)).Count() == 2 &&
                x.User.Id == _user.Id));
        }

        [Test]
        public async Task UpdateWordWithDirectRelatedWords_ShouldBeUpdated()
        {
            var word1 = _db.LexicalUnits.Add(new LexicalUnit()
            {
                Text = "Word #1",
                User = _user
            }).Entity;
            var word2 = _db.LexicalUnits.Add(new LexicalUnit()
            {
                Text = "Word #2",
                User = _user
            }).Entity;
            var word3 = _db.LexicalUnits.Add(new LexicalUnit()
            {
                Text = "Word #3",
                User = _user,
            }).Entity;
            _db.SaveChanges();

            var updatedWord = new App.Dto.LexicalUnit()
            {
                Id = word1.Id,
                RelatedLexicalUnits = new App.Dto.LexicalUnitRelation[] { new App.Dto.LexicalUnitRelation() { LexicalUnitId = word2.Id } }
            };

            var category1 = await _service.UpdateAsync(_user.Id, updatedWord);

            Assert.NotNull(_db.LexicalUnits.FirstOrDefault(x =>
                x.Id == updatedWord.Id &&
                x.RelatedLexicalUnits != null && x.RelatedLexicalUnits.Count() > 0 && x.RelatedLexicalUnits[0].RelatedLexicalUnitId == word2.Id && x.RelatedLexicalUnits[0].LexicalUnitId == word1.Id &&
                x.User.Id == _user.Id));
        }

        [Test]
        public async Task UpdateWordWithRelatedWordsOf_ShouldBeUpdated()
        {
            var word1 = _db.LexicalUnits.Add(new LexicalUnit()
            {
                Text = "Word #1",
                User = _user
            }).Entity;
            var word2 = _db.LexicalUnits.Add(new LexicalUnit()
            {
                Text = "Word #2",
                User = _user,
            }).Entity;
            _db.SaveChanges();
            word2.RelatedLexicalUnits = new List<LexicalUnitToLexicalUnitRelation>() { new LexicalUnitToLexicalUnitRelation(word2, word1) };
            _db.SaveChanges();

            var updatedWord = new App.Dto.LexicalUnit()
            {
                Id = word1.Id,
                RelatedLexicalUnits = new App.Dto.LexicalUnitRelation[] { new App.Dto.LexicalUnitRelation() { LexicalUnitId = word2.Id } }
            };

            await _service.UpdateAsync(_user.Id, updatedWord);

            Assert.NotNull(_db.LexicalUnits.FirstOrDefault(x =>
                x.Id == updatedWord.Id &&
                x.RelatedLexicalUnitsOf != null && x.RelatedLexicalUnitsOf.Count() > 0 && x.RelatedLexicalUnitsOf[0].RelatedLexicalUnitId == word1.Id && x.RelatedLexicalUnitsOf[0].LexicalUnitId == word2.Id &&
                x.User.Id == _user.Id));
        }

        [Test]
        public async Task UpdateWordWithComplexRelatedWords_ShouldBeUpdated()
        {
            var word1 = _db.LexicalUnits.Add(new LexicalUnit()
            {
                Text = "Word #1",
                User = _user
            }).Entity;
            var word2 = _db.LexicalUnits.Add(new LexicalUnit()
            {
                Text = "Word #2",
                User = _user
            }).Entity;
            _db.SaveChanges();
            word1.RelatedLexicalUnits = new List<LexicalUnitToLexicalUnitRelation>() { new LexicalUnitToLexicalUnitRelation(word1, word2) };
            _db.SaveChanges();

            var word3 = _db.LexicalUnits.Add(new LexicalUnit()
            {
                Text = "Word #3",
                User = _user,
            }).Entity;
            _db.SaveChanges();
            word3.RelatedLexicalUnits = new List<LexicalUnitToLexicalUnitRelation>() { new LexicalUnitToLexicalUnitRelation(word3, word1) };
            _db.SaveChanges();

            var word4 = _db.LexicalUnits.Add(new LexicalUnit()
            {
                Text = "Word #4",
                User = _user,
            }).Entity;
            _db.SaveChanges();
            word4.RelatedLexicalUnits = new List<LexicalUnitToLexicalUnitRelation>() { new LexicalUnitToLexicalUnitRelation(word4, word1) };
            _db.SaveChanges();

            var word5 = _db.LexicalUnits.Add(new LexicalUnit()
            {
                Text = "Word #5",
                User = _user,
            }).Entity;
            _db.SaveChanges();

            var updatedWord = new App.Dto.LexicalUnit()
            {
                Id = word1.Id,
                RelatedLexicalUnits = new App.Dto.LexicalUnitRelation[] {
                    new App.Dto.LexicalUnitRelation() { LexicalUnitId = word5.Id },
                    new App.Dto.LexicalUnitRelation() { LexicalUnitId = word3.Id }
                }
            };

            await _service.UpdateAsync(_user.Id, updatedWord);

            Assert.NotNull(_db.LexicalUnits.FirstOrDefault(x =>
                x.Id == updatedWord.Id &&
                x.RelatedLexicalUnits != null && x.RelatedLexicalUnits.Count() > 0 && x.RelatedLexicalUnits[0].LexicalUnitId == word1.Id && x.RelatedLexicalUnits[0].RelatedLexicalUnitId == word5.Id &&
                x.RelatedLexicalUnitsOf != null && x.RelatedLexicalUnitsOf.Count() > 0 && x.RelatedLexicalUnitsOf[0].LexicalUnitId == word3.Id && x.RelatedLexicalUnitsOf[0].RelatedLexicalUnitId == word1.Id &&
                x.User.Id == _user.Id));
        }


        [Test]
        public async Task DeleteWord_ShouldBeDeleted()
        {
            var word = _db.LexicalUnits.Add(new LexicalUnit()
            {
                Text = "Word #1",
                User = _user
            }).Entity;
            _db.SaveChanges();

            await _service.DeleteAsync(_user.Id, word.Id);

            Assert.Null(_db.LexicalUnits.ByUser(_user.Id).FirstOrDefault(x => x.Id == word.Id));
        }

        [Test]
        public async Task GetWords_ShouldEmptyList()
        {
            var result = await _service.GetListAsync(_user.Id);

            Assert.IsEmpty(result);
        }

        [Test]
        public async Task GetWords_ShouldReturnList()
        {
            _db.LexicalUnits.Add(new LexicalUnit()
            {
                Text = "Word #1",
                User = _user
            });
            _db.LexicalUnits.Add(new LexicalUnit()
            {
                Text = "Word #2",
                User = _user
            });
            _db.SaveChanges();

            var result = await _service.GetListAsync(_user.Id);

            Assert.AreEqual(2, result.Count());
        }
    }
}
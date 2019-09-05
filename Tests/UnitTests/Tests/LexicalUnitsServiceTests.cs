using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using SlidEnglish.App;
using SlidEnglish.Domain;
using SlidEnglish.Web.Tests;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SlidEnglish.Web.UnitTests
{
    public class LexicalUnitsServiceTests : TestsBase
    {
        private LexicalUnitsService _service;
        private EntityFactory _factory;

        [SetUp]
        public void Setup()
        {
            _service = new LexicalUnitsService(_autoMapper.Create(_db), _db);
            _factory = new EntityFactory(_db);
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
                ExamplesOfUse = new[] { new App.Dto.ExampleOfUse { Example = "Sentence #1" } }
            };

            await _service.AddAsync(_user.Id, word);
        }

        [Test]
        public async Task AddWordWithRelatedWord_ShouldBeAdded()
        {
            var word1 = _factory.CreateLexicalUnit(_user);

            var word2 = new App.Dto.LexicalUnit()
            {
                Text = "Word #2",
                RelatedLexicalUnits = new[] { new App.Dto.LexicalUnitRelation() { LexicalUnitId = word1.Id } }
            };

            var newWord = await _service.AddAsync(_user.Id, word2);

            Assert.NotNull(_db.LexicalUnits.FirstOrDefault(c =>
                c.Id == newWord.Id &&
                c.Text == word2.Text &&
                c.RelatedLexicalUnits != null && c.RelatedLexicalUnits.Count() == 1 && c.RelatedLexicalUnits[0].RelatedLexicalUnitId == word1.Id &&
                c.User.Id == _user.Id));
        }

        [Test]
        public async Task UpdateWord_ShouldBeUpdated()
        {
            var word = _factory.CreateLexicalUnit(_user);

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
                Text = Guid.NewGuid().ToString(),
                ExamplesOfUse = new[] {
                    new ExampleOfUse { Example = "Sentence #1" },
                    new ExampleOfUse { Example = "Sentence #2" } },
                User = _user
            }).Entity;

            _db.SaveChanges();

            var updatedWord = new App.Dto.LexicalUnit()
            {
                Id = word.Id,
                ExamplesOfUse = new[] {
                    new App.Dto.ExampleOfUse { Example = "Sentence #1" },
                    new App.Dto.ExampleOfUse { Example = "Sentence #3" }
                },
            };

            var category1 = await _service.UpdateAsync(_user.Id, updatedWord);

            Assert.NotNull(_db.LexicalUnits.FirstOrDefault(x =>
                x.Id == updatedWord.Id &&
                x.ExamplesOfUse != null &&
                x.ExamplesOfUse.Where(example => updatedWord.ExamplesOfUse.Select(example2 => example2.Example).Contains(example.Example)).Count() == 2 &&
                x.User.Id == _user.Id));
        }

        [Test]
        public async Task UpdateWordWithSynonym_ShouldBeUpdated()
        {
            var word1 = _factory.CreateLexicalUnit(_user);
            var word2 = _factory.CreateLexicalUnit(_user);

            var updatedWord = new App.Dto.LexicalUnit()
            {
                Id = word1.Id,
                RelatedLexicalUnits = new [] { new App.Dto.LexicalUnitRelation() { LexicalUnitId = word2.Id, Attribute = RelationAttribute.Synonym } }
            };

            var category1 = await _service.UpdateAsync(_user.Id, updatedWord);

            Assert.NotNull(_db.LexicalUnits.FirstOrDefault(x =>
                x.Id == updatedWord.Id &&
                x.RelatedLexicalUnits != null && x.RelatedLexicalUnits.Count() > 0 && 
                x.RelatedLexicalUnits[0].RelatedLexicalUnitId == word2.Id && x.RelatedLexicalUnits[0].LexicalUnitId == word1.Id &&
                x.RelatedLexicalUnits[0].Attribute == RelationAttribute.Synonym &&
                x.User.Id == _user.Id));
        }

        [Test]
        public async Task UpdateWordWithDirectRelatedWords_ShouldBeUpdated()
        {
            var word1 = _factory.CreateLexicalUnit(_user);
            var word2 = _factory.CreateLexicalUnit(_user);

            var updatedWord = new App.Dto.LexicalUnit()
            {
                Id = word1.Id,
                RelatedLexicalUnits = new [] { new App.Dto.LexicalUnitRelation() { LexicalUnitId = word2.Id } }
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
            var word1 = _factory.CreateLexicalUnit(_user);
            var word2 = _factory.CreateLexicalUnit(_user);

            word2.AddRelatedLexicalUnit(word1);
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
            var word1 = _factory.CreateLexicalUnit(_user);
            var word2 = _factory.CreateLexicalUnit(_user);
            var word3 = _factory.CreateLexicalUnit(_user);
            var word4 = _factory.CreateLexicalUnit(_user);
            var word5 = _factory.CreateLexicalUnit(_user);

            word1.AddRelatedLexicalUnit(word2);
            word3.AddRelatedLexicalUnit(word1);
            word4.AddRelatedLexicalUnit(word1);

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
            var word = _factory.CreateLexicalUnit(_user);

            await _service.DeleteAsync(_user.Id, word.Id);

            Assert.Null(_db.LexicalUnits.ByUser(_user.Id).FirstOrDefault(x => x.Id == word.Id));
        }

        [Test]
        public async Task DeleteWordWithRelatedLexicalUnit_ShouldBeDeleted()
        {
            var word1 = _factory.CreateLexicalUnit(_user);
            var word2 = _factory.CreateLexicalUnit(_user);
            word2.AddRelatedLexicalUnit(word1);
            _db.SaveChanges();

            await _service.DeleteAsync(_user.Id, word1.Id);
            await _service.DeleteAsync(_user.Id, word2.Id);

            Assert.Null(_db.LexicalUnits.ByUser(_user.Id).FirstOrDefault(x => x.Id == word1.Id));
            Assert.Null(_db.LexicalUnits.ByUser(_user.Id).FirstOrDefault(x => x.Id == word2.Id));
        }

        [Test]
        public async Task DeleteWordWithExamplesOfUse_ShouldBeDeleted()
        {
            var word = _factory.CreateLexicalUnit(_user);
            word.ExamplesOfUse.Add(new ExampleOfUse { Example = "Example" });
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
            var word1 = _factory.CreateLexicalUnit(_user);
            var word2 = _factory.CreateLexicalUnit(_user);
            word2.RelatedLexicalUnits = new[] { new LexicalUnitToLexicalUnitRelation(word2, word1) };
            _db.SaveChanges();

            var result = await _service.GetListAsync(_user.Id);

            Assert.AreEqual(2, result.Count());
        }
    }
}
using AutoMapper;
using SlidEnglish.Domain;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace SlidEnglish.App
{
    public class LexicalUnitsService : ILexicalUnitsService
    {
        private IMapper _mapper;
        private readonly IApplicationDbContext _context;

        public LexicalUnitsService(IMapper mapper, IApplicationDbContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<Dto.LexicalUnit> AddAsync(string userId, Dto.LexicalUnit lexicalUnit)
        {
            var dto = lexicalUnit;

            var newLexicalUnit = _mapper.Map<LexicalUnit>(dto);

            newLexicalUnit.User = await _context.Users.FindAsync(userId);
            newLexicalUnit.InputAttributes = LexicalUnitInputAttribute.UserInput;

            if (dto.RelatedLexicalUnits != null && dto.RelatedLexicalUnits.Length > 0)
                await AddRelatedLexicalUnits(userId, newLexicalUnit, dto);

            _context.LexicalUnits.Add(newLexicalUnit);

            await _context.SaveChangesAsync();
            return _mapper.Map<Dto.LexicalUnit>(newLexicalUnit);
        }

        private async Task AddRelatedLexicalUnits(string userId, LexicalUnit newLexicalUnit, Dto.LexicalUnit dto)
        {
            newLexicalUnit.RelatedLexicalUnits = new List<LexicalUnitToLexicalUnitRelation>(dto.RelatedLexicalUnits.Length);

            foreach (var relation in dto.RelatedLexicalUnits)
            {
                var linkedLexicalUnit = await _context.LexicalUnits.ByUser(userId).FindAsync(relation.LexicalUnitId);
                newLexicalUnit.RelatedLexicalUnits.Add(new LexicalUnitToLexicalUnitRelation(newLexicalUnit, linkedLexicalUnit));
            }
        }

        public async Task<Dto.LexicalUnit[]> GetListAsync(string userId)
        {
            var lexicalUnit = await _context.LexicalUnits.ByUser(userId).ToListAsync();

            return _mapper.Map<Dto.LexicalUnit[]>(lexicalUnit);
        }

        public async Task<Dto.LexicalUnit> UpdateAsync(string userId, Dto.LexicalUnit lexicalUnit)
        {
            var dto = lexicalUnit;

            var editLexicalUnit = await _context.LexicalUnits.ByUser(userId).FindAsync(dto.Id);

            _mapper.Map(dto, editLexicalUnit);

            await UpdateRelatedLexicalUnits(userId, editLexicalUnit, dto);
            UpdateExamplesOfUse(editLexicalUnit, dto);

            await _context.SaveChangesAsync();

            return _mapper.Map<Dto.LexicalUnit>(editLexicalUnit);
        }

        public async Task UpdateRelatedLexicalUnits(string userId, LexicalUnit editLexicalUnit, Dto.LexicalUnit dto)
        {
            // вызываем чтобы сработал lazyloading, это позволит потом сохранить это свойство
            var oldRelatedLexicalUnits = editLexicalUnit.RelatedLexicalUnits;
            var oldRelatedLexicalUnitsOf = editLexicalUnit.RelatedLexicalUnitsOf;

            var relatedLexicalUnits = new List<LexicalUnitToLexicalUnitRelation>(dto.RelatedLexicalUnits != null ? dto.RelatedLexicalUnits.Length : 0);
            var relatedLexicalUnitsOf = new List<LexicalUnitToLexicalUnitRelation>(dto.RelatedLexicalUnits != null ? dto.RelatedLexicalUnits.Length : 0);

            // Обновляем список связанных синонимов
            if (dto.RelatedLexicalUnits != null && dto.RelatedLexicalUnits.Length > 0)
            {
                foreach (var relation in dto.RelatedLexicalUnits)
                {
                    if (editLexicalUnit.RelatedLexicalUnitsOf.Any(x => x.LexicalUnitId == relation.LexicalUnitId &&
                        x.RelatedLexicalUnitId == editLexicalUnit.Id))
                    {
                        //var linkedLexicalUnit = await _dal.LexicalUnits.GetByIdWithAccessCheck(userId, relation.LexicalUnitId);
                        var linkedLexicalUnit = await _context.LexicalUnits.ByUser(userId).FirstAsync(x => x.Id == relation.LexicalUnitId);
                        relatedLexicalUnitsOf.Add(new LexicalUnitToLexicalUnitRelation(linkedLexicalUnit, editLexicalUnit, relation.Attribute));
                    }
                    else
                    {
                        //var linkedLexicalUnit = await _dal.LexicalUnits.GetByIdWithAccessCheck(userId, relation.LexicalUnitId);
                        var linkedLexicalUnit = await _context.LexicalUnits.ByUser(userId).FirstAsync(x => x.Id == relation.LexicalUnitId);
                        relatedLexicalUnits.Add(new LexicalUnitToLexicalUnitRelation(editLexicalUnit, linkedLexicalUnit, relation.Attribute));
                    }
                }
            }

            editLexicalUnit.RelatedLexicalUnits = relatedLexicalUnits;
            editLexicalUnit.RelatedLexicalUnitsOf = relatedLexicalUnitsOf;
        }

        public void UpdateExamplesOfUse(LexicalUnit editLexicalUnit, Dto.LexicalUnit dto)
        {
            // вызываем чтобы сработал lazyloading, это позволит потом сохранить это свойство
            var oldExamplesOfUse = editLexicalUnit.ExamplesOfUse;

            var examplesOfUse = new List<ExampleOfUse>(dto.ExamplesOfUse != null ? dto.ExamplesOfUse.Length : 0);

            // Обновляем список связанных синонимов
            if (dto.ExamplesOfUse != null && dto.ExamplesOfUse.Length > 0)
            {
                foreach (var example in dto.ExamplesOfUse)
                {
                    if (editLexicalUnit.ExamplesOfUse.Any(x => x.Example == example.Example))
                    {
                        var exampleToUpdate = editLexicalUnit.ExamplesOfUse.First(x => x.Example == example.Example);
                        exampleToUpdate.Attribute = example.Attribute;
                        examplesOfUse.Add(exampleToUpdate);
                    }
                    else
                    {
                        examplesOfUse.Add(example);
                    }
                }
            }

            editLexicalUnit.ExamplesOfUse = examplesOfUse;
        }

        public async Task DeleteAsync(string userId, int id)
        {
            var lexicalUnit = await _context.LexicalUnits.ByUser(userId).FindAsync(id);

            if (lexicalUnit == null)
                throw new EntityNotFoundException();

            _context.LexicalUnits.Remove(lexicalUnit);
            await _context.SaveChangesAsync();
        }
    }
}

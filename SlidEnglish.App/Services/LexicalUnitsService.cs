using AutoMapper;
using SlidEnglish.Domain;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace SlidEnglish.App
{
	public class LexicalUnitsService
	{
		private IDataAccessLayer _dal;
		private IMapper _mapper;
        private readonly IApplicationDbContext _context;

        public LexicalUnitsService(IDataAccessLayer dal, IMapper mapper, IApplicationDbContext context)
		{
			_dal = dal;
			_mapper = mapper;
            _context = context;
        }

        public async Task<Dto.LexicalUnit> AddAsync(string userId, Dto.LexicalUnit lexicalUnit)
		{
            var dto = lexicalUnit;

			var newLexicalUnit = _mapper.Map<LexicalUnit>(dto);

			newLexicalUnit.User = await _dal.Users.GetById(userId);
            newLexicalUnit.InputAttributes = LexicalUnitInputAttribute.UserInput;

            if (dto.RelatedLexicalUnits != null && dto.RelatedLexicalUnits.Length > 0)
                await AddRelatedLexicalUnits(userId, newLexicalUnit, dto);

            await _dal.LexicalUnits.Add(newLexicalUnit);

			return _mapper.Map<Dto.LexicalUnit>(newLexicalUnit);
		}

        private async Task AddRelatedLexicalUnits(string userId, LexicalUnit newLexicalUnit, Dto.LexicalUnit dto)
        {
            newLexicalUnit.RelatedLexicalUnits = new List<LexicalUnitToLexicalUnitRelation>(dto.RelatedLexicalUnits.Length);

            foreach (var relation in dto.RelatedLexicalUnits)
            {
                var linkedLexicalUnit = await _dal.LexicalUnits.GetByIdWithAccessCheck(userId, relation.LexicalUnitId);
                newLexicalUnit.RelatedLexicalUnits.Add(new LexicalUnitToLexicalUnitRelation(newLexicalUnit, linkedLexicalUnit));
            }
        }

		public Task<LexicalUnit> GetAsync(string userId, int id)
		{
			return _dal.LexicalUnits.GetByIdWithAccessCheck(userId, id);
		}

        public Task<LexicalUnit> GetAsync(string userId, string text)
        {
            return _dal.LexicalUnits.GetByTextWithAccessCheck(userId, text);
        }

        public async Task<Dto.LexicalUnit[]> GetListAsync(string userId)
		{
			var lexicalUnit = await _dal.LexicalUnits.GetListWithAccessCheck(userId);

			return _mapper.Map<Dto.LexicalUnit[]>(lexicalUnit);
		}

		public async Task<Dto.LexicalUnit> UpdateAsync(string userId, Dto.LexicalUnit lexicalUnit)
		{
            var dto = lexicalUnit;

			var editLexicalUnit = await _dal.LexicalUnits.GetByIdWithAccessCheck(userId, dto.Id);

            _mapper.Map<Dto.LexicalUnit, LexicalUnit>(dto, editLexicalUnit);

            await UpdateRelatedLexicalUnits(userId, editLexicalUnit, dto);
            UpdateExamplesOfUse(editLexicalUnit, dto);

            await _dal.LexicalUnits.Update(editLexicalUnit);

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
                        var linkedLexicalUnit = await _dal.LexicalUnits.GetByIdWithAccessCheck(userId, relation.LexicalUnitId);
                        relatedLexicalUnitsOf.Add(new LexicalUnitToLexicalUnitRelation(linkedLexicalUnit, editLexicalUnit));
                    }
                    else
                    {
                        var linkedLexicalUnit = await _dal.LexicalUnits.GetByIdWithAccessCheck(userId, relation.LexicalUnitId);
                        relatedLexicalUnits.Add(new LexicalUnitToLexicalUnitRelation(editLexicalUnit, linkedLexicalUnit));
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
                        examplesOfUse.Add(_mapper.Map(example, editLexicalUnit.ExamplesOfUse.First(x => x.Example == example.Example)));
                    }
                    else
                    {
                        examplesOfUse.Add(example);
                    }
                }
            }

            editLexicalUnit.ExamplesOfUse = examplesOfUse;
        }

        public async Task<bool> ExistsAsync(string userId, string lexicalUnit) => await _dal.LexicalUnits.GetByTextWithAccessCheck(userId, lexicalUnit) != null;

        public async Task<bool> ExistsAsync(string userId, LexicalUnit lexicalUnit) => await _dal.LexicalUnits.GetByIdWithAccessCheck(userId, lexicalUnit.Id) != null;

		public async Task DeleteAsync(string userId, int id)
		{
			var lexicalUnit = await _dal.LexicalUnits.GetByIdWithAccessCheck(userId, id);

			if (lexicalUnit == null)
				throw new EntityNotFoundException();

			await _dal.LexicalUnits.Delete(lexicalUnit);
		}
	}
}

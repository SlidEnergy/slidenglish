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

		public LexicalUnitsService(IDataAccessLayer dal, IMapper mapper)
		{
			_dal = dal;
			_mapper = mapper;
		}

        public async Task<Dto.LexicalUnit> AddAsync(string userId, Dto.LexicalUnit lexicalUnit)
		{
			var newLexicalUnit = _mapper.Map<LexicalUnit>(lexicalUnit);

			newLexicalUnit.User = await _dal.Users.GetById(userId);
            newLexicalUnit.InputAttributes = LexicalUnitInputAttribute.UserInput;

			if (lexicalUnit.RelatedLexicalUnits != null && lexicalUnit.RelatedLexicalUnits.Length > 0)
			{
				newLexicalUnit.RelatedLexicalUnits = new List<LexicalUnitToLexicalUnitRelation>(lexicalUnit.RelatedLexicalUnits.Length);

				foreach (var relation in lexicalUnit.RelatedLexicalUnits)
				{
					var linkedLexicalUnit = await _dal.LexicalUnits.GetByIdWithAccessCheck(userId, relation.LexicalUnitId);
					newLexicalUnit.RelatedLexicalUnits.Add(new LexicalUnitToLexicalUnitRelation(newLexicalUnit, linkedLexicalUnit));
				}
			}

			await _dal.LexicalUnits.Add(newLexicalUnit);

			return _mapper.Map<Dto.LexicalUnit>(newLexicalUnit);
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
			var editLexicalUnit = await _dal.LexicalUnits.GetByIdWithAccessCheck(userId, lexicalUnit.Id);
			// вызываем чтобы сработал lazyloading, это позволит потом сохранить это свойство
			var oldRelatedLexicalUnits = editLexicalUnit.RelatedLexicalUnits;
			var oldRelatedLexicalUnitsOf = editLexicalUnit.RelatedLexicalUnitsOf;

			_mapper.Map<Dto.LexicalUnit, LexicalUnit>(lexicalUnit, editLexicalUnit);

			var relatedLexicalUnits = new List<LexicalUnitToLexicalUnitRelation>(lexicalUnit.RelatedLexicalUnits != null ? lexicalUnit.RelatedLexicalUnits.Length : 0);
			var relatedLexicalUnitsOf = new List<LexicalUnitToLexicalUnitRelation>(lexicalUnit.RelatedLexicalUnits != null ? lexicalUnit.RelatedLexicalUnits.Length : 0);

			// Обновляем список связанных синонимов
			if (lexicalUnit.RelatedLexicalUnits != null && lexicalUnit.RelatedLexicalUnits.Length > 0)
			{
				foreach (var relation in lexicalUnit.RelatedLexicalUnits)
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

			await _dal.LexicalUnits.Update(editLexicalUnit);

			return _mapper.Map<Dto.LexicalUnit>(editLexicalUnit);
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

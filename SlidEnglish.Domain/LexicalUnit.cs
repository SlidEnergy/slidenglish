using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace SlidEnglish.Domain
{
    /// <summary>
    /// Лексическая единица (слово, частица, предлог, артикль)
    /// </summary>
	public class LexicalUnit : IUniqueObject
	{
		public int Id { get; set; }

		[Required]
        public string Text { get; set; }

		[Required]
		public virtual User User { get; set; }

		/// <summary>
		/// Описание, Этимология, ссылки на ресурсы
		/// </summary>
		[Required(AllowEmptyStrings = true)]
		public string Notes { get; set; } = "";

		[Required(AllowEmptyStrings = true)]
		public string Association { get; set; } = "";

		public virtual IList<LexicalUnitToLexicalUnitRelation> RelatedLexicalUnits { get; set; } = new List<LexicalUnitToLexicalUnitRelation>();
		public virtual IList<LexicalUnitToLexicalUnitRelation> RelatedLexicalUnitsOf { get; set; } = new List<LexicalUnitToLexicalUnitRelation>();

        public int UsagesCount { get; set; }

        public LexicalUnitInputAttribute InputAttributes { get; set; }

        public PartOfSpeech PartOfSpeech { get; set; }

        public virtual IList<ExampleOfUse> ExamplesOfUse { get; set; } = new List<ExampleOfUse>();

        [NotMapped]
		public ICollection<LexicalUnitRelation> AllRelatedLexicalUnits => RelatedLexicalUnits
            .Select(x => new LexicalUnitRelation(x.LexicalUnit, x.Attribute))
            .Union(RelatedLexicalUnitsOf.Select(x => new LexicalUnitRelation(x.LexicalUnit, x.Attribute))).ToList();

		public bool IsBelongsTo(string userId) => User.Id == userId;

        public bool IsWord => PartOfSpeech == PartOfSpeech.Noun || PartOfSpeech == PartOfSpeech.Verb || PartOfSpeech == PartOfSpeech.Adjective;

        public bool IsPhrase => Text.Split(' ').Length > 1;

        public LexicalUnit() { }

        public void AddRelatedLexicalUnit(LexicalUnit relatedLexicalUnit)
        {
            if (Id <= 0)
                throw new InvalidOperationException();

            RelatedLexicalUnits.Add(new LexicalUnitToLexicalUnitRelation(this, relatedLexicalUnit));
        }
	}
}

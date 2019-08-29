using SlidEnglish.Domain;
using System.ComponentModel.DataAnnotations;

namespace SlidEnglish.App.Dto
{
    public class LexicalUnit
	{
		public int Id { get; set; }

		[Required]
		public string Text { get; set; }

		public string Association { get; set; }

		public string Notes { get; set; }

		public LexicalUnitRelation[] RelatedLexicalUnits { get; set; }

        public LexicalUnitInputAttribute InputAttributes { get; set; }

        public int UsagesCount { get; set; }

        public PartOfSpeech PartOfSpeech { get; set; }

        public ExampleOfUse[] ExamplesOfUse { get; set; }

        public bool IsWord { get; set; }

        public bool IsPhrase { get; set; }

    }
}

using SlidEnglish.Domain;

namespace SlidEnglish.App.Dto
{
	public class LexicalUnitRelation
	{
		public int LexicalUnitId { get; set; }

        public RelationAttribute Attribute { get; set; }
    }
}

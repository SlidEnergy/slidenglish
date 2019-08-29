namespace SlidEnglish.Domain
{
	public class LexicalUnitRelation
	{
		public LexicalUnit LexicalUnit { get; set; }

        public RelationAttribute Attribute { get; set; }

        public LexicalUnitRelation(LexicalUnit lexicalUnit, RelationAttribute attribute)
        {
            LexicalUnit = lexicalUnit;
            Attribute = attribute;
        }
    }
}

namespace SlidEnglish.Domain
{
	public class LexicalUnitToLexicalUnitRelation
	{
		public int LexicalUnitId { get; set; }

		public virtual LexicalUnit LexicalUnit { get; set; }

		public int RelatedLexicalUnitId { get; set; }
		public virtual LexicalUnit RelatedLexicalUnit { get; set; }
        
        public RelationAttribute Attribute { get; set; }

		public LexicalUnitToLexicalUnitRelation() { }

		public LexicalUnitToLexicalUnitRelation(LexicalUnit lexicalUnit, LexicalUnit relatedLecicalUnit)
		{
			LexicalUnitId = lexicalUnit.Id;
			LexicalUnit = lexicalUnit;
			RelatedLexicalUnitId = relatedLecicalUnit.Id;
			LexicalUnit = relatedLecicalUnit;
		}
	}
}

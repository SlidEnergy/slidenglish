namespace SlidEnglish.Domain
{
	public class WordSynonym
	{
		public int WordId { get; set; }
		public virtual Word Word { get; set; }

		public int SynonymId { get; set; }
		public virtual Word Synonym { get; set; }

		public WordSynonym() { }

		public WordSynonym(Word word, Word synonym)
		{
			WordId = word.Id;
			Word = word;
			SynonymId = synonym.Id;
			Synonym = synonym;
		}
	}
}

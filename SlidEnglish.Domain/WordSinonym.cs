namespace SlidEnglish.Domain
{
	public class WordSinonym
	{
		public int WordId { get; set; }
		public virtual Word Word { get; set; }

		public int SinonymId { get; set; }
		public virtual Word Sinonym { get; set; }

		public WordSinonym() { }

		public WordSinonym(Word word, Word synonym)
		{
			WordId = word.Id;
			Word = word;
			SinonymId = synonym.Id;
			Sinonym = synonym;
		}
	}
}

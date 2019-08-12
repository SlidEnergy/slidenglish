namespace SlidEnglish.Domain
{
	public class WordSinonym
	{
		public int WordId { get; set; }
		public virtual Word Word { get; set; }

		public int SinonymId { get; set; }
		public virtual Word Sinonym { get; set; }
	}
}

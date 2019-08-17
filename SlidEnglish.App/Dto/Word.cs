using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SlidEnglish.App.Dto
{
	public class Word
	{
		public int Id { get; set; }

		[Required]
		public string Text { get; set; }

		public string Association { get; set; }

		public string Description { get; set; }

		public int[] Synonyms { get; set; }
	}
}

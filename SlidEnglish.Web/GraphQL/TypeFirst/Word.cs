using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SlidEnglish.Web.Graphql
{
	public class WordGraphql
	{
		public int Id { get; set; }

		[Required]
		public string Text { get; set; }

		public string Association { get; set; }

		public string Description { get; set; }

		public WordGraphql[] Synonyms { get; set; }
	}
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SlidEnglish.App.Dto
{
	public class EditWordDto
	{
		public int Id { get; set; }

		[Required]
		public string Text { get; set; }

		public string Association { get; set; }

		public string Description { get; set; }

		public EditSynonymDto[] Synonyms { get; set; }
	}

	public class EditSynonymDto
	{
		public int? Id { get; set; }

		public string Text { get; set; }
	}
}

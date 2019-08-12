using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SlidEnglish.Web.Dto
{
	public class DetailsWord
	{
		public int Id { get; set; }

		[Required]
		public string Text { get; set; }

		public string Association { get; set; }

		public string Description { get; set; }

		public List<Dto.Word> Sinonyms { get; set; }
	}
}

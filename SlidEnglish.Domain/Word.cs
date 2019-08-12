using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace SlidEnglish.Domain
{
	public class Word : IUniqueObject
	{
		public int Id { get; set; }

		[Required]
		public string Text { get; set; }

		[Required]
		public virtual User User { get; set; }

		/// <summary>
		/// Описание, Этимология, ссылки на ресурсы
		/// </summary>
		[Required(AllowEmptyStrings = true)]
		public string Description { get; set; }

		[Required(AllowEmptyStrings = true)]
		public string Association { get; set; }

		public virtual ICollection<WordSinonym> Sinonyms{ get; set; }
		public virtual ICollection<WordSinonym> SinonymOf { get; set; }

		[NotMapped]
		public ICollection<Word> AllSinonyms => Sinonyms.Select(x => x.Sinonym).Union(SinonymOf.Select(x => x.Word)).ToList();

		public bool IsBelongsTo(string userId) => User.Id == userId;
	}
}

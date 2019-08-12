using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SlidEnglish.Domain
{
	public class User : IdentityUser, IUniqueObject<string>
	{
		[Required]
		public virtual ICollection<Word> Words { get; set; } = new List<Word>();
	}
}

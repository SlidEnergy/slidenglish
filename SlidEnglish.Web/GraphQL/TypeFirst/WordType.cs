using GraphQL.Types;
using SlidEnglish.App.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SlidEnglish.Web.Graphql
{
	public class WordType : ObjectGraphType<Word>
	{
		public WordType()
		{
			Name = "Word";

			Field(x => x.Id, type: typeof(IntGraphType));
			Field(x => x.Text);
			Field(x => x.Association);
			Field(x => x.Description);
			Field(x => x.Synonyms);
		}
	}
}

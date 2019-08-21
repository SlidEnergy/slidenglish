using GraphQL.Types;
using SlidEnglish.App;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SlidEnglish.Web.GraphQL
{
	public class WordQuery : ObjectGraphType
	{
		public WordQuery(WordsService wordsService, string userId)
		{
			Field<ListGraphType<WordType>>("Words", resolve: context => wordsService.GetListAsync(userId));
		}
	}
}

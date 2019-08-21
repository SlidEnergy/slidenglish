using GraphQL.Types;
using SlidEnglish.App.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SlidEnglish.Web.Graphql
{
	public class WordInputType : InputObjectGraphType
	{
		public WordInputType()
		{
			Name = "wordInput";

			Field<IntGraphType>("id");
			Field<NonNullGraphType<StringGraphType>>("text");
			Field<NonNullGraphType<StringGraphType>>("association");
			Field<NonNullGraphType<StringGraphType>>("description");
			Field<NonNullGraphType<ListGraphType<IntGraphType>>>("synonyms");
		}
	}
}

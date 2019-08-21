using GraphQL.Types;
using SlidEnglish.App;
using SlidEnglish.App.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SlidEnglish.Web.Graphql
{
	public class WordMutation : ObjectGraphType
	{
		public WordMutation(WordsService wordsService, string userId)
		{
			Field<WordType>(
				"addWord",
				arguments: new QueryArguments(new QueryArgument<NonNullGraphType<WordInputType>> { Name = "word" }),
				resolve: context =>
				{
					return wordsService.AddAsync(userId, context.GetArgument<Word>("word"));
					});

			Field<WordType>(
				"updateWord",
				arguments: new QueryArguments(new QueryArgument<NonNullGraphType<WordInputType>> { Name = "word" }),
				resolve: context =>
				{

					return wordsService.EditAsync(userId, context.GetArgument<Word>("word"));
				});

			FieldAsync<BooleanGraphType>(
				"deleteWord",
				arguments: new QueryArguments(new QueryArgument<NonNullGraphType<IntGraphType>> { Name = "id" }),
				resolve: async context =>
				{
					await wordsService.DeleteAsync(userId, context.GetArgument<int>("id"));
					return true;
				});
		}
	}
}

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
					var newWord = Task.Run(async () => await wordsService.AddAsync(userId, context.GetArgument<Word>("word"))).Result;
                    var words = Task.Run(async () => await wordsService.GetListAsync(userId)).Result;
                    //var words = await wordsService.GetListAsync(userId);
                    return new WordGraphql
                    {
                        Id = newWord.Id,
                        Text = newWord.Text,
                        Association = newWord.Association,
                        Description = newWord.Description,
                        Synonyms = words.Where(w => newWord.Synonyms.Contains(w.Id)).Select(w => new WordGraphql
                        {
                            Id = w.Id,
                            Text = w.Text,
                            Association = w.Association,
                            Description = w.Description
                        }).ToArray()
                    };
                });

			Field<WordType>(
				"updateWord",
				arguments: new QueryArguments(new QueryArgument<NonNullGraphType<WordInputType>> { Name = "word" }),
				resolve: context =>
				{

					var newWord = Task.Run(async () => await wordsService.UpdateAsync(userId, context.GetArgument<Word>("word"))).Result;
                    var words = Task.Run(async () => await wordsService.GetListAsync(userId)).Result;
                    //var words = await wordsService.GetListAsync(userId);
                    return new WordGraphql
                    {
                        Id = newWord.Id,
                        Text = newWord.Text,
                        Association = newWord.Association,
                        Description = newWord.Description,
                        Synonyms = words.Where(w => newWord.Synonyms.Contains(w.Id)).Select(w => new WordGraphql
                        {
                            Id = w.Id,
                            Text = w.Text,
                            Association = w.Association,
                            Description = w.Description
                        }).ToArray()
                    };
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

using Microsoft.Extensions.DependencyInjection;
using SlidEnglish.App;

namespace SlidEnglish.Web
{
	public static class SlidEnglishAppExtensions
	{
		public static void AddSlidEnglishCore(this IServiceCollection services)
		{
			services.AddScoped<IUsersService, UsersService>();
			services.AddScoped<ILexicalUnitsService, LexicalUnitsService>();
            services.AddScoped<ITranslateService, TranslateService>();
            services.AddScoped<IImportService, ImportService>();
		}
	}
}

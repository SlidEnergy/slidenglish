using Microsoft.Extensions.DependencyInjection;
using SlidEnglish.App;
using SlidEnglish.Domain;
using SlidEnglish.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SlidEnglish.Web
{
	public static class SlidEnglishServicesExtensions
	{
		public static void AddSlidEnglishServices(this IServiceCollection services)
		{
            services.AddScoped<IApplicationDbContext>(provider => provider.GetService<ApplicationDbContext>());

            services.AddScoped<ITokenGenerator, TokenGenerator>();

            services.AddScoped<IUsersService, UsersService>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<ILexicalUnitsService, LexicalUnitsService>();
            services.AddScoped<ITranslateService, TranslateService>();
            services.AddScoped<IImportService, ImportService>();

            services.AddScoped<ITranslator, GoogleTranslator>();
		}
	}
}

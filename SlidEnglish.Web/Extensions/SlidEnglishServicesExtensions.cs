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
            services.AddScoped<ITokenGenerator, TokenGenerator>();

            services.AddScoped<IRepository<User, string>, EfRepository<User, string>>();
			services.AddScoped<IWordsRepository, EfWordsRepository>();
            services.AddScoped<IRefreshTokensRepository, EfRefreshTokensRepository>();

            services.AddScoped<UsersService>();
            services.AddScoped<TokenService>();
            services.AddScoped<WordsService>();
            services.AddScoped<TranslateService>();

            services.AddScoped<IDataAccessLayer, DataAccessLayer>();
		}
	}
}

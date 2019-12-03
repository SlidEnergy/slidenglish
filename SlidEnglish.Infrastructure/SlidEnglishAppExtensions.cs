using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SlidEnglish.App;
using SlidEnglish.Infrastructure;

namespace SlidEnglish.Infrastructure
{
	public static class SlidEnglishAppExtensions
	{
		public static void AddSlidEnglishInfrastructure(this IServiceCollection services, string connectionString)
		{
			services.AddEntityFrameworkNpgsql()
				.AddDbContext<ApplicationDbContext>(options => options
					.UseLazyLoadingProxies()
					.UseNpgsql(connectionString))
				.BuildServiceProvider();

			services.AddScoped<IApplicationDbContext>(provider => provider.GetService<ApplicationDbContext>());
			services.AddScoped<ITranslator, GoogleTranslator>();
		}
	}
}

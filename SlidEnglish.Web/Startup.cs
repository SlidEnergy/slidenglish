using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SlidEnglish.Infrastructure;
using AutoMapper;
using SlidEnglish.App;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using SlidEnglish.Domain;
using Swashbuckle.AspNetCore.Swagger;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Google.Apis.Auth.OAuth2;
using System.IO;

namespace SlidEnglish.Web
{
	public class Startup
	{
		public IConfiguration Configuration { get; }

		private IHostingEnvironment CurrentEnvironment { get; }

		private string ConnectionString
		{
			get
			{
				if (CurrentEnvironment.IsProduction())
					return Environment.GetEnvironmentVariable("CONNECTION_STRING");

				return Configuration.GetConnectionString(Environment.MachineName) ??
					Configuration.GetConnectionString("DefaultConnection");
			}
		}
		
		private AuthSettings AuthSettings
		{
			get
			{
				if (CurrentEnvironment.IsDevelopment())
				{
					return Configuration
						.GetSection("Security")
						.GetSection("Token")
						.Get<AuthSettings>();
				}
				else
				{
					return new AuthSettings
					{
						Audience = Environment.GetEnvironmentVariable("TOKEN_AUDIENCE"),
						Issuer = Environment.GetEnvironmentVariable("TOKEN_ISSUER"),
						Key = Environment.GetEnvironmentVariable("TOKEN_KEY"),
						LifetimeMinutes = Convert.ToInt32(Environment.GetEnvironmentVariable("TOKEN_LIFETIME_MINUTES"))
					};
				}
			}
		}

        private GoogleCredential GoogleCredential
        {
            get
            {
                if (CurrentEnvironment.IsDevelopment())
                    return GoogleCredential.FromFile(Path.Combine(CurrentEnvironment.ContentRootPath, "..", "google-translate-secret.json"));
                else
                    return GoogleCredential.FromJson(Environment.GetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS"));
            }
        }

        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
			Configuration = configuration;
			CurrentEnvironment = env;
		}

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			services
				.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
				.AddJwtBearer(options =>
				{
					options.RequireHttpsMetadata = false;
					options.TokenValidationParameters = new TokenValidationParameters
					{
						// Укзывает, будет ли проверяться издатель при проверке токена
						ValidateIssuer = false,
						// Строка, представляющая издателя
						ValidIssuer = AuthSettings.Issuer,

						// Будет ли проверяться потребитель токена
						ValidateAudience = false,
						// Установка потребителя токена
						ValidAudience = AuthSettings.Audience,
						// будет ли валидироваться время существования
						ValidateLifetime = true,

						// установка ключа безопасности
						IssuerSigningKey = AuthSettings.GetSymmetricSecurityKey(),
						// валидация ключа безопасности
						ValidateIssuerSigningKey = true,
					};
					// options.SaveToken = true;
				});

			// AutoMapper
			services.AddScoped(provider => new MapperConfiguration(cfg =>
			{
				cfg.AddProfile(new MappingProfile(provider.GetService<ApplicationDbContext>()));
			}).CreateMapper());

			services.AddCors();
			services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

			services.AddEntityFrameworkNpgsql()
				.AddDbContext<ApplicationDbContext>(options => options
					.UseLazyLoadingProxies()
					.UseNpgsql(ConnectionString))
				.BuildServiceProvider();

			// Register the Swagger generator, defining 1 or more Swagger documents
			services.AddSwaggerGen(c =>
			{
				c.SwaggerDoc("v1", new Info { Title = "SlidEnglish", Version = "v1" });
				c.AddSecurityDefinition("Oauth2", new OAuth2Scheme
				{
					Type = "oauth2",
					Flow = "implicit",
					TokenUrl = "/api/v1/tokens"
				});
				c.AddSecurityDefinition("Bearer", new ApiKeyScheme { In = "header", Description = "Please enter JWT with Bearer into field", Name = "Authorization", Type = "apiKey" });

                c.DescribeAllEnumsAsStrings();

				c.OperationFilter<ResponseWithDescriptionOperationFilter>();
				c.OperationFilter<SecurityRequirementsOperationFilter>();

                c.SchemaFilter<EnumAsModelSchemaFilter>();
            });

			// AddIdentity и AddDefaultIdentity добавляют много чего лишнего. Ссылки для сранения.
			// https://github.com/aspnet/Identity/blob/c7276ce2f76312ddd7fccad6e399da96b9f6fae1/src/Core/IdentityServiceCollectionExtensions.cs
			// https://github.com/aspnet/Identity/blob/c7276ce2f76312ddd7fccad6e399da96b9f6fae1/src/Identity/IdentityServiceCollectionExtensions.cs
			// https://github.com/aspnet/Identity/blob/c7276ce2f76312ddd7fccad6e399da96b9f6fae1/src/UI/IdentityServiceCollectionUIExtensions.cs#L49
			services.AddIdentityCore<User>()
				.AddEntityFrameworkStores<ApplicationDbContext>();

            services.AddSingleton(x => AuthSettings);
            services.AddSingleton(x => GoogleCredential);

            services.AddSlidEnglishServices();
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IHostingEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}
			else
			{
				// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
				app.UseHsts();
			}

			app.UseCors(x => x
			   .AllowAnyOrigin()
			   .AllowAnyMethod()
			   .AllowAnyHeader());
			//.AllowCredentials());

			app.UseDefaultFiles();
			app.UseStaticFiles();
			app.UseAuthentication();

			if (env.IsProduction())
			{ 
				app.UseHttpsRedirection();
			}

			// Enable middleware to serve generated Swagger as a JSON endpoint.
			app.UseSwagger(c =>
			{

			});

			// Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), 
			// specifying the Swagger JSON endpoint.
			app.UseSwaggerUI(c =>
			{
				c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
			});

			app.UseMvc();
		}
	}
}

using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SlidEnglish.Web;
using SlidEnglish.Infrastructure;
using NUnit.Framework;

namespace SlidEnglish.Web.UnitTests
{
    public class AutoMapperTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void CreateMapperProfile_Validated()
        {
            var context = new ApplicationDbContext(new DbContextOptions<ApplicationDbContext>());
			var config = new MapperConfiguration(cfg => {
				cfg.AddProfile(new MappingProfile(context));
			});
			config.AssertConfigurationIsValid();
        }
    }
}
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
            Mapper.Initialize(x=>x.AddProfile(new MappingProfile(context)));
            Mapper.Configuration.AssertConfigurationIsValid();
        }
    }
}
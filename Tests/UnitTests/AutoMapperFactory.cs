using AutoMapper;
using SlidEnglish.Web;
using SlidEnglish.Infrastructure;

namespace SlidEnglish.Web.UnitTests
{
    public class AutoMapperFactory
    {
        public IMapper Create(ApplicationDbContext context)
        {
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(new MappingProfile(context)));
            return new Mapper(configuration);
        }
    }
}

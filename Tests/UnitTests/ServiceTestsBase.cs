using Microsoft.EntityFrameworkCore;
using Moq;
using SlidEnglish.App;
using SlidEnglish.Infrastructure;
using NUnit.Framework;
using System.Threading.Tasks;
using SlidEnglish.Domain;
using System.Linq;
using System.Collections.Generic;

namespace SlidEnglish.Web.UnitTests
{
    public class TestsBase
    {
        protected readonly AutoMapperFactory _autoMapper = new AutoMapperFactory();
        protected ApplicationDbContext _db;
        protected User _user;

        [SetUp]
        public void SetupBase()
        {
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            optionsBuilder.UseInMemoryDatabase(TestContext.CurrentContext.Test.Name);
            _db = new ApplicationDbContext(optionsBuilder.Options);

            _user = _db.Users.Add(new User() { Email = "test1@email.com" }).Entity;
            _db.SaveChanges();
        }
    }
}

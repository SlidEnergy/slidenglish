using Microsoft.EntityFrameworkCore;
using Moq;
using SlidEnglish.App;
using SlidEnglish.Infrastructure;
using NUnit.Framework;
using System.Threading.Tasks;
using SlidEnglish.Domain;

namespace SlidEnglish.Web.UnitTests
{
    public class TestsBase
    {
        protected readonly AutoMapperFactory _autoMapper = new AutoMapperFactory();
        protected ApplicationDbContext _db;
        protected DataAccessLayer _dal;
        protected DataAccessLayer _mockedDal;
        protected User _user;

        protected Mock<IRepositoryWithAccessCheck<Word>> _words;
        protected Mock<IRepository<User, string>> _users;
		protected Mock<IRefreshTokensRepository> _refreshTokens;

        [SetUp]
        public async Task SetupBase()
        {
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            optionsBuilder.UseInMemoryDatabase(TestContext.CurrentContext.Test.Name);
            _db = new ApplicationDbContext(optionsBuilder.Options);
            _dal = new DataAccessLayer(
                _db,
                new EfRepository<User, string>(_db),
                new EfWordsRepository(_db),
				new EfRefreshTokensRepository(_db));

            _words = new Mock<IRepositoryWithAccessCheck<Word>>();
            _users = new Mock<IRepository<User, string>>();
			_refreshTokens = new Mock<IRefreshTokensRepository>();

			_mockedDal = new DataAccessLayer(_db, _users.Object, _words.Object, _refreshTokens.Object);

            _user = await _dal.Users.Add(new User() { Email = "test1@email.com" });
        }
    }
}

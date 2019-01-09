using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace EfMajor.Test
{
    public static class BasicUow
    {
        /// <summary>
        /// Gets a test Unit of Work
        /// </summary>
        public static TestUnitOfWork Uow
        {
            get
            {
                var dbContext = DbContext;
                var repositoryFactory = new RepositoryFactory();
                repositoryFactory.Initialize(dbContext);

                return new TestUnitOfWork(dbContext, repositoryFactory);
            }
        }

        /// <summary>
        /// Gets a test DB context
        /// </summary>
        private static TestDbContext DbContext
        {
            get
            {
                var connection = new SqliteConnection("DataSource=:memory:");
                connection.Open();
                var options = new DbContextOptionsBuilder<TestDbContext>()
                    .UseSqlite(connection)
                    .Options;
                var dbContext = new TestDbContext(connection, options);
                dbContext.Database.EnsureCreated();

                return dbContext;
            }
        }
    }

    /// <summary>
    /// Test DB Context uses the in-memory Sqlite DB for testing
    /// </summary>
    public class TestDbContext : DbContext
    {
        private SqliteConnection _connection;

        public TestDbContext(SqliteConnection connection, DbContextOptions<TestDbContext> options) : base(options)
        {
            _connection = connection;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var mb = modelBuilder;
            mb.Entity<Customer>();
            mb.Entity<Address>();
            mb.Entity<CustomerAddress>().HasKey(x => new { x.CustomerId, x.AddressId });

            //mb.Entity<Customer>().HasMany(c => c.CustomerAddresses).WithOne().HasForeignKey(c => c.CustomerId);
        }

        /// <summary>
        ///  For testing token cancellation
        /// </summary>
        public override async Task<int> SaveChangesAsync(CancellationToken token)
        {
            await Task.Delay(1000);
            return await base.SaveChangesAsync(token);
        }

        public override void Dispose()
        {
            _connection.Close();
            base.Dispose();
        }
    }

    /// <summary>
    /// This implements the UOW with the test DB Context, exposing an IQueryable for each DB Set entity
    /// </summary>
    public class TestUnitOfWork : UnitOfWork
    {
        public TestUnitOfWork(TestDbContext context, IRepositoryFactory repositoryFactory) : base(context, repositoryFactory)
        {
        }

        public IQueryable<Customer> Customers => EntityQuery<Customer>();

        public IQueryable<Address> Addresses => EntityQuery<Address>();

        public IQueryable<CustomerAddress> CustomerAddresses => EntityQuery<CustomerAddress>();
    }
}

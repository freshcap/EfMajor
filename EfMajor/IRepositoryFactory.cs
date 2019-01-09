using Microsoft.EntityFrameworkCore;

namespace EfMajor
{
    /// <summary>
    /// Provides a repository for any type using the same db context
    /// </summary>
    public interface IRepositoryFactory
    {
        /// <summary>
        /// Initializes this class for use.
        /// </summary>
        /// <param name="dbContext">The DbContext to use</param>
        void Initialize(DbContext dbContext);

        /// <summary>
        /// Gets an instance of a repository of type TEntity
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns>An instance of the repository</returns>
        IRepository<TEntity> Get<TEntity>() where TEntity : class;
    }
}

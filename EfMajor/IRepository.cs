using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace EfMajor
{
    /// <summary>
    /// Implement CRUD methods for a specific entity T and 
    /// the provided db context
    /// </summary>
    /// <typeparam name="TEntity">The entity type</typeparam>
    public interface IRepository<TEntity> where TEntity : class
    {
        /// <summary>
        /// Attaches an entity as Unchanged
        /// </summary>
        /// <param name="entity">The entity to attach</param>
        void Attach(TEntity entity);

        /// <summary>
        /// Detach a specific entity from the local cache
        /// </summary>
        /// <param name="entity">The entity to detach</param>
        void Detach(TEntity entity);

        /// <summary>
        /// Gets a reference to the DbSet of type TEntity.
        /// </summary>
        IQueryable<TEntity> All { get; }

        /// <summary>
        /// Returns a value indicating whether a database table row exists matching the ID passed in.
        /// </summary>
        /// <param name="id">The ID value to query</param>
        /// <returns>True if the row exists</returns>
        bool Exists(object id);

        /// <summary>
        /// Returns a value indicating whether a database table row exists matching the ID passed in.
        /// Async.
        /// </summary>
        /// <param name="id">The ID value to query</param>
        /// <returns>True if the row exists</returns>
        Task<bool> ExistsAsync(object id);

        /// <summary>
        /// Returns a value indicating whether a database table row exists matching the ID passed in.
        /// Async with cancellation token.
        /// </summary>
        /// <param name="id">The ID value to query</param>
        /// <param name="cancellationToken">A task cancellation token</param>
        /// <returns>True if the row exists</returns>
        Task<bool> ExistsAsync(object id, CancellationToken cancellationToken);

        /// <summary>
        /// Gets an entity object of type TEntity matching the ID passed in.
        /// </summary>
        /// <param name="id">The ID value to query</param>
        /// <returns>The entity object, or NULL if not found</returns>
        TEntity Get(object id);

        /// <summary>
        /// Gets an entity object of type TEntity matching the ID passed in.
        /// Async.
        /// </summary>
        /// <param name="id">The ID value to query</param>
        /// <returns>The entity object, or NULL if not found</returns>
        Task<TEntity> GetAsync(object id);

        /// <summary>
        /// Gets an entity object of type TEntity matching the ID passed in.
        /// Async with cancellation token.
        /// </summary>
        /// <param name="id">The ID value to query</param>
        /// <param name="cancellationToken">A task cancellation token</param>
        /// <returns>The entity object, or NULL if not found</returns>
        Task<TEntity> GetAsync(object id, CancellationToken cancellationToken);

        /// <summary>
        /// Adds an entity to the DbContext.
        /// </summary>
        /// <param name="entity">The entity to add</param>
        void Add(TEntity entity);

        /// <summary>
        /// Attaches an entity and marks it as Modified. When writing the SQL, it will consider all fields to be modififed.
        /// </summary>
        /// <param name="entity">The entity to attach</param>
        void Update(TEntity entity);

        /// <summary>
        /// Gets an entity from the database by its ID and (if it exists) marks it as deleted in the DbContext.
        /// Does NOT throw an exception if the entity does not exist.
        /// </summary>
        /// <param name="id"></param>
        void DeleteId(object id);

        /// <summary>
        /// Attaches an entity to the DbContext and marks it for deletion - minimumally requires the ID property to be populated.
        /// This DOES THROW an exception when saving if the entity does not exist in the database.
        /// </summary>
        /// <param name="entity">The entity object to delete</param>
        void DeleteEntity(TEntity entity);
    }
}

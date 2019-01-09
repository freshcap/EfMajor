using System;
using System.Threading;
using System.Threading.Tasks;

namespace EfMajor
{
    /// <summary>
    /// Unit of Work - encapsulates a DbContext for a series of operations against multiple data entities.
    /// Changes are not committed to the database until Save is executed.
    /// </summary>
    public interface IUnitOfWork : IDisposable
    {
        /// <summary>
        /// Detaches all tracked entities.
        /// </summary>
        void Reset();

        /// <summary>
        /// Attaches an entity as unchanged
        /// </summary>
        /// <typeparam name="T">The entity type</typeparam>
        /// <param name="entity">The entity object</param>
        void Attach<T>(T entity) where T : class;

        /// <summary>
        /// Detach an entity from the local cache
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entity"></param>
        void Detach<TEntity>(TEntity entity) where TEntity : class;

        /// <summary>
        /// Commits all pending changes is DbContext to the database.
        /// </summary>
        void Save();

        /// <summary>
        /// Commits all pending changes is DbContext to the database asynchronously.
        /// </summary>
        Task SaveAsync();

        /// <summary>
        /// Commits all pending changes is DbContext to the database asynchronously, with a cancellsation token.
        /// </summary>
        /// <param name="cancellationToken">A cancellation token</param>
        Task SaveAsync(CancellationToken cancellationToken);

        /// <summary>
        /// Gets an entity object of type TEntity matching the ID passed in.
        /// </summary>
        /// <param name="id">The ID value to query</param>
        /// <returns>The entity object, or NULL if not found</returns>
        T Get<T>(object id) where T : class;

        /// <summary>
        /// Gets an entity object of type TEntity matching the ID passed in, asynchronously
        /// </summary>
        /// <param name="id">The ID value to query</param>
        /// <returns>The entity object, or NULL if not found</returns>
        Task<T> GetAsync<T>(object id) where T : class;

        /// <summary>
        /// Gets an entity object of type TEntity matching the ID passed in, asynchronously, with a cancellsation token.
        /// </summary>
        /// <param name="id">The ID value to query</param>
        /// <param name="cancellationToken">A cancellation token</param>
        /// <returns>The entity object, or NULL if not found</returns>
        Task<T> GetAsync<T>(object id, CancellationToken cancellationToken) where T : class;

        /// <summary>
        /// Adds an entity to the DbContext.
        /// </summary>
        /// <param name="entity">The entity to add</param>
        void Add<T>(T entity) where T : class;

        /// <summary>
        /// Attaches an entity and marks it as Modified. When writing the SQL, it will consider all fields to be modififed.
        /// </summary>
        /// <param name="entity">The entity to attach</param>
        void Update<T>(T Entity) where T : class;

        /// <summary>
        /// Gets an entity from the database by its ID and and (if it exists) marks it as deleted in the DbContext.
        /// Does NOT throw an exception if the entity does not exist.
        /// </summary>
        /// <param name="id"></param>
        void DeleteEntity<T>(T entity) where T : class;

        /// <summary>
        /// Attaches an entity to the DbContext and marks it for deletion - minimumally requires the ID property to be populated.
        /// This DOES THROW an exception when saving if the entity does not exist in the database.
        /// </summary>
        /// <param name="entity">The entity object to delete</param>
        void DeleteId<T>(object id) where T : class;

        /// <summary>
        /// Returns a value indicating whether a database table row exists matching the ID passed in.
        /// </summary>
        /// <param name="id">The ID value to query</param>
        /// <returns>True if the row exists</returns>
        bool Exists<T>(object id) where T : class;

        /// <summary>
        /// Returns a value indicating whether a database table row exists matching the ID passed in.
        /// </summary>
        /// <param name="id">The ID value to query</param>
        /// <param name="cancellationToken">A cancellation token</param>
        /// <returns>True if the row exists</returns>
        Task<bool> ExistsAsync<T>(object id, CancellationToken cancellationToken) where T : class;

        /// <summary>
        /// Returns a value indicating whether a database table row exists matching the ID passed in.
        /// </summary>
        /// <param name="id">The ID value to query</param>
        /// <returns>True if the row exists</returns>
        Task<bool> ExistsAsync<T>(object id) where T : class;
    }
}

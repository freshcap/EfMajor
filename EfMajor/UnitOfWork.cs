using System;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.EntityFrameworkCore;

namespace EfMajor
{
    /// <inheritdoc />
    abstract public class UnitOfWork : IUnitOfWork
    {
        private bool _disposed;
        private DbContext _context;
        private IRepositoryFactory _repositoryFactory;

        /// <summary>
        /// Initializes a new instances of the <see cref="UnitOfWork"/> class.
        /// </summary>
        /// <param name="context">The DbContext to use</param>
        /// <param name="repositoryFactory">A repository factory instance</param>
        public UnitOfWork(DbContext context, IRepositoryFactory repositoryFactory)
        {
            _disposed = false;
            _context = context;
            _repositoryFactory = repositoryFactory;

            repositoryFactory.Initialize(context);
        }

        /// <summary>
        /// Gets an instance of a repository of type TEntity
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns>An instance of the repository</returns>
        protected IRepository<TEntity> Repository<TEntity>() where TEntity : class
        {
            return _repositoryFactory.Get<TEntity>();
        }

        /// <summary>
        /// Gets a queryable reference to an entity, through its repository
        /// </summary>
        /// <typeparam name="TEntity">The entity type</typeparam>
        /// <returns>A queryable entity</returns>
        protected IQueryable<TEntity> EntityQuery<TEntity>() where TEntity : class
        {
            return Repository<TEntity>().All;
        }

        /// <inheritdoc />
        public void Reset()
        {
            foreach (var entry in _context.ChangeTracker.Entries().ToList())
                entry.State = EntityState.Detached;
        }

        /// <inheritdoc />
        public void Detach<TEntity>(TEntity entity) where TEntity : class
        {
            Repository<TEntity>().Detach(entity);
        }

        /// <inheritdoc />
        public void Save()
        {
            // Ignore concurrecy exceptions for deletes (it's okay if it's already deleted)
            DbUpdateConcurrencyException exception = null;
            var retryCounter = 0;
            do
            {
                try
                {
                    _context.SaveChanges();
                    exception = null;
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    exception = ex;
                    foreach (var entry in ex.Entries)
                    {
                        if (entry.State == EntityState.Deleted)
                        {
                            //When EF deletes an item its state is set to Detached
                            //http://msdn.microsoft.com/en-us/data/jj592676.aspx
                            entry.State = EntityState.Detached;
                        }
                        else
                        {
                            throw;
                        }
                    }
                }
            } while (exception != null && retryCounter++ < 3);

            if (exception != null)
            {
                throw exception;
            }
        }

        /// <inheritdoc />
        public async Task SaveAsync(CancellationToken cancellationToken)
        {
            // Ignore concurrecy exceptions for deletes (it's okay if it's already deleted)
            DbUpdateConcurrencyException exception = null;
            var retryCounter = 0;
            do
            {
                try
                {
                    await _context.SaveChangesAsync(cancellationToken);
                    exception = null;
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    exception = ex;
                    foreach (var entry in ex.Entries)
                    {
                        if (entry.State == EntityState.Deleted)
                        {
                            //When EF deletes an item its state is set to Detached
                            //http://msdn.microsoft.com/en-us/data/jj592676.aspx
                            entry.State = EntityState.Detached;
                        }
                        else
                        {
                            throw;
                        }
                    }
                }
            } while (exception != null && retryCounter++ < 3);

            if (exception != null)
            {
                throw exception;
            }
        }

        /// <inheritdoc />
        public async Task SaveAsync()
        {
            await SaveAsync(CancellationToken.None);
        }

        /// <inheritdoc />
        public T Get<T>(object id) where T : class
        {
            return Repository<T>().Get(id);
        }

        /// <inheritdoc />
        public async Task<T> GetAsync<T>(object id) where T : class
        {
            return await GetAsync<T>(id, CancellationToken.None);
        }

        /// <inheritdoc />
        public async Task<T> GetAsync<T>(object id, CancellationToken cancellationToken) where T : class
        {
            return await Repository<T>().GetAsync(id, cancellationToken);
        }

        /// <inheritdoc />
        public void Add<T>(T entity) where T : class
        {
            Repository<T>().Add(entity);
        }

        /// <inheritdoc />
        public void Attach<T>(T entity) where T : class
        {
            Repository<T>().Attach(entity);
        }

        /// <inheritdoc />
        public void Update<T>(T Entity) where T : class
        {
            Repository<T>().Update(Entity);
        }

        /// <inheritdoc />
        public void DeleteEntity<T>(T entity) where T : class
        {
            Repository<T>().DeleteEntity(entity);
        }

        /// <inheritdoc />
        public void DeleteId<T>(object id) where T : class
        {
            Repository<T>().DeleteId(id);
        }

        /// <inheritdoc />
        public bool Exists<T>(object id) where T : class
        {
            return Repository<T>().Exists(id);
        }

        /// <inheritdoc />
        public async Task<bool> ExistsAsync<T>(object id, CancellationToken cancellationToken) where T : class
        {
            return await Repository<T>().ExistsAsync(id, cancellationToken);
        }

        /// <inheritdoc />
        public async Task<bool> ExistsAsync<T>(object id) where T : class
        {
            return await ExistsAsync<T>(id, CancellationToken.None);
        }

        /// <inheritdoc />
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }

            if (disposing)
            {
                _context.Dispose();
            }

            _disposed = true;
        }
    }
}

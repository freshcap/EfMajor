using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace EfMajor
{
    /// <summary>
    /// Implement CRUD methods for a specific entity of type TEntity and the provided DbContext
    /// </summary>
    /// <typeparam name="TEntity">The entity type</typeparam>
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        private DbContext _dbContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="Repository {TEntity}"/> class.
        /// </summary>
        /// <param name="dbContext">The DbContext to use</param>
        public Repository(DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <inheritdoc />
        public virtual IQueryable<TEntity> All
        {
            get
            {
                return _dbContext.Set<TEntity>();
            }
        }

        public virtual void Attach(TEntity entity)
        {
            _dbContext.Attach(entity);
        }

        public virtual void Detach(TEntity entity)
        {
            EntityEntry<TEntity> entry = null;
            try
            {
                entry = _dbContext.Entry(entity);
            }
            catch (Exception)
            { }

            if (entity != null)
            {
                entry.State = EntityState.Detached;
            }
        }

        /// <inheritdoc />
        public virtual bool Exists(object id)
        {
            return Get(id) != null;
        }

        /// <inheritdoc />
        public virtual async Task<bool> ExistsAsync(object id)
        {
            return await ExistsAsync(id, CancellationToken.None);
        }

        /// <inheritdoc />
        public virtual async Task<bool> ExistsAsync(object id, CancellationToken cancellationToken)
        {
            var entity = await GetAsync(id);
            return entity != null;
        }

        /// <inheritdoc />
        public virtual TEntity Get(object id)
        {
            return _dbContext.Set<TEntity>().Find(id);
        }

        /// <inheritdoc />
        public virtual async Task<TEntity> GetAsync(object id)
        {
            return await GetAsync(id, CancellationToken.None);
        }

        /// <inheritdoc />
        public virtual async Task<TEntity> GetAsync(object id, CancellationToken cancellationToken)
        {
            return await _dbContext.Set<TEntity>().FindAsync(new object[] { id }, cancellationToken);
        }

        /// <inheritdoc />
        public virtual void Add(TEntity entity)
        {
            _dbContext.Set<TEntity>().Add(entity);
        }

        /// <inheritdoc />
        public virtual void Update(TEntity entity)
        {
            _dbContext.Update(entity);
        }

        /// <inheritdoc />
        public virtual void DeleteId(object id)
        {
            var entity = Get(id);
            if (entity != null)
            {
                _dbContext.Set<TEntity>().Remove(entity);
            }
        }

        /// <inheritdoc />
        public virtual void DeleteEntity(TEntity entity)
        {
            _dbContext.Entry(entity).State = EntityState.Deleted;
        }
    }
}

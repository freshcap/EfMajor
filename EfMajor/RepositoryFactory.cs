using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace EfMajor
{
    /// <inheritdoc />
    public class RepositoryFactory : IRepositoryFactory
    {
        private DbContext _dbContext;
        private Dictionary<Type, object> _repositories;

        /// <summary>
        /// Initializes a new instance of the <see cref="RepositoryFactory"/> class.
        /// </summary>
        public RepositoryFactory()
        {
            _repositories = new Dictionary<Type, object>();
        }

        /// <inheritdoc />
        public void Initialize(DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <inheritdoc />
        public IRepository<T> Get<T>() where T : class
        {
            if (!_repositories.ContainsKey(typeof(T)))
            {
                _repositories.Add(typeof(T), new Repository<T>(_dbContext));
            }

            return (Repository<T>)_repositories[typeof(T)];
        }
    }
}

﻿using System.Linq.Expressions;

namespace Chat.Logic.Interfaces
{
    public interface IRepositoryManager<T> where T : class
    {
        /// <summary>
        /// Create new entity async.
        /// </summary>
        /// <param name="entity">Entity object</param>
        Task CreateAsync(T entity);

        /// <summary>
        /// Create new entities async.
        /// </summary>
        /// <param name="entities">Entity collection.</param>
        Task CreateRangeAsync(IEnumerable<T> entities);

        /// <summary>
        /// Get all queries.
        /// </summary>
        /// <returns>IQueryable queries.</returns>
        IQueryable<T> GetAll();

        /// <summary>
        /// Get entity async by predicate.
        /// </summary>
        /// <param name="predicate">LINQ predicate.</param>
        /// <returns>T entity.</returns>
        Task<T> GetEntityAsync(Expression<Func<T, bool>> predicate);

        /// <summary>
        /// Get entity async by predicate (without tracking).
        /// </summary>
        /// <param name="predicate">LINQ predicate.</param>
        /// <returns>T entity.</returns>
        Task<T> GetEntityWithoutTrackingAsync(Expression<Func<T, bool>> predicate);

        /// <summary>
        /// Update entity
        /// </summary>
        /// <param name="entity">Entity object</param>
        void Update(T entity);

        /// <summary>
        /// Remove entity from database.
        /// </summary>
        /// <param name="entity">Entity object.</param>
        void Delete(T entity);

        /// <summary>
        /// Remove entities from database
        /// </summary>
        /// <param name="entity">Entity object</param>
        void DeleteRange(IEnumerable<T> entity);

        /// <summary>
        /// Persists all updates to the data source async.
        /// </summary>
        Task SaveChangesAsync();
    }
}

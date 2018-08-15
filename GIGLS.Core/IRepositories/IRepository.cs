using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace GIGL.GIGLS.Core.Repositories  
{
    public interface IRepository<TEntity> where TEntity : class
    {
        TEntity Get(int id);
        IQueryable<TEntity> GetAll();
        IQueryable<TEntity> GetAllAsQueryable();
        /// <summary>
        /// Gets all entries for this entity
        /// </summary>
        /// <param name="includeProperties">Specify additional properties to be pulled. Separate multiple properties by comma (,)</param>
        /// <returns></returns>
        IEnumerable<TEntity> GetAll(string includeProperties);
        IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate);
        /// <summary>
        /// Gets all entries that match specified conditions
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="includeProperties">Additional entity properties to pull. Separate multiple entities with comma (,)</param>
        /// <returns></returns>
        IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate, string includeProperties = "");
        /// <summary>
        /// Gets all entries that match specified conditions asynchronously
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="includeProperties">Additional entity properties to pull. Separate multiple entities with comma (,)</param>
        /// <returns></returns>
        Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate, string includeProperties = "");
        TEntity SingleOrDefault(Expression<Func<TEntity, bool>> predicate);

        Task<TEntity> GetAsync(int id);
        Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> predicate);
        /// <summary>
        /// Gets requested entity
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="includeProperties">Additional entity properties to pull. Separate multiple entities with comma (,) </param>
        /// <returns></returns>
        Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> predicate, string includeProperties = "");
        Task<bool> ExistAsync(Expression<Func<TEntity, bool>> predicate);

        void Add(TEntity entity);
        void AddRange(IEnumerable<TEntity> entities);
        
        void Remove(TEntity entity);
        void RemoveRange(IEnumerable<TEntity> entities);        
    }
}
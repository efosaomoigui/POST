using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace GIGLS.Core.IRepositories
{
    public interface IQueryFluent<TEntity> 
    {
        IEnumerable<TEntity> SelectPage(int page, int pageSize, out int totalCount);
        IEnumerable<TResult> Select<TResult>(Expression<Func<TEntity, TResult>> selector = null);
        IEnumerable<TEntity> Select();
        Task<IEnumerable<TEntity>> SelectAsync();
        IQueryFluent<TEntity> Include(Expression<Func<TEntity, object>> expression);
        IQueryFluent<TEntity> OrderBy(Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy);
    }
}

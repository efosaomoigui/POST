using GIGLS.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace GIGLS.Core.Common.Helpers
{
    public interface IPagedQueryBuilder<TEntity> : IQueryCommand<PagedList<TEntity>>
    {
        IPagedQueryBuilder<TEntity> Search(string searchTerm);
        IPagedQueryBuilder<TEntity> Search(Expression<Func<TEntity, bool>> predicate);
        IPagedQueryBuilder<TEntity> OrderByDescendending<TKey>(Expression<Func<TEntity, TKey>> keySelector);
        IPagedQueryBuilder<TEntity> OrderBy<TKey>(Expression<Func<TEntity, TKey>> keySelector);
        IPagedQueryBuilder<TEntity> Order<TKey>(Expression<Func<TEntity, TKey>> keySelector, string order = PageOrder.ASC);
    }
}

using GIGLS.Core.IRepositories;
using GIGLS.Infrastructure.Persistence.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace GIGLS.Infrastructure.Persistence.Repositories
{
    public sealed class QueryFluent<TEntity,TContext> : IQueryFluent<TEntity> where TEntity : class where TContext : GIGLSContext
    {
        #region Private Fields
        private readonly Expression<Func<TEntity, bool>> _expression;
        private readonly Repository<TEntity,TContext> _repository;
        private readonly List<Expression<Func<TEntity, object>>> _includes;
        private Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> _orderBy;
        #endregion Private Fields

        #region Constructors
        public QueryFluent(Repository<TEntity,TContext> repository)
        {
            _repository = repository;
            _includes = new List<Expression<Func<TEntity, object>>>();
        }

        public QueryFluent(Repository<TEntity,TContext> repository, Expression<Func<TEntity, bool>> expression) : this(repository) { _expression = expression; }
        #endregion Constructors

        public IEnumerable<TEntity> SelectPage(int page, int pageSize, out int totalCount)
        {
            totalCount = _repository.Select(_expression).Count();
            return _repository.Select(_expression, _includes, _orderBy, page, pageSize);
        }

        public IEnumerable<TEntity> Select() { return _repository.Select(_expression, _includes, _orderBy); }

        public IEnumerable<TResult> Select<TResult>(Expression<Func<TEntity, TResult>> selector) { return _repository.Select(_expression, _includes, _orderBy).Select(selector); }

        public async Task<IEnumerable<TEntity>> SelectAsync() { return await _repository.SelectAsync(_expression, _orderBy, _includes); }

        public IQueryFluent<TEntity> Include(Expression<Func<TEntity, object>> expression)
        {
            _includes.Add(expression);
            return this;
        }

        public IQueryFluent<TEntity> OrderBy(Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy)
        {
            _orderBy = orderBy;
            return this;
        }
    }
}


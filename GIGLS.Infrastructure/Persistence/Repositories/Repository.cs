using GIGL.GIGLS.Core.Domain;
using GIGL.GIGLS.Core.Repositories;
using GIGLS.Core;
using GIGLS.CORE.Domain;
using GIGLS.Infrastructure.IdentityInfrastrure;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace GIGLS.Infrastructure.Persistence.Repository
{
    public class Repository<TEntity, TContext> : IRepository<TEntity>
        where TEntity : class
        where TContext : GIGLSContext
    {
        protected readonly TContext Context;

        public Repository(TContext context)
        {
            Context = context;
            MapperConfig.Initialize();
        }

        public TEntity Get(int id)
        {
            return Context.Set<TEntity>().Find(id);
        }

        public Task<TEntity> GetAsync(int id)
        {
            return Context.Set<TEntity>().FindAsync(id);
        }

        public Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return Context.Set<TEntity>().FirstOrDefaultAsync(predicate);
        }
        public Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> predicate, string includeProperties = "")
        {
            IQueryable<TEntity> query = Context.Set<TEntity>();
            query = _IncludeProperties(query, includeProperties);
            return query.FirstOrDefaultAsync(predicate);
        }

        public IQueryable<TEntity> GetAll()
        {
            return Context.Set<TEntity>().AsQueryable();
        }

        public IQueryable<TEntity> GetAllAsQueryable()
        {
            return Context.Set<TEntity>().AsQueryable();
        }

        //public IEnumerable<TEntity> GetAll()
        //{
        //    return Context.Set<TEntity>().ToList();
        //}
        public IEnumerable<TEntity> GetAll(string includeProperties)
        {
            IQueryable<TEntity> query = Context.Set<TEntity>();
            query = _IncludeProperties(query, includeProperties);
            return query.ToList();  // Context.Set<TEntity>().ToList();
        }
        public IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate)
        {
            return Context.Set<TEntity>().Where(predicate);
        }

        public IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate, string includeProperties = "")
        {
            IQueryable<TEntity> query = Context.Set<TEntity>();
            query = _IncludeProperties(query, includeProperties);
            return query.Where(predicate);
        }

        public Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate, string includeProperties = "")
        {
            IQueryable<TEntity> query = Context.Set<TEntity>();
            query = _IncludeProperties(query, includeProperties);
            return Task.FromResult<IEnumerable<TEntity>>(query.Where(predicate).ToList());
        }

        private IQueryable<TEntity> _IncludeProperties(IQueryable<TEntity> query, string properties)
        {
            if (!string.IsNullOrEmpty(properties))
            {
                foreach (var property in properties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(property.Trim());
                }
            }
            return query;
        }

        public TEntity SingleOrDefault(Expression<Func<TEntity, bool>> predicate)
        {
            return Context.Set<TEntity>().SingleOrDefault(predicate);
        }

        public void Add(TEntity entity)
        {
            Context.Set<TEntity>().Add(entity);
        }

        public void AddRange(IEnumerable<TEntity> entities)
        {
            Context.Set<TEntity>().AddRange(entities);
        }

        public void Remove(TEntity entity)
        {
            Context.Set<TEntity>().Remove(entity);
        }

        public void RemoveRange(IEnumerable<TEntity> entities)
        {
            Context.Set<TEntity>().RemoveRange(entities);
        }

        public Task<bool> ExistAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return Context.Set<TEntity>().AnyAsync(predicate);
        }
    }

    public class AuthRepository<TEntity, TContext> : IDisposable
                where TEntity : class
        where TContext : GIGLSContext
    {

        public UserManager<User> _userManager;
        public RoleManager<AppRole> _roleManager; 
        public Repository<User, TContext> _repo;
        public Repository<AppRole, TContext> _repoRole;
        //private TContext context { get; set; }
        //public UserStore<User, AppRole, string, IdentityUserLogin, IdentityUserRole, AppUserClaim> _userManager3;


        public AuthRepository(TContext context)
        {
            _userManager = new UserManager<User>(new GiglsUserStore<User>(context));
            _roleManager = new RoleManager<AppRole>(new RoleStore<AppRole>(context)); 
            _repo = new Repository<User, TContext>(context);
            //_repoRole = new Repository<AppRole, TContext>(context); 
            //this.context = context;
            //_userManager3 = new GiglsUserStore<User>(context);
        }


        public void Dispose()
        {
            _userManager.Dispose();
            //_roleManager.Dispose();

        }

    }

}
using Entities;
using MicroBeard.Contracts;
using MicroBeard.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace MicroBeard.Repository
{
    public abstract class RepositoryBase<T> : IRepositoryBase<T> where T : class
    {
        protected RepositoryContext RepositoryContext { get; set; }

        public RepositoryBase(RepositoryContext repositoryContext)
        {
            RepositoryContext = repositoryContext;
        }

        public IQueryable<T> FindAll() => RepositoryContext.Set<T>().AsNoTracking();
        public IQueryable<T> FindByCollecion(Expression<Func<T, bool>> expression) => RepositoryContext.Set<T>().Where(expression).AsNoTracking();
        public void Create(T entity) => RepositoryContext.Set<T>().Add(entity);
        public void Update(T entity) => RepositoryContext.Set<T>().Update(entity);
        public void Delete(T entity) => RepositoryContext.Set<T>().Remove(entity);
    }
}

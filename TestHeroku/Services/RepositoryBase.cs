using IntelliCRMAPIService.DBContext;
using IntelliCRMAPIService.Repository;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace IntelliCRMAPIService.Services
{
    public abstract class RepositoryBase<T> : IRepositoryBase<T> where T : class
    {
        protected PostgresDBContext ApplicationDBContext { get; set; }
        public RepositoryBase(PostgresDBContext repositoryContext)
        {
            this.ApplicationDBContext = repositoryContext;
        }
        public IQueryable<T> FindAll()
        {
            return this.ApplicationDBContext.Set<T>().AsNoTracking();
        }
        public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression)
        {
            return this.ApplicationDBContext.Set<T>().Where(expression).AsNoTracking();
        }
        public T Create(T entity)
        {
            var result = this.ApplicationDBContext.Set<T>().Add(entity);
            this.ApplicationDBContext.SaveChanges();

            return result.Entity;
        }
        public void Update(T entity)
        {
            this.ApplicationDBContext.Set<T>().Update(entity);
            this.ApplicationDBContext.SaveChanges();
        }
        public void Delete(T entity)
        {
            this.ApplicationDBContext.Set<T>().Remove(entity);
            this.ApplicationDBContext.SaveChanges();
        }
    }
}

using Book.DataAccess.Data;
using Book.DataAccess.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;

namespace Book.DataAccess.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly ApplicationDbContext dbContext;
        internal DbSet<T> dbSet;

        public Repository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
            this.dbSet = dbContext.Set<T>();
            // dbContext.Categories = dbSet
        }
        public void Add(T entity)
        {
           dbSet.Add(entity); // equivalent to dbContext.Categories.Add(entity);
        }

        public T Get(Expression<Func<T, bool>> filter)
        {
            IQueryable<T> query = dbSet; // dbContext.Categories.
            query = query.Where(filter); // .Where(c => c.Id == id)

            return query.FirstOrDefault(); // FirstOrDefault(); 
        }

        public IEnumerable<T> GetAll()
        {
            IQueryable<T> query = dbSet;

            return query.ToList();
        }

        public void Remove(T entity)
        {
            dbContext.Remove(entity);
        }

        public void RemoveRange(IEnumerable<T> entity)
        {
            dbContext.RemoveRange(entity);
        }
    }
}

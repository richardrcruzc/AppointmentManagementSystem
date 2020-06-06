using AmsApi.Domain;
using AmsApi.Infraestructure;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AmsApi.Repository
{
    public class Repository<T> : IRepository<T> where T : BaseEntity
    {
        private readonly AmsApiDbContext _dbContext;
        private DbSet<T> entities;
        string errorMessage = string.Empty;
        public Repository(AmsApiDbContext dbContext)
        {
            _dbContext = dbContext;
            entities = dbContext.Set<T>();
        }
        public IQueryable<T> FindBy(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes)
        {
            var query = GetAll().Where(predicate);
            return includes.Aggregate(query, (current, includeProperty) => current.Include(includeProperty));
        }

        public async Task CreateAsync(T entity)
        {
            if (entity == null) throw new ArgumentNullException("entity");
            entities.Add(entity);
            await _dbContext.SaveChangesAsync();
             
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await GetById(id);
            _dbContext.Set<T>().Remove(entity);
            await _dbContext.SaveChangesAsync();
        }

       

        public IQueryable<T> GetAll()
        {
            return  _dbContext.Set<T>().AsNoTracking();
        }

        public async Task<T> GetById(int id)
        {
            var query = await _dbContext.Set<T>()
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.Id == id);
            return query;

        }

        public async Task UpdateAsync(int id, T entity)
        {
            if (entity == null) throw new ArgumentNullException("entity");


            if(await GetById(id) == null) throw new ArgumentNullException("entity");


            _dbContext.Set<T>().Update(entity);
            await _dbContext.SaveChangesAsync();
        }
    }
}

using AmsApi.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AmsApi.Repository
{
    public interface IRepository<T> where T : BaseEntity
    {
      
        
        IQueryable<T> GetAll();

        Task<T> GetById(int id);

        Task CreateAsync(T entity);

        Task UpdateAsync(int id, T entity);

        Task DeleteAsync(int id);

        IQueryable<T> FindBy(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes);
    }
}
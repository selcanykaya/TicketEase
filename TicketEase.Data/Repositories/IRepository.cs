using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TicketEase.Data.Entities;

namespace TicketEase.Data.Repositories
{
    public interface IRepository<TEntity> where TEntity : BaseEntity
    {
        // Basic CRUD operations
        Task<TEntity> GetByIdAsync(int id);
        Task<IEnumerable<TEntity>> GetAllAsync();
        Task AddAsync(TEntity entity);
        Task UpdateAsync(TEntity entity);
        Task DeleteAsync(int id);

        //Get a single entity by predicate
        Task<TEntity> Get(Expression<Func<TEntity, bool>> predicate);

        // Find entities by predicate
        Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate);

        // Check if an entity exists by predicate
        Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate);

        // Count entities with an optional filter
        Task<int> CountAsync(Expression<Func<TEntity, bool>> filter = null);


        // Return IQueryable for more complex queries
        IQueryable<TEntity> Query(Expression<Func<TEntity, bool>> predicate = null);


        // Get entities with pagination, sorting, and filtering
        Task<IEnumerable<TEntity>> GetPagedAsync(
            int page,
            int pageSize,
            Expression<Func<TEntity, bool>> filter = null,
            Expression<Func<TEntity, object>> orderBy = null,
            bool descending = false
        );


        // Get entities sorted by a specific field
        Task<IEnumerable<TEntity>> GetSortedAsync(Expression<Func<TEntity, object>> orderBy,bool descending = false);
           
            
        // Get entities filtered by a specific condition
        Task<IEnumerable<TEntity>> GetFilteredAsync(Expression<Func<TEntity, bool>> filter);
           
        
    }
}


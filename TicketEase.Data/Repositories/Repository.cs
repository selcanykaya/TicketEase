using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TicketEase.Data.Context;
using TicketEase.Data.Entities;

namespace TicketEase.Data.Repositories
{
    public class Repository<TEntity> : IRepository<TEntity>
        where TEntity : BaseEntity
    {
        
        private readonly TicketEaseDbContext _context;
        private readonly DbSet<TEntity> _dbSet;
       
        
        public Repository(TicketEaseDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<TEntity>();
        }

        public async Task AddAsync(TEntity entity)
        {
           entity.CreatedAt = DateTime.UtcNow; // Set CreatedAt to current time
            await _dbSet.AddAsync(entity);
        }

        public Task<int> CountAsync(Expression<Func<TEntity, bool>> filter = null)
        {
            return filter == null
                ? _dbSet.CountAsync()
                : _dbSet.CountAsync(filter);
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _dbSet.FindAsync(id);
            if (entity == null)
            {
                throw new ArgumentException($"Entity with id {id} not found.");
            }
            await DeleteAsync(entity); // Soft delete
        }

        public async Task DeleteAsync(TEntity entity)
        {
            entity.IsDeleted = true; // Soft delete
            entity.UpdatedAt = DateTime.UtcNow; // Set UpdatedAt to current time
            _dbSet.Update(entity); 
        }

        public async Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await _dbSet.AnyAsync(predicate);
        }

        public async Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await _dbSet.Where(predicate).ToListAsync();
        }

        public async Task<TEntity> Get(Expression<Func<TEntity, bool>> predicate)
        {
            return await _dbSet.FirstOrDefaultAsync(predicate);
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<TEntity> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task<IEnumerable<TEntity>> GetFilteredAsync(Expression<Func<TEntity, bool>> filter)
        {
           return await _dbSet.Where(filter).ToListAsync();
        }

        public async Task<IEnumerable<TEntity>> GetPagedAsync(int page, int pageSize, Expression<Func<TEntity, bool>> filter = null, Expression<Func<TEntity, object>> orderBy = null, bool descending = false)
        {
            IQueryable<TEntity> query = _dbSet;
            if (filter != null)
            {
                query = query.Where(filter);
            }
            if (orderBy != null)
            {
                query = descending ? query.OrderByDescending(orderBy) : query.OrderBy(orderBy);
            }
            return await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<IEnumerable<TEntity>> GetSortedAsync(Expression<Func<TEntity, object>> orderBy, bool descending = false)
        {
            IQueryable<TEntity> query = _dbSet;
            query = descending ? query.OrderByDescending(orderBy) : query.OrderBy(orderBy);
            return await query.ToListAsync();
        }

        public IQueryable<TEntity> Query(Expression<Func<TEntity, bool>> predicate = null)
        {
            return predicate == null
                ? _dbSet
                : _dbSet.Where(predicate);
        }

        public async Task UpdateAsync(TEntity entity)
        {
            entity.UpdatedAt = DateTime.UtcNow; // Set UpdatedAt to current time
            _dbSet.Update(entity);
        }
    }
}

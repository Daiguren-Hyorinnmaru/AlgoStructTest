﻿using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace ServerAPI.DataBase.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly DataContext _context;
        protected readonly DbSet<T> _dbSet;

        public Repository(DataContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public async Task<T?> GetByIdAsync(int id) => await _dbSet.FindAsync(id);

        public async Task<IEnumerable<T>> GetAllAsync() => await _dbSet.ToListAsync();

        public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate) =>
            await _dbSet.Where(predicate).ToListAsync();

        public async Task AddAsync(T entity) =>
            await _dbSet.AddAsync(entity);

        public async Task<bool> ExistsAsync(T entity) =>
            await _dbSet.AnyAsync(e => e.Equals(entity));

        public async Task AddRangeAsync(IEnumerable<T> entities) => 
            await _dbSet.AddRangeAsync(entities);

        public void Update(T entity) => _dbSet.Update(entity);

        public void Remove(T entity) => _dbSet.Remove(entity);

        public void RemoveRange(IEnumerable<T> entities) => _dbSet.RemoveRange(entities);

        public async Task<IEnumerable<TProp>> GetDistinctAsync<TProp>(Expression<Func<T, TProp>> selector)
        {
            return await _dbSet
                .Select(selector)
                .Distinct()
                .ToListAsync();
        }
    }
}

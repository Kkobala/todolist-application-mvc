using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace TodoList_Application
{
    public class TodoListRepository<T> : ITodoListRepository<T> where T : class
    {
        private readonly AppDbContext _appDbContext;
        public TodoListRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _appDbContext.Set<T>().ToListAsync();
        }

        public async Task<IEnumerable<T>> Find(Expression<Func<T, bool>> expression)
        {
            return await _appDbContext.Set<T>().Where(expression).ToListAsync();
        }

        public async Task<T> GetById(int id)
        {
            return await _appDbContext.Set<T>().FindAsync(id);
        }

        public Task Add(T todo)
        {
            var result = _appDbContext.Set<T>().Add(todo);

            return Task.FromResult(result);
        }

        public Task Remove(T todo)
        {
            var result = _appDbContext.Set<T>().Remove(todo);

            return Task.FromResult(result);
        }

        public Task Update(T todo)
        {
            var result = _appDbContext.Set<T>().Update(todo);

            return Task.FromResult(result);
        }

        public async Task SaveChangesAsync()
        {
            await _appDbContext.SaveChangesAsync();
        }
    }
}

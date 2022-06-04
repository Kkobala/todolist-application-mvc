using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace TodoList_Application
{
    public interface ITodoListRepository<T>
    {
        Task<T> GetById(int id);

        Task<IEnumerable<T>> GetAllAsync();
        Task<IEnumerable<T>> Find(Expression<Func<T, bool>> expression);

        Task Add(T entity);

        Task Remove(T entity);

        Task Update(T entity);

        Task SaveChangesAsync();
    }
}

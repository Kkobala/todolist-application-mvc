using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace TodoList_Application
{
    public interface ITodoListRepository
    {
        public Task<TodoList> GetById(int id);

        public Task<IEnumerable<TodoList>> GetAllAsync();

        public Task<IEnumerable<TodoList>> GetFilteredTodoLists(bool showDueToday, bool hideCompleted);

        public Task<IEnumerable<TodoList>> GetNotStartedToDoLists();

        public Task<IEnumerable<TodoList>> Find(Expression<Func<TodoList, bool>> expression);

        public Task Add(TodoList entity);

        public Task Remove(TodoList entity);

        public Task Update(TodoList entity);

        public Task SetReminderVisibility(List<TodoList> todoList, bool isVisible);

        public Task SaveChangesAsync();
    }
}

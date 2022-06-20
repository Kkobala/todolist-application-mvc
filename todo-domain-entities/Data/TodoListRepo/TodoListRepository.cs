using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace TodoList_Application
{
    public class TodoListRepository : ITodoListRepository
    {
        private readonly AppDbContext _appDbContext;
        public TodoListRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<IEnumerable<TodoList>> GetAllAsync()
        {
            return await _appDbContext.TodoLists.ToListAsync();
        }
        
        public async Task<IEnumerable<TodoList>> GetFilteredTodoLists(bool showDueToday, bool hideCompleted)
        {
            var todoLists = _appDbContext.TodoLists.AsQueryable();

            if (hideCompleted)
                todoLists = todoLists.Where(x => x.Status != TodoStatus.Completed);

            if (showDueToday)
                todoLists = todoLists.Where(x => x.DueDate.Date == DateTime.Now.Date);

            return await todoLists.ToListAsync();
        }

        public async Task<IEnumerable<TodoList>> GetNotStartedToDoLists()
        {
            return await _appDbContext.TodoLists.Where(x => x.Status == TodoStatus.NotStarted).ToListAsync();
        }

        public async Task<IEnumerable<TodoList>> Find(Expression<Func<TodoList, bool>> expression)
        {
            return await _appDbContext.TodoLists.Where(expression).ToListAsync();
        }

        public async Task<TodoList> GetById(int id)
        {
            return await _appDbContext.TodoLists.FirstOrDefaultAsync(x => x.Id == id);
        }

        public Task Add(TodoList todo)
        {
            var result = _appDbContext.TodoLists.Add(todo);

            return Task.FromResult(result);
        }

        public Task Remove(TodoList todo)
        {
            var result = _appDbContext.TodoLists.Remove(todo);

            return Task.FromResult(result);
        }

        public Task Update(TodoList todo)
        {
            var result = _appDbContext.TodoLists.Update(todo);

            return Task.FromResult(result);
        }

        public async Task SetReminderVisibility(List<TodoList> todoList, bool isVisible)
        {
            for (int i = 0; i < todoList.Count; i++)
                todoList[i].IsVisibleReminder = isVisible;

            _appDbContext.TodoLists.UpdateRange(todoList);

            await SaveChangesAsync();
        }

        public async Task SaveChangesAsync()
        {
            await _appDbContext.SaveChangesAsync();
        }
    }
}

using System.Collections.Generic;
using System.Linq;
using todo_aspnetmvc.Models;
using TodoList_Application;

namespace todo_aspnetmvc.Mappers
{
    public static class DomainModelToClientModelMappers
    {
        public static TodoListModel ToDoListDomainToClientModel(this TodoList todoList)
        {
            return new TodoListModel()
            {
                Id = todoList.Id,
                Title = todoList.Title,
                Description = todoList.Description,
                DueDate = todoList.DueDate,
                CreatedAt = todoList.CreationDate,
                Status = todoList.Status,
                IsVisible = todoList.IsVisible,
                IsVisibleReminder = todoList.IsVisibleReminder,
            };
        }

        public static IEnumerable<TodoListModel> ListOfToDoListsDomainToClientModel(this IEnumerable<TodoList> enumer)
        {
            return enumer.Select(x => x.ToDoListDomainToClientModel());
        }
    }
}

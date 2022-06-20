using System.Collections.Generic;
using System.Linq;
using todo_aspnetmvc.Models;
using TodoList_Application;

namespace todo_aspnetmvc.Mappers
{
    public static class ClientModelToDomainModelMappers
    {
        public static TodoList ToDoListClientToDomainModel(this TodoListModel todoListModel)
        {
            return new TodoList()
            {
                Id = todoListModel.Id,
                Title = todoListModel.Title,
                IsVisible = todoListModel.IsVisible,
                CreationDate = todoListModel.CreatedAt,
                Description = todoListModel.Description,
                DueDate = todoListModel.DueDate,
                Status = todoListModel.Status,
                IsVisibleReminder = todoListModel.IsVisibleReminder
            };
        }

        public static IEnumerable<TodoList> ListOfTodoListClientToDomainModel(this IEnumerable<TodoListModel> models)
        {
            return models.Select(x => x.ToDoListClientToDomainModel());
        }
    }
}

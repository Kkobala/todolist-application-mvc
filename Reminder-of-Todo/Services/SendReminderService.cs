
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TodoList_Application.UnitOfWork;
using System.Linq;
using TodoList_Application;

namespace Reminder_of_Todo.Services
{
    public class SendReminderService
    {
        private readonly IUnitofWork _unitofWork;

        public SendReminderService(IUnitofWork unitOfWork)
        {
            _unitofWork = unitOfWork;
        }

        public async Task Execute()
        {
            var todoLists = await _unitofWork.TodoLists.GetNotStartedToDoLists();

            if (todoLists != default && todoLists.Any()) {
                var servicesForSettingReminder = new List<TodoList>();
                var servicesForUnsettingReminder = new List<TodoList>();

                foreach (var item in todoLists)
                {
                    var diffBetweenNowAndDueDate = DateTime.Now.Day - item.DueDate.Day;

                    if (diffBetweenNowAndDueDate >= 0 && diffBetweenNowAndDueDate < 2 && !item.IsVisibleReminder)
                        servicesForSettingReminder.Add(item);
                    else if(item.IsVisibleReminder)
                        servicesForUnsettingReminder.Add(item);
                }

                if (servicesForSettingReminder.Any())
                    await _unitofWork.TodoLists.SetReminderVisibility(servicesForSettingReminder, true);

                if (servicesForUnsettingReminder.Any())
                    await _unitofWork.TodoLists.SetReminderVisibility(servicesForUnsettingReminder, false);
            }
        }
    }
}
    
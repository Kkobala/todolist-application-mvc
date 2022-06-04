using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TodoList_Application.UnitOfWork
{
    public class UnitofWork<T> : IUnitofWork<T> where T : class
    {
        public UnitofWork(ITodoListRepository<T> todoListRepository)
        {
            TodoLists = todoListRepository;
        }

        public ITodoListRepository<T> TodoLists { get; private set; }

        public async Task Complete()
        {
            await TodoLists.SaveChangesAsync();
        }
    }
}

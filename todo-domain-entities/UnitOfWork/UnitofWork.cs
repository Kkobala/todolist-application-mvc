using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TodoList_Application.UnitOfWork
{
    public class UnitofWork : IUnitofWork
    {
        public UnitofWork(ITodoListRepository todoListRepository)
        {
            TodoLists = todoListRepository;
        }

        public ITodoListRepository TodoLists { get; private set; }


        public async Task Complete()
        {
            await TodoLists.SaveChangesAsync();
        }
    }
}

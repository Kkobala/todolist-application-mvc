using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TodoList_Application.UnitOfWork
{
    public interface IUnitofWork<T> where T : class
    {
        public ITodoListRepository<T> TodoLists { get; }

        public Task Complete();
    }
}

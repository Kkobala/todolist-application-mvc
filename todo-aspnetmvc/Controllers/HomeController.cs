using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using todo_aspnetmvc.Mappers;
using todo_aspnetmvc.Models;
using TodoList_Application.UnitOfWork;

namespace todo_aspnetmvc.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnitofWork _unitofWork;

        public HomeController(ILogger<HomeController> logger,
            IUnitofWork unitofWork)
        {
            _logger = logger;
            _unitofWork = unitofWork;
        }

        public async Task<ActionResult> Index(bool showDueToday = false, bool hideCompleted = false)
        {
            var todoList = await _unitofWork.TodoLists.GetFilteredTodoLists(showDueToday, hideCompleted);

            _logger.LogInformation($"All items are accepted. Controller{nameof(Index)}");

            if (todoList == default)
                return RedirectToAction(nameof(Error), new { statusCode = StatusCodes.Status404NotFound });

            return View(todoList.Where(x => x.IsVisible).ListOfToDoListsDomainToClientModel());
        }

        public async Task<ActionResult> Details(int id)
        {
            if (id <= 0)
            {
                _logger.LogError($"Id:{id} was invalid (was less than zero)." +
                    $"Controller{nameof(Details)}");

                return RedirectToAction(nameof(Error), new { statusCode = StatusCodes.Status404NotFound });
            }

            var todoList = await _unitofWork.TodoLists.GetById(id);

            if (todoList == null)
            {
                _logger.LogError($"Id:{id} was not found." +
                   $"Controller{nameof(Details)}");

                return RedirectToAction(nameof(Error), new { statusCode = StatusCodes.Status404NotFound });
            }

            _logger.LogError($"Id:{id} was found." +
                   $"Controller{nameof(Details)}");

            var toDoListViewModel = todoList.ToDoListDomainToClientModel();
            toDoListViewModel.IsVisibleReminder = todoList.IsVisibleReminder;

            return View(toDoListViewModel);
        }

        public ViewResult Create()
        {
            _logger.LogInformation($"View called. Controller{nameof(Create)}");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<RedirectToActionResult> Create(TodoListModel list)
        {
            if (list == null || ModelState.IsValid)
            {
                _logger.LogError($"TodoList is null or ModelState is invalid:{ModelState.IsValid}");

                return RedirectToAction(nameof(Error));
            }

            try
            {
                await _unitofWork.TodoLists.Add(list.ToDoListClientToDomainModel());
                await _unitofWork.Complete();
                _logger.LogInformation($"Item was added. Controller{nameof(Create)}");

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                _logger.LogError($"Error occured while  trying to add items. Controller{nameof(Create)}");

                return RedirectToAction(nameof(Error), new { StatusCode = StatusCodes.Status406NotAcceptable });
            }
        }

        public async Task<ActionResult> Update(int id)
        {
            if (id <= 0)
            {
                _logger.LogError($"Id:{id} was invalid. Controller{nameof(Update)}");

                return RedirectToAction(nameof(Error), new { StatusCode = StatusCodes.Status406NotAcceptable });
            }

            var todo = await _unitofWork.TodoLists.GetById(id);

            if (todo == null)
            {
                _logger.LogError($"item with id:{id} was not found. Controller{nameof(Update)}");

                return RedirectToAction(nameof(Error), new { statusCode = StatusCodes.Status406NotAcceptable });
            }

            _logger.LogInformation($"Item woth Id:{id} was found. Controller{nameof(Update)}");

            return View(todo.ToDoListDomainToClientModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<RedirectToActionResult> Update(TodoListModel list)
        {
            if (list == null || list.Id <= 0 || ModelState.IsValid)
            {
                _logger.LogError($"Id:{list.Id}, item or ModelState was invalid.{ModelState.IsValid}. Controller{nameof(Update)}");

                return RedirectToAction(nameof(Error), new { statusCode = StatusCodes.Status406NotAcceptable });
            }

            try
            {
                await _unitofWork.TodoLists.Update(list.ToDoListClientToDomainModel());
                await _unitofWork.Complete();

                _logger.LogInformation($"Item with Id:{list.Id} was updated. Controller{nameof(Update)}");

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                _logger.LogError($"Error occured while trying to update item with Id:{list.Id}. Controller{nameof(Update)}");

                return RedirectToAction(nameof(Error), 404);
            }
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<RedirectToActionResult> Delete(int id)
        {
            if (id <= 0)
            {
                _logger.LogError($"Id:{id} was invalid. Controller{nameof(Delete)}");

                return RedirectToAction(nameof(Error), new { StatusCode = StatusCodes.Status404NotFound });
            }

            try
            {
                var list = await _unitofWork.TodoLists.GetById(id);

                if (list == null)
                {
                    _logger.LogError($"Error occured while trying to delete item with Id:{id}." +
                        $" Controller{nameof(Delete)}");

                    return RedirectToAction(nameof(Error), new { StatusCode = StatusCodes.Status404NotFound });
                }

                await _unitofWork.TodoLists.Remove(list);
                await _unitofWork.Complete();

                _logger.LogInformation($"Item with Id:{id} was deleted." +
                    $" Controller{nameof(Delete)}");

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                _logger.LogError($"Error occured while trying to delete item with Id:{id}." +
                    $"Controller{nameof(Delete)}");

                return RedirectToAction(nameof(Error), new { StatusCode = StatusCodes.Status404NotFound });
            }
        }

        [HttpGet]
        public async Task<IActionResult> Copy(int id)
        {
            var toDolist = await _unitofWork.TodoLists.GetById(id);
            
            if (toDolist != default)
            {
                var copiedToDolist = new TodoListModel()
                {
                    Description = toDolist.Description,
                    DueDate = toDolist.DueDate,
                    IsVisible = toDolist.IsVisible,
                    IsVisibleReminder = toDolist.IsVisibleReminder,
                    Status = toDolist.Status,
                    Title = toDolist.Title,
                };

                return RedirectToAction(nameof(Create), new { list = copiedToDolist });
            }

            return RedirectToAction(nameof(Error), new { StatusCode = StatusCodes.Status404NotFound });
        }

        public IActionResult Privacy()
        {
            _logger.LogInformation(" ");
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

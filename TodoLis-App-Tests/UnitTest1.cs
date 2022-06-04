using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using todo_aspnetmvc.Controllers;
using todo_aspnetmvc.Models;
using TodoList_Application;
using TodoList_Application.UnitOfWork;

namespace TodoLis_App_Tests
{
    public class Tests
    {
        private Mock<IUnitofWork<TodoList>> mock;
        private Mock<ILogger<HomeController>> mockWork;

        [SetUp]
        public void Setup()
        {
            mock = new Mock<IUnitofWork<TodoList>>();
            mockWork = new Mock<ILogger<HomeController>>();
        }

        [Test]
        public void Index_Returns_ViewResult_With_TodoList()
        {
            // Arrange
            mock.Setup(repo => repo.TodoLists.GetAllAsync().Result)
                .Returns(GetTestTodoLists());

            var controller = new HomeController(mockWork.Object, mock.Object);

            var result = controller.Index().Result;

            //Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.TypeOf<ViewResult>());
            mock.Verify(repo => repo.TodoLists.GetAllAsync(), Times.Exactly(1));
        }

        [Test]
        [TestCase(1)]
        [TestCase(2)]
        public void Details_Returns_ViewResult_If_Model_Is_Returned(int id)
        {
            // Arrange
            mock.Setup(repo => repo.TodoLists.GetById(It.IsAny<int>()).Result)
                    .Returns(GetTestTodoLists()[0]);
            var controller = new HomeController(mockWork.Object, mock.Object);

            // Act
            var result = controller.Details(id, It.IsAny<bool>(), It.IsAny<bool>()).Result;

            // Assert
            Assert.That(result, Is.TypeOf<ViewResult>());
            mock.Verify(x => x.TodoLists.GetById(It.IsAny<int>()), Times.Exactly(1));
        }

        [Test]
        [TestCase(0)]
        [TestCase(-1)]
        public void Details_Returns_RedirectToActionResult_And_Repository_Method_Is_Not_Called_If_Id_Passed_Is_Invalid(int id)
        {
            // Arrange
            mock.Setup(repo => repo.TodoLists.GetById(It.IsAny<int>()).Result)
                    .Returns(GetTestTodoLists()[0]);
            var controller = new HomeController(mockWork.Object, mock.Object);

            // Act
            var result = controller.Details(id, It.IsAny<bool>(), It.IsAny<bool>()).Result;

            // Assert
            Assert.That(result, Is.TypeOf<RedirectToActionResult>());
            mock.Verify(x => x.TodoLists.GetById(It.IsAny<int>()), Times.Exactly(0));
        }

        [Test]
        [TestCase(null)]
        public void Create_Post_Returns_RedirectToActionResult_And_Repository_Method_Is_Not_Called_If_Model_Is_Null_Or_Invalid(TodoListModel list)
        {
            // Arrange
            mock.Setup(repo => repo.TodoLists.Add(It.IsAny<TodoList>()));
            var controller = new HomeController(mockWork.Object, mock.Object);

            // Act
            var result = controller.Create(list).Result;

            // Assert
            Assert.That(result, Is.TypeOf<RedirectToActionResult>());
            mock.Verify(x => x.TodoLists.Add(It.IsAny<TodoList>()), Times.Exactly(0));
        }

        [Test]
        [TestCase(0)]
        [TestCase(-1)]
        public void Edit_Returns_RedirectToActionResult_And_Repository_Method_Is_Not_Called__If_Id_Is_Invalid(int id)
        {
            // Arrange
            mock.Setup(repo => repo.TodoLists.GetById(It.IsAny<int>()).Result)
                    .Returns(GetTestTodoLists()[0]);
            var controller = new HomeController(mockWork.Object, mock.Object);

            // Act
            var result = controller.Update(id).Result;

            // Assert
            Assert.That(result, Is.TypeOf<RedirectToActionResult>());
            mock.Verify(x => x.TodoLists.GetById(It.IsAny<int>()), Times.Exactly(0));
        }

        private List<TodoList> GetTestTodoLists()
        {
            return new List<TodoList>()
            {
                new TodoList()
                {
                    Id = 1,
                    Title = "Test TodoList #1",
                    CreationDate = System.DateTime.Today,
                    IsVisible = true,
                },
                new TodoList()
                {
                    Id = 2,
                    Title = "Test TodoList #2",
                    CreationDate = System.DateTime.Today,
                    IsVisible = false,
                },
                new TodoList()
                {
                    Id = 3,
                    Title = "Test TodoList #3",
                    CreationDate = System.DateTime.Today,
                    IsVisible = true,
                }
            };
        }
    }
}
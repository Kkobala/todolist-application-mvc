using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using todo_aspnetmvc;
using TodoList_Application;

namespace todo_aspnetmvc.Models
{
    public class TodoListModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Title is required")]
        [Display(Name = "Title of Todo List")]
        [MaxLength(50)]
        public string Title { get; set; }

        [Required, Display(Name = "Visibility")]
        public bool IsVisible { get; set; } = true;

        //[DisplayName("Last Changed")]
        //public DateTime LastModifiedDate { get; set; } = DateTime.Now; // es


        [DisplayName("Created in")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [Display(Name = "Hide Completed ToDo's")]
        public bool HideCompleted { get; set; }

        [Display(Name = "Show ToDo's Due Today")]
        public bool DueToday { get; set; }

        [MaxLength(300)]
        public string Description { get; set; } // es 

        [Display(Name = "Status of Todo List")]
        public TodoStatus Status { get; set; } // es

        [Display(Name = "Due Date")]
        public DateTime? DueDate { get; set; } // es

        public string TodoDateTime()
        {
            return DueDate.HasValue ? DueDate.Value.ToString("dd/MM/yy HH:mm") : "Date isn't Selected Yed";
        }

        public string Visibility()
        {
            return IsVisible ? "Visible" : "Not Visible";
        }
    }
}

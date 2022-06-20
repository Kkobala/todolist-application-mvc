using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
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

        [DisplayName("Created in")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [MaxLength(300)]
        public string Description { get; set; }  

        [Display(Name = "Status of Todo List")]
        public TodoStatus Status { get; set; } 

        [Display(Name = "Due Date")]
        [DataType(DataType.DateTime)]
        public DateTime DueDate { get; set; }

        public bool IsVisibleReminder { get; set; }

        public string TodoDateTime()
        {
            return  DueDate.ToString("dd/MM/yy HH:mm");
        }

        public string Visibility()
        {
            return IsVisible ? "Visible" : "Not Visible";
        }
    }
}

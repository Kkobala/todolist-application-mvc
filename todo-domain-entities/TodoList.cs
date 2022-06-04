using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using todo_aspnetmvc.Data;

namespace TodoList_Application
{
    public class TodoList
    {
        public int Id { get; set; }

        public string Title { get; set; }

        [DisplayName("Created in")]
        public DateTime CreationDate { get; set; } = DateTime.Now;

        //public bool Finished { get; set; }

        public bool IsVisible { get; set; }

        public TodoStatus Status { get; set; }

        public string Description { get; set; }

        public DateTime? DueDate { get; set; }


        //[DisplayName("Last Changed")]
        //public DateTime LastModifiedDate { get; set; } = DateTime.Now;
    }
}

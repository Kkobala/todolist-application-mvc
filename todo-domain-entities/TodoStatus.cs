using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TodoList_Application
{
    public enum TodoStatus
    {
        [Display(Name = "Completed")]

        Completed,

        [Display(Name = "In Progress")]
        InProgress,

        [Display(Name = "Not Started")]
        NotStarted
    }
}

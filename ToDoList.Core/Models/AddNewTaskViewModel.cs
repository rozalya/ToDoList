using System.ComponentModel.DataAnnotations;

namespace ToDoList.Core.Models
{
    public class AddNewTaskViewModel
    {

        [Required]
        public string Note { get; set; }    

        public string ?DueDate { get; set; }

        public bool IsImportant { get; set; }

    }
}

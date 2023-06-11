using System.ComponentModel.DataAnnotations;

namespace ToDoList.Core.Models
{
    public class AddNewTaskViewModel
    {

        [Required]
        public string Note { get; set; }

        public DateTime? DueDate { get; set; } = DateTime.Today;

        public bool IsImportant { get; set; }

    }
}

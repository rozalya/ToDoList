using System.ComponentModel.DataAnnotations;

namespace ToDoList.Core.Models
{
    public class AddNewTaskViewModel
    {
        public Guid Id { get; set; }

        [Required]
        public string Note { get; set; }

        public DateTime? DueDate { get; set; } = DateTime.Today;

        public bool IsImportant { get; set; }

    }
}

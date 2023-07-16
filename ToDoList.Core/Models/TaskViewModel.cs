using System.ComponentModel.DataAnnotations;

namespace ToDoList.Core.Models
{
    public class TaskViewModel
    {
        public Guid Id { get; set; }

        [Required]
        public string Note { get; set; }

        public DateTime? DueDate { get; set; } = DateTime.Today;

        public bool IsImportant { get; set; }
        public bool IsColsed{ get; set; }
        public DateTime? CompletedDate { get; set; }

    }
}

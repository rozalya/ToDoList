using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using ToDoList.Infrastructure.Data;

namespace ToDoList.Core.Models
{
    public class TaskViewModel
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Note is required")]
        [MaxLength(500, ErrorMessage = "Name cannot exceed 500 characters")]
        public string Note { get; set; }

        public DateTime? DueDate { get; set; } = DateTime.Today;

        public bool IsImportant { get; set; }
        public bool IsColsed{ get; set; }

        [MaxLength(500, ErrorMessage = "Step cannot exceed 500 characters")]
        [MinLength(1, ErrorMessage = "Step cannot less then 1 characters")]
        public string Step { get; set; }

        public ICollection<Step> Steps { get; set; } = new List<Step>();
        public ICollection<Statement> Statements { get; set; } = new List<Statement>();
    }
}

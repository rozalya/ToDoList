using System.ComponentModel.DataAnnotations;

namespace ToDoList.Infrastructure.Data
{
    public class DoneTask
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public string UserId { get; set; }

        [Required]
        [StringLength(500)]
        public string? Note { get; set; }

        public DateTime? DueDate { get; set; }
        public DateTime? CompletedDate { get; set; }

        [Range(typeof(bool), "true", "true")]
        public bool IsImportant { get; set; }

        [Required]
        [StringLength(500)]
        public string? ClosingStatus { get; set; }

        public Rate Rate { get; set; }
    }
}

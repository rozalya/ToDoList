using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ToDoList.Infrastructure.Data
{
    public class Statement
    {
        [Key]
        public Guid StatementId { get; set; }

        [Required]
        [MaxLength(500, ErrorMessage = "Text cannot exceed 500 characters")]
        public string If { get; set; }

        [Required]
        [MaxLength(500, ErrorMessage = "Text cannot exceed 500 characters")]
        public string Then { get; set; }

        [ForeignKey(nameof(ActiveTask))]
        public Guid TaskFK { get; set; }
        public ActiveTask ActiveTask { get; set; }
    }
}

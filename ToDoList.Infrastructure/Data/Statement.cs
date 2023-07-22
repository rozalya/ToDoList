using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ToDoList.Infrastructure.Data
{
    public class Statement
    {
        [Key]
        public Guid StatementId { get; set; }

        [Required]
        [StringLength(500)]
        public string If { get; set; }

        [Required]
        [StringLength(500)]
        public string Then { get; set; }

        [ForeignKey(nameof(ActiveTask))]
        public Guid TaskFK { get; set; }
        public ActiveTask ActiveTask { get; set; }
    }
}

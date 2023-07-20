using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ToDoList.Infrastructure.Data
{
    public class Step
    {
        [Key]
        public Guid StepId { get; set; }

        [Required]
        [StringLength(500)]
        public string Title { get; set; }

        [ForeignKey(nameof(ActiveTask))]
        public Guid TaskFK { get; set; }
        public ActiveTask ActiveTask { get; set; }
    }
}

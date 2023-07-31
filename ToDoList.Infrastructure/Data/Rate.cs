using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ToDoList.Infrastructure.Data
{
    public class Rate
    {
        [Key]
        public Guid RateId { get; set; }

        [Required]
        [Range(0, 5)]
        public int FirstStar { get; set; }

        [Required]
        [Range(0, 5)]
        public int SecondStar { get; set; }

        [Required]
        [Range(0, 5)]
        public int ThirdStar { get; set; }

        [ForeignKey(nameof(DoneTask))]
        public Guid TaskFK { get; set; }
        public DoneTask DoneTask{ get; set; }
    }
}

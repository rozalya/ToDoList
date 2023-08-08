using System.ComponentModel.DataAnnotations;

namespace ToDoList.Core.Models
{
    public class StepsViewModel
    {
        [MaxLength(500, ErrorMessage = "Step cannot exceed 500 characters")]
        public string Step { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace ToDoList.Core.Models
{
    public class StatementsViewModel
    {
        [MaxLength(500, ErrorMessage = "Step cannot exceed 500 characters")]
        [MinLength(1, ErrorMessage = "If state cannot be less then 1 character")]
        public string If { get; set; }


        [MaxLength(500, ErrorMessage = "Step cannot exceed 500 characters")]
        [MinLength(1, ErrorMessage = "Then state cannot be less then 1 character")]
        public string Then { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace ToDoList.Core.Models
{
    public class CreateRoleViewModel
    {
        [Required]
        public string RoleName { get; set; }
    }
}

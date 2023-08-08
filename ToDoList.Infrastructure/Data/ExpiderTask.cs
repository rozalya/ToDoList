﻿using System.ComponentModel.DataAnnotations;

namespace ToDoList.Infrastructure.Data
{
    public class ExpiderTask
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public string UserId { get; set; }

        [Required]
        [MaxLength(500, ErrorMessage = "Text cannot exceed 500 characters")]
        public string? Note { get; set; }

        public DateTime? DueDate { get; set; }

        [Range(typeof(bool), "true", "true")]
        public bool IsImportant { get; set; }
    }
}

﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoList.Infrastructure.Data
{
    public class NewTask
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public string UserId { get; set; }

        [Required]
        [StringLength(500)]
        public string? Note { get; set; }

        public string? DueDate { get; set; }

        [Range(typeof(bool), "true", "true")]
        public bool IsImportant { get; set; }
    }
}
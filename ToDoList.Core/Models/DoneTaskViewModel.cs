using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoList.Infrastructure.Data;

namespace ToDoList.Core.Models
{
    public class DoneTaskViewModel : TaskViewModel
    {
        public Rate Rate { get; set; }
    }
}

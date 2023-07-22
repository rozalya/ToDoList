using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoList.Core.Contracts
{
    public interface IStatementService
    {
        Task AddStatement(string IfText, string ThenText, Guid taskId);
    }
}

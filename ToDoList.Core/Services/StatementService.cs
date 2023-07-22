using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoList.Core.Contracts;
using ToDoList.Infrastructure.Data;
using ToDoList.Infrastructure.Data.Repositories;

namespace ToDoList.Core.Services
{
    public class StatementService : IStatementService
    {
        private readonly IApplicatioDbRepository repo;
        public StatementService(IApplicatioDbRepository _repo)
        {
            repo = _repo;
        }

        public async Task AddStatement(string IfText, string ThenText, Guid taskId)
        {
            var task = await repo.GetByIdAsync<ActiveTask>(taskId);

            var statemetToAdd = new Statement()
            {
               If = IfText,
               Then = ThenText
            };

            task.Statements.Add(statemetToAdd);
            repo.SaveChanges();
        }
    }
}

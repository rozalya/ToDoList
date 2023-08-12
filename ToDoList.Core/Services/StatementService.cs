using System.Web;
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

        /// <summary>
        /// Add Statement for the current task.
        /// </summary>
        /// <param name="IfText"></param>
        /// <param name="ThenText"></param>
        /// <param name="taskId"></param>
        /// <returns></returns>
        public async Task AddStatement(string IfText, string ThenText, Guid taskId)
        {
            var task = await repo.GetByIdAsync<ActiveTask>(taskId);

            var statemetToAdd = new Statement()
            {
               If = HttpUtility.HtmlEncode(IfText),
               Then = HttpUtility.HtmlEncode(ThenText)
            };

            task.Statements.Add(statemetToAdd);
            repo.SaveChanges();
        }
    }
}

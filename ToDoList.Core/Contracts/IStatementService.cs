namespace ToDoList.Core.Contracts
{
    public interface IStatementService
    {
        Task AddStatement(string IfText, string ThenText, Guid taskId);
    }
}

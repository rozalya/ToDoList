using System.Web;
using ToDoList.Core.Models;
using ToDoList.Infrastructure.Data;

namespace ToDoList.Core.Services.CommonUtils
{
    public static class Common
    {
        /// <summary>
        /// Escapes all special symbols with encoding. 
        /// </summary>
        /// <param name="taskViewModel"></param>
        /// <returns></returns>
        public static TaskViewModel EncodeTask(TaskViewModel taskViewModel)
        {
            var encodedTask = new TaskViewModel();
            encodedTask.Note = HttpUtility.HtmlEncode(taskViewModel.Note);

            if (taskViewModel.Steps.Count != null)
            {
                taskViewModel.Steps.ToList().ForEach(step =>
                {
                    step.Title = HttpUtility.HtmlEncode(step.Title);
                    encodedTask.Steps.Add(step);
                });
            }

            if (taskViewModel.Statements.Count != null)
            {
                taskViewModel.Statements.ToList().ForEach(Stat =>
                {
                    Stat.If = HttpUtility.HtmlEncode(Stat.If);
                    Stat.Then = HttpUtility.HtmlEncode(Stat.Then);
                    encodedTask.Statements.Add(Stat);
                });
            }  
            
            return encodedTask;
        }

        /// <summary>
        /// Decode all special symbols that are escaped into DB.
        /// </summary>
        /// <param name="activeTask"></param>
        /// <returns></returns>
        public static ActiveTask DecodeTask(ActiveTask activeTask)
        {
            var decodedTask = activeTask;
            decodedTask.Note = HttpUtility.HtmlDecode(activeTask.Note);

            if (activeTask.Steps.Count > 0)
            {
                activeTask.Steps.ToList().ForEach(step =>
                {
                    step.Title = HttpUtility.HtmlDecode(step.Title);
                    decodedTask.Steps.Add(step);
                });
            }

            if (activeTask.Statements.Count > 0)
            {
                activeTask.Statements.ToList().ForEach(Stat =>
                {
                    Stat.If = HttpUtility.HtmlDecode(Stat.If);
                    Stat.Then = HttpUtility.HtmlDecode(Stat.Then);
                    decodedTask.Statements.Add(Stat);
                });
            }

            return decodedTask;
        }

        /// <summary>
        /// Decode all special symbols that are escaped into DB.
        /// </summary>
        /// <param name="steps"></param>
        /// <returns></returns>
        public static List<Step> DecodeSteps(List<Step> steps)
        {
            if (steps.Count != null)
            {
                var decodedSteps = new List<Step>();
                steps.ForEach(step =>
                {
                    step.Title = HttpUtility.HtmlDecode(step.Title);
                    decodedSteps.Add(step);
                });
                return decodedSteps;
            }
            return steps;
        }

        /// <summary>
        /// Decode all special symbols that are escaped into DB.
        /// </summary>
        /// <param name="statements"></param>
        /// <returns></returns>
        public static List<Statement> DecodeStatements(List<Statement> statements)
        {
            if (statements.Count != null)
            {
                var decodedStatements = new List<Statement>();
                statements.ForEach(state =>
                {
                    state.If = HttpUtility.HtmlDecode(state.If);
                    state.Then = HttpUtility.HtmlDecode(state.Then);
                    decodedStatements.Add(state);
                });
                return decodedStatements;
            }
            return statements;
        }
    }
}

using Microsoft.EntityFrameworkCore;
using ToDoListConsole.Data;
using ToDoListConsole.Models;

namespace ToDoListConsole
{
    public record TaskItem (int? Priority, string Title, string Description, string? TaskStatus);

    public class RecordManager
    {
        public static TaskItem AskInfo()
        {
            string? status = "";

            Console.Write("What is the task priority?: ");
            int.TryParse(Console.ReadLine(), out int taskPriority);

            Console.Write("What is name of task?: ");
            string taskName = Console.ReadLine() ?? "";

            Console.Write("Write your description for task: ");
            string taskDesc = Console.ReadLine() ?? "";

            Console.Write("Is it completed?:\nN Not completed\nY Completed\nS Started\nD In Progress\nEnter current status:");
            string? statusInput = Console.ReadLine()?.Trim().ToLower();

            switch(statusInput)
            {
                case "y":
                    status = IsCompletedEnum.Completed.ToString();
                    break;
                case "n":
                    status = IsCompletedEnum.NotCompleted.ToString();
                    break;
                case "s":
                    status = IsCompletedEnum.Started.ToString();
                    break;
                case "d":
                    status = IsCompletedEnum.Doing.ToString();
                    break;

            }



                return new TaskItem(taskPriority, taskName, taskDesc, status);
        }

        public static List<ToDoRecords> ReadFromDatabase(ToDoContext db)
        {
            return db.SaveRecords
                     .AsNoTracking()
                     .OrderBy(r => r.Priority)
                     .ToList();
        }

        public static void SaveToDatabase(TaskItem item, ToDoContext db)
        {
            var entity = new ToDoRecords
            {
                Priority = item.Priority ?? 0,
                Title = item.Title,
                Description = item.Description,
                TaskStatus = item.TaskStatus,
                DateTime = DateTime.UtcNow
            };

            db.SaveRecords.Add(entity);
            db.SaveChanges();
        }

        public static string Main(ToDoContext db)
        {
            var task = AskInfo();
            SaveToDatabase(task, db);
            return "Saved";
        }
    }
}

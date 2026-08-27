using ToDoListConsole.Data;
using ToDoListConsole.Models;

namespace ToDoListConsole
{
    public static class DatabaseWriteQuery
    {
        public static int Write(TaskItem item, ToDoContext db)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));
            if (db == null) throw new ArgumentNullException(nameof(db));

            if (item.Priority.HasValue)
            {
                bool exists = db.SaveRecords.Any(r => r.Priority == item.Priority.Value);
                if (exists)
                {
                    Console.Clear();
                    Console.WriteLine($"A record with Id {item.Priority.Value} already exists; please choose another Id.");
                }
                else
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
                    return db.SaveChanges();
                }
            }
            return 0;
        }
    }
}

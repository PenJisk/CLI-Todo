using Microsoft.EntityFrameworkCore;
using ToDoListConsole.Data;
using ToDoListConsole.Models;

namespace ToDoListConsole.Queries
{
    public static class DatabaseReadQuery
    {
        public static List<ToDoRecords> Read(ToDoContext db)
        {
            return db.SaveRecords
                     .AsNoTracking()
                     .OrderBy(r => r.Priority)
                     .ToList();

        }
    }
}


using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ToDoListConsole;
using ToDoListConsole.Queries;

public class Todo
{
    public class InvalidUserChoiceException : Exception
    {
        public InvalidUserChoiceException(string message) : base(message) { }
    }

    public static void Main(string[] args)
    {
        var host = Host.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration((ctx, cfg) =>
            {
                cfg.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            })
            .ConfigureServices((ctx, services) =>
            {
                var conn = ctx.Configuration.GetConnectionString("ToDoDatabase");
                services.AddDbContext<ToDoListConsole.Data.ToDoContext>(options =>
                    options.UseSqlite(conn));
            })
            .Build();

        using (var scope = host.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<ToDoListConsole.Data.ToDoContext>();
            db.Database.Migrate();
        }
        Console.WriteLine("Welcome to the Todo List Application!");
        Console.WriteLine("What do to?:\n1.Create a task\n2.List added tasks\n3.Change status of a task\n4.Delete a task\n5.Clear all tasks!\n");
        Console.Write("Enter a number: ");
        string? UserInput = Console.ReadLine();

        try
        {
            if (!int.TryParse(UserInput, out int userChoice))
                throw new InvalidUserChoiceException("Wrong input format");

            switch (userChoice)
            {
                case 1:
                    Console.Clear();
                    var item = RecordManager.AskInfo();
                    using (var scope = host.Services.CreateScope())
                    {
                        var db = scope.ServiceProvider.GetRequiredService<ToDoListConsole.Data.ToDoContext>();
                        var rows = DatabaseWriteQuery.Write(item, db);
                        Console.WriteLine($"Saved {rows} row(s).");
                    }
                    break;

                case 2:
                    Console.Clear();
                    using (var scope = host.Services.CreateScope())
                    {
                        var db = scope.ServiceProvider.GetRequiredService<ToDoListConsole.Data.ToDoContext>();
                        var records = DatabaseReadQuery.Read(db);
                        Console.WriteLine("Your tasks are:");
                        if (records.Count == 0) Console.WriteLine("  (no tasks found)");
                        else
                        {
                            foreach (var r in records)
                                Console.WriteLine($"  Id:{r.Id} Priority:{r.Priority} Title:{r.Title} Completed:{r.TaskStatus} Date:{r.DateTime}");
                        }
                    }
                    break;

                case 3:
                    Console.Clear();
                    using (var scope = host.Services.CreateScope())
                    {
                        var db = scope.ServiceProvider.GetRequiredService<ToDoListConsole.Data.ToDoContext>();
                        var records = DatabaseReadQuery.Read(db);
                        Console.WriteLine("Which task do you want to change the status of? (Enter priority)");
                        string? userInput = Console.ReadLine();
                        int inputNumber = userInput != null && int.TryParse(userInput, out int result) ? result : 0;

                        foreach (var r in records)
                        {
                            if (r.Priority == inputNumber)
                                Console.WriteLine($"Your task is in {r.TaskStatus} status\ntype and enter new status" +
                                    $"\nN Not completed\nY Completed\nS Started\nD In Progress\nEnter current status:");
                            string? statusInput = Console.ReadLine()?.Trim().ToLower();

                            switch (statusInput)
                            {
                                case "y":
                                    r.TaskStatus = IsCompletedEnum.Completed.ToString();
                                    break;
                                case "n":
                                    r.TaskStatus = IsCompletedEnum.NotCompleted.ToString();
                                    break;
                                case "s":
                                    r.TaskStatus = IsCompletedEnum.Started.ToString();
                                    break;
                                case "d":
                                    r.TaskStatus = IsCompletedEnum.Doing.ToString();
                                    break;

                            }
                            db.SaveRecords.UpdateRange(records);
                            db.SaveChanges();
                            Console.WriteLine($"Task with priority {inputNumber} status changed to {r.TaskStatus}");
                            break;

                        }
                    }
                    break;
                case 4:
                    Console.Clear();
                    using (var scope = host.Services.CreateScope())
                    {
                        var db = scope.ServiceProvider.GetRequiredService<ToDoListConsole.Data.ToDoContext>();
                        var records = DatabaseReadQuery.Read(db);
                        Console.WriteLine("Which task do you want to change the status of? (Enter priority)");
                        string? userInput = Console.ReadLine();
                        int inputNumber = userInput != null && int.TryParse(userInput, out int result) ? result : 0;

                        foreach (var r in records)
                        {
                            if (r.Priority == inputNumber)
                            {
                                db.SaveRecords.Remove(r);
                                db.SaveChanges();
                                Console.WriteLine($"Task with priority {inputNumber} removed.");
                                break;
                            }
                        }
                        break;
                    } 
                        
                case 5:
                    using (var scope = host.Services.CreateScope())
                    {
                        var db = scope.ServiceProvider.GetRequiredService<ToDoListConsole.Data.ToDoContext>();
                        var records = DatabaseReadQuery.Read(db);

                        if (records.Count == 0)
                        {
                            Console.WriteLine("No records to remove.");
                        }
                        else
                        {
                            db.SaveRecords.RemoveRange(records);
                            var removed = db.SaveChanges();
                            Console.WriteLine($"{removed} record(s) removed.");
                        }
                        Console.WriteLine("All data cleared.");
                    }
                    break;
            }
        }
        catch (InvalidUserChoiceException ex)
        {
            Console.WriteLine($"Please enter a valid number!\nError is {ex}");
        }
    }
}

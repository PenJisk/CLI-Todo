using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace ToDoListConsole
{
    public interface IItem
    {
        int Priority { get; set; }
        string? Title { get; set; }
        string? Description { get; set; }
        bool Status { get; set; }
    }
}

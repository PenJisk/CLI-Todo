using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace ToDoListConsole.Models
{
    public partial class ToDoRecords
    {
        [Key]
        public int Id { get; set; }
        public int Priority { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string? TaskStatus { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime? DateTime {get; set; }

    }
}

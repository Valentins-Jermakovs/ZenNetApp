using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// Šo vajag pieslēgt, lai varētu veidot tabulas - obligāts
using Microsoft.EntityFrameworkCore;

namespace ZenNetApp.Models
{
    public class Reader
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        // Foreign Key uz Book tabulu
        public List<Book> BorrowedBooks { get; set; } = new List<Book>();
    }
}

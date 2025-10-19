// Šo vajag pieslēgt, lai varētu veidot tabulas - obligāts
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZenNetApp.Models
{
    public class Book
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Range(1000, 2100)]
        public int Year { get; set; }

        // Foreign Key uz Author, Genre un Reader tabulām
        // FK uz Author tabulu
        [Required]
        public int AuthorId { get; set; }
        public Author Author { get; set; }
        // FK uz Genre tabulu
        [Required]
        public int GenreId { get; set; }
        public Genre Genre { get; set; }
        // FK uz Reader tabulu

        public int? BorrowedById { get; set; }
        public Reader? BorrowedBy { get; set; }
        // FK uz Publisher tabulu
        [Required]
        public int PublisherId { get; set; }
        public Publisher Publisher { get; set; }

    }
}

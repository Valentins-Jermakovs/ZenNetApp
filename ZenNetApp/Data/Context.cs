using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// Šo vajag pieslēgt, lai varētu veidot tabulas - obligāts
using Microsoft.EntityFrameworkCore;
using ZenNetApp.Models;

namespace ZenNetApp.Data
{
    public class Context: DbContext
    {
        public DbSet<Book> Books { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Publisher> Publishers { get; set; }
        public DbSet<Reader> Readers { get; set; }

        // Konstruuktors, kas pieņem DbContextOptions un nodod to pamatklasei
        public Context(DbContextOptions<Context> options)
            : base(options)
        {
        }

        // Noklusētais konstruktors
        public Context()
        {
        }

        // Šeit konfigurējam savienojumu ar SQL Server datubāzi
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Pārbaudām, vai opcijas jau nav konfigurētas
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=LOQ;Database=ZenNetDB;Trusted_Connection=True;TrustServerCertificate=true");
            }
            // Izsaucam pamatklases metodi
            base.OnConfiguring(optionsBuilder);
        }

        // Tabulu konfigurācijas, dzēšana
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Šeit var pievienot papildu konfigurācijas, ja nepieciešams
            base.OnModelCreating(modelBuilder);

            // Grāmata uz autoru
            modelBuilder.Entity<Book>()
                .HasOne(b => b.Author)
                .WithMany(a => a.Books)
                .HasForeignKey(b => b.AuthorId)
                .OnDelete(DeleteBehavior.Restrict);
            // Grāmata uz izdevēju
            modelBuilder.Entity<Book>()
                .HasOne(b => b.Publisher)
                .WithMany(p => p.Books)
                .HasForeignKey(b => b.PublisherId)
                .OnDelete(DeleteBehavior.Restrict);
            // Grāmata uz žanru
            modelBuilder.Entity<Book>()
                .HasOne(b => b.Genre)
                .WithMany(g => g.Books)
                .HasForeignKey(b => b.GenreId)
                .OnDelete(DeleteBehavior.Restrict);
            // Grāmata uz lasītāju
            modelBuilder.Entity<Book>()
                .HasOne(b => b.BorrowedBy)
                .WithMany(r => r.BorrowedBooks)
                .HasForeignKey(b => b.BorrowedById)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}

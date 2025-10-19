using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ZenNetApp.Data;
using ZenNetApp.Models;
namespace ZenNetApp.Controllers
{
    public class BookController : Controller
    {
        // Pievienojam kontekstu
        private readonly Context _context; // datubāzes konteksts - lauks
        public BookController(Context context) // konstruktors
        {
            _context = context;
        }
        // GET: BookController
        public ActionResult Index()
        {
            var books = _context.Books
                .Include(b => b.Author)
                .Include(b => b.Genre)
                .Include(b => b.Publisher)
                .Include(b => b.BorrowedBy)
                .ToList();
            return View(books);
        }

        // GET: BookController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: BookController/Create
        public ActionResult Create()
        {
            ViewBag.Authors = new SelectList(_context.Authors, "Id", "Name");
            ViewBag.Genres = new SelectList(_context.Genres, "Id", "Name");
            ViewBag.Publishers = new SelectList(_context.Publishers, "Id", "Name");
            ViewBag.Readers = new SelectList(_context.Readers, "Id", "Name");
            return View();
        }

        // POST: BookController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Book book)
        {
            // Izveidojam SelectList priekš Author, Genre, Publisher un Reader
            ViewBag.Authors = new SelectList(_context.Authors, "Id", "Name");
            ViewBag.Genres = new SelectList(_context.Genres, "Id", "Name");
            ViewBag.Publishers = new SelectList(_context.Publishers, "Id", "Name");
            ViewBag.Readers = new SelectList(_context.Readers, "Id", "Name");

            // Pārbaudām validāciju
            _context.Books.Add(book);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
            
            //return View(book);
        }

        // GET: BookController/Edit/5
        public ActionResult Edit(int id)
        {
            ViewBag.Authors = new SelectList(_context.Authors, "Id", "Name");
            ViewBag.Genres = new SelectList(_context.Genres, "Id", "Name");
            ViewBag.Publishers = new SelectList(_context.Publishers, "Id", "Name");
            ViewBag.Readers = new SelectList(_context.Readers, "Id", "Name");
            // iegūstam grāmatu pēc Id
            var book = _context.Books.Find(id);
            if (book == null)
            {
                return NotFound();
            }
            return View(book);
        }

        // POST: BookController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id,Book book)
        {
            ViewBag.Authors = new SelectList(_context.Authors, "Id", "Name");
            ViewBag.Genres = new SelectList(_context.Genres, "Id", "Name");
            ViewBag.Publishers = new SelectList(_context.Publishers, "Id", "Name");
            ViewBag.Readers = new SelectList(_context.Readers, "Id", "Name");
            // Pārbaudām, vai viss sakrīt
            if (id != book.Id)
            {
                return NotFound();
            }
            
            _context.Update(book);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
            
        }

        // GET: BookController/Delete/5
        public ActionResult Delete(int id)
        {
            var book = _context.Books.Find(id);

            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        // POST: BookController/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var book = _context.Books.Find(id);
            if (book == null)
            {
                return NotFound();
            }
           
            
            _context.Books.Remove(book);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
            
        }
    }
}

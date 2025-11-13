using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ZenNetApp.Data;
using ZenNetApp.Models;

namespace ZenNetApp.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly Context _context;

        public BooksController(Context context)
        {
            _context = context;
        }

        // ======== LINQ GET methods =============================================
        // search all books
        // GET: api/Books - stock method
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Book>>> GetBooks()
        {
            return await _context.Books.ToListAsync();
        }
        // search book by id
        // GET: api/Books/5 - stock method
        [HttpGet("{id}")]
        public async Task<ActionResult<Book>> GetBook(int id)
        {
            var book = await _context.Books.FindAsync(id);

            if (book == null)
            {
                return NotFound();
            }

            return book;
        }
        // GET sorted by title
        [HttpGet("sortedByTitle")]
        public async Task<ActionResult<IEnumerable<Book>>> GetBooksSortedByTitle()
        {
            return await _context.Books
                .OrderBy(b => b.Title)
                .ToListAsync();
        }
        // GET sorted by year
        [HttpGet("sortedByYear")]
        public async Task<ActionResult<IEnumerable<Book>>> GetBooksSortedByYear()
        {
            return await _context.Books
                .OrderBy(b => b.Year)
                .ToListAsync();
        }
        // ========================================================================

        // PUT: api/Books/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBook(int id, Book book)
        {
            if (id != book.Id)
            {
                return BadRequest();
            }

            _context.Entry(book).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BookExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Books
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Book>> PostBook(Book book)
        {
            _context.Books.Add(book);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetBook", new { id = book.Id }, book);
        }

        // ===== LINQ POST ============================================================
        // 3 LINQ POST - search methods
        // Search by book title
        [HttpPost("searchByTitle")]
        public async Task<ActionResult<IEnumerable<Book>>> SearchByTitle([FromBody] string title)
        {
            var books = await _context.Books
                .Where(b => b.Title.ToLower().Contains(title.ToLower()))
                .ToListAsync();

            if (books.Count == 0)
            {
                return NotFound();
            }

            return books;
        }
        // Search by year
        [HttpPost("searchByYear")]
        public async Task<ActionResult<IEnumerable<Book>>> GetBooksNewerThan([FromBody] int year)
        {
            var books = await _context.Books
                .Where(b => b.Year == year)
                .ToListAsync();

            if (books.Count == 0)
            {
                return NotFound();
            }

            return books;
        }
        // Search books by author
        [HttpPost("searchByAuthor")]
        public async Task<ActionResult<IEnumerable<Book>>> GetBooksByAuthor([FromBody] string author)
        {
            var books = await _context.Books
                .Where(b => b.Author.Name.ToLower().Contains(author.ToLower()))
                .ToListAsync();

            if (books.Count == 0)
            {
                return NotFound();
            }

            return books;
        }
        // ============================================================================

        // ===== LINQ DELETE =============
        // DELETE: api/Books/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBook(int id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book == null)
            {
                return NotFound();
            }

            _context.Books.Remove(book);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool BookExists(int id)
        {
            return _context.Books.Any(e => e.Id == id);
        }
    }
}

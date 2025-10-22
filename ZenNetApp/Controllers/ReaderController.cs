using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using Microsoft.EntityFrameworkCore;
using ZenNetApp.Data;
using ZenNetApp.Models;

namespace ZenNetApp.Controllers
{
    public class ReaderController : Controller
    {
        // Pievienojam kontekstu
        private readonly Context _context; // datubāzes konteksts - lauks
        public ReaderController(Context context) // konstruktors
        {
            _context = context;
        }
        // GET: ReaderController
        public ActionResult Index()
        {
            var readers = _context.Readers
                .ToList();
            return View(readers);
        }

        // GET: ReaderController/Details/5
        public ActionResult Details(int id)
        {
            var reader = _context.Readers
                .Include(r => r.BorrowedBooks) // Iekļauj saistītās grāmatas
                .FirstOrDefault(r => r.Id == id);

            if (reader == null)
            {
                return NotFound();
            }

            return View(reader);
        }

        // GET: ReaderController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ReaderController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create( Reader reader)
        {
            if (ModelState.IsValid)
            {
                _context.Readers.Add(reader);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(reader);
        }

        // GET: ReaderController/Edit/5
        public ActionResult Edit(int id)
        {
            var reader = _context.Readers.Find(id);
            if (reader == null)
            {
                return NotFound();
            }
            return View(reader);
        }

        // POST: ReaderController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Reader reader)
        {
            // pārbaudam vai id sakrīt
            if (id != reader.Id)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(reader);
                    _context.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Readers.Any(e => e.Id == reader.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(reader);
        }

        // GET: ReaderController/Delete/5
        public ActionResult Delete(int id)
        {
            var reader = _context.Readers.Find(id);
            if (reader == null)
            {
                return NotFound();
            }
            return View(reader);
        }

        // POST: ReaderController/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var reader = _context.Readers.Find(id);
            if (reader == null)
            {
                return NotFound();
            }
            _context.Readers.Remove(reader);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }
}

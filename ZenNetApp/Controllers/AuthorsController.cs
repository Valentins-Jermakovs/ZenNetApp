using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using ZenNetApp.Data;
using ZenNetApp.Models;
using Microsoft.EntityFrameworkCore;

namespace ZenNetApp.Controllers
{
    public class AuthorsController : Controller
    {
        // Iegūstam context
        private readonly Context _context;
        public AuthorsController(Context context)
        {
            _context = context;
        }
        // GET: AuthorsController
        public ActionResult Index()
        {
            var authors = _context.Authors
                .ToList();
            return View(authors);
        }

        // GET: AuthorsController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: AuthorsController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AuthorsController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create( Author author)
        {
            if (ModelState.IsValid)
            {
                _context.Authors.Add(author);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(author);
        }

        // GET: AuthorsController/Edit/5
        public ActionResult Edit(int id)
        {
            // Iegūstam autoru pēc id
            var author = _context.Authors.Find(id);
            if (author == null) {
                return NotFound();
            }

            return View(author);
        }

        // POST: AuthorsController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Author author)
        {
            // Pārbaudām, vai id sakrīt
            if (id != author.Id)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(author);
                    _context.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Authors.Any(e => e.Id == author.Id))
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
            return View(author);
        }

        // GET: AuthorsController/Delete/5
        public ActionResult Delete(int id)
        {
            // Iegūstam autoru pēc id
            var author = _context.Authors.Find(id);
            if (author == null)
            {
                return NotFound();
            }

            return View(author);
        }

        // POST: AuthorsController/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            // Iegūstam autoru pēc id
            var author = _context.Authors.Find(id);
            if (author == null)
            {
                return NotFound();
            }
            try
            {
                _context.Authors.Remove(author);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException)
            {
                // Ja ir izņēmums, pāradresējam uz DeleteError skatu
                TempData["ErrorMessage"] = "Neizdevās dzēst autoru. Iespējams, ir saistīti ieraksti.";
                return RedirectToAction(nameof(Index));
            }
        }
    }
}

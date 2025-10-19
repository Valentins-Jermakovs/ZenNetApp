using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using Microsoft.EntityFrameworkCore;
using ZenNetApp.Data;
using ZenNetApp.Models;

namespace ZenNetApp.Controllers
{
    public class GenreController : Controller
    {
        // Pievienojam kontekstu
        private readonly Context _context; // datubāzes konteksts - lauks
        public GenreController(Context context) // konstruktors
        {
            _context = context;
        }

        // GET: GenreController
        public ActionResult Index()
        {
            var genres = _context.Genres
                .ToList();
            return View(genres);
        }

        // GET: GenreController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: GenreController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: GenreController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create( Genre genre)
        {
            if (ModelState.IsValid)
            {
                _context.Genres.Add(genre);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(genre);
        }

        // GET: GenreController/Edit/5
        public ActionResult Edit(int id)
        {
            // iegūstam pēc id
            var genre = _context.Genres.Find(id);
            if (genre == null)
            {
                return NotFound();
            }
            return View(genre);
        }

        // POST: GenreController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Genre genre)
        {
            // pārbaudam vai id sakrīt
            if (id != genre.Id)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(genre);
                    _context.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Genres.Any(e => e.Id == genre.Id))
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
            return View(genre);
        }

        // GET: GenreController/Delete/5
        public ActionResult Delete(int id)
        {
            // iegūstam žanru pēc id
            var genre = _context.Genres.Find(id);
            if (genre == null)
            {
                return NotFound();
            }
            return View(genre);
        }

        // POST: GenreController/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            // Iegūstam žanru pēc id
            var genre = _context.Genres.Find(id);
            if (genre == null)
            {
                return NotFound();
            }
            try
            {
                _context.Genres.Remove(genre);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException)
            {
                // Ja ir izņēmums, pāradresējam uz DeleteError skatu
                TempData["ErrorMessage"] = "Neizdevās dzēst žanru. Iespējams, ir saistīti ieraksti.";
                return RedirectToAction(nameof(Index));
            }
        }
    }
}

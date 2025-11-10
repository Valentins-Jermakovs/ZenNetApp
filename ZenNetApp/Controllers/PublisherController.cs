using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using Microsoft.EntityFrameworkCore;
using ZenNetApp.Data;
using ZenNetApp.Models;

namespace ZenNetApp.Controllers
{
    public class PublisherController : Controller
    {
        // Pievienojam kontekstu
        private readonly Context _context; // datubāzes konteksts - lauks
        public PublisherController(Context context) // konstruktors
        {
            _context = context;
        }
        // GET: PublisherController
        public ActionResult Index()
        {
            var publishers = _context.Publishers
                .ToList();
            return View(publishers);
        }

        // GET: PublisherController/Details/5
        public ActionResult Details(int id)
        {
            var publisher = _context.Publishers
                .Include(p => p.Books) // Iekļauj saistītās grāmatas
                .FirstOrDefault(p => p.Id == id);
            
            if (publisher == null)
            {
                return NotFound();
            }

            return View(publisher);
        }

        // GET: PublisherController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: PublisherController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Publisher publisher )
        {
            if (ModelState.IsValid)
            {
                _context.Publishers.Add(publisher);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(publisher);
        }

        // GET: PublisherController/Edit/5
        public ActionResult Edit(int id)
        {
            // iegūstam pēc id
            var publisher = _context.Publishers.Find(id);
            if (publisher == null)
            {
                return NotFound();
            }
            return View(publisher);
        }

        // POST: PublisherController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Publisher publisher)
        {
            // Pārbaudām, vai id sakrīt
            if (id != publisher.Id)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(publisher);
                    _context.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Publishers.Any(e => e.Id == publisher.Id))
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
            return View(publisher);
        }

        // GET: PublisherController/Delete/5
        public ActionResult Delete(int id)
        {
            // iegūstam pēc id
            var publisher = _context.Publishers.Find(id);
            if (publisher == null)
            {
                return NotFound();
            }
            return View(publisher);
        }

        // POST: PublisherController/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            // Iegūstam autoru pēc id
            var publisher = _context.Publishers.Find(id);
            if (publisher == null)
            {
                return NotFound();
            }
            try
            {
                _context.Publishers.Remove(publisher);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException)
            {
                // Ja ir izņēmums, pāradresējam uz DeleteError skatu
                TempData["ErrorMessage"] = "Neizdevās dzēst izdevēju. Iespējams, ir saistīti ieraksti.";
                return RedirectToAction(nameof(Index));
            }
        }

        // Metodes izdevēja un saistīto grāmatu meklēšanai

        // GET: PublisherController/Search
        public ActionResult Search()
        {
            return View();
        }
        // POST: PublisherController/SearchResults
        [HttpPost]
        public ActionResult Search(string publisherName)
        {
            if (string.IsNullOrWhiteSpace(publisherName))
            {
                return View();
            }

            var results = _context.Publishers
                .Where(p => p.Name.Contains(publisherName))
                .Include(p => p.Books) // Iekļauj saistītās grāmatas
                .ToList();
            return View(results);
        }
    }
}

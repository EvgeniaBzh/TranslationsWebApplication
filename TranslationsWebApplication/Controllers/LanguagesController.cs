using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TranslationsWebApplication.Models;

namespace TranslationsWebApplication.Controllers
{
    public class LanguagesController : Controller
    {
        private readonly DbtranslationAgencyContext _context;

        public LanguagesController(DbtranslationAgencyContext context)
        {
            _context = context;
        }

        // GET: Languages
        public async Task<IActionResult> Index()
        {
              return _context.Languages != null ? 
                          View(await _context.Languages.ToListAsync()) :
                          Problem("Entity set 'DbtranslationAgencyContext.Languages'  is null.");
        }

        // GET: Languages/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Languages == null)
            {
                return NotFound();
            }

            var language = await _context.Languages
                 .Include(l => l.OriginalLanguage)
                     .ThenInclude(o => o.Type)
                .Include(t => t.OriginalLanguage)
                    .ThenInclude(o => o.Topic)
         .FirstOrDefaultAsync(m => m.LanguageId == id);


            if (language == null)
            {
                return NotFound();
            }

            return View(language);
        }

        // GET: Languages/DetailsOriginal/5
        public async Task<IActionResult> DetailsOriginal(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var orders = await _context.Orders
                .Include(o => o.OriginalLanguage)
                .Include(o => o.TranslationLanguage)
                .Include(o => o.Type)
                .Include(o => o.Topic)
                .Where(o => o.OriginalLanguageId == id).ToListAsync();

            if (orders == null)
            {
                return NotFound();
            }

            return View("DetailsOriginal", orders); // Використовуйте вигляд, що показує замовлення
        }


        // GET: Languages/DetailsTranslation/5
        public async Task<IActionResult> DetailsTranslation(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var orders = await _context.Orders
                .Include(o => o.TranslationLanguage)
                .Include(o => o.OriginalLanguage)
                .Include(o => o.Type)
                .Include(o => o.Topic)
                .Where(o => o.TranslationLanguageId == id).ToListAsync();

            if (orders == null)
            {
                return NotFound();
            }

            return View("DetailsTranslation", orders); // Використовуйте той же вигляд, що і для DetailsOriginal
        }


        // GET: Languages/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Languages/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("LanguageId,LanguageName")] Language language)
        {
            if (ModelState.IsValid)
            {
                // Перевірка на унікальність назви теми
                var languageExists = await _context.Languages.AnyAsync(t => t.LanguageName == language.LanguageName);
                if (languageExists)
                {
                    ModelState.AddModelError("LanguageName", "A language with that name already exists.");
                    return View(language);
                }

                _context.Add(language);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(language);
        }

        // GET: Languages/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Languages == null)
            {
                return NotFound();
            }

            var language = await _context.Languages.FindAsync(id);
            if (language == null)
            {
                return NotFound();
            }
            return View(language);
        }

        // POST: Languages/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("LanguageId,LanguageName")] Language language)
        {
            if (id != language.LanguageId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var languageExists = await _context.Languages
                                                 .AnyAsync(t => t.LanguageName == language.LanguageName && t.LanguageId != language.LanguageId);
                if (languageExists)
                {
                    ModelState.AddModelError("LanguageName", "A language with that name already exists.");
                    return View(language);
                }

                try
                {
                    _context.Update(language);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LanguageExists(language.LanguageId))
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
            return View(language);
        }

        // GET: Languages/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Languages == null)
            {
                return NotFound();
            }

            var language = await _context.Languages
                .FirstOrDefaultAsync(m => m.LanguageId == id);
            if (language == null)
            {
                return NotFound();
            }

            return View(language);
        }

        // POST: Languages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Languages == null)
            {
                return Problem("Entity set 'DbtranslationAgencyContext.Languages'  is null.");
            }
            var language = await _context.Languages.FindAsync(id);
            if (language != null)
            {
                _context.Languages.Remove(language);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LanguageExists(int id)
        {
          return (_context.Languages?.Any(e => e.LanguageId == id)).GetValueOrDefault();
        }
    }
}

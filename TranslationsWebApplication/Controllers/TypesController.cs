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
    public class TypesController : Controller
    {
        private readonly DbtranslationAgencyContext _context;

        public TypesController(DbtranslationAgencyContext context)
        {
            _context = context;
        }

        // GET: Types
        public async Task<IActionResult> Index()
        {
              return _context.Types != null ? 
                          View(await _context.Types.ToListAsync()) :
                          Problem("Entity set 'DbtranslationAgencyContext.Types'  is null.");
        }

        // GET: Types/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Types == null)
            {
                return NotFound();
            }
            var type = await _context.Types
                .Include(t => t.Orders)
                    .ThenInclude(o => o.Topic)
                .Include(t => t.Orders)
                    .ThenInclude(o => o.OriginalLanguage)
                .Include(t => t.Orders)
                    .ThenInclude(o => o.TranslationLanguage)
                .FirstOrDefaultAsync(m => m.TypeId == id);

            if (type == null)
            {
                return NotFound();
            }

            return View(type);
        }

        // GET: Types/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Types/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TypeId,TypeName")] TranslationsWebApplication.Models.Type type)
        {
            if (ModelState.IsValid)
            {
                var typeExists = await _context.Types.AnyAsync(t => t.TypeName == type.TypeName);
                if (typeExists)
                {
                    ModelState.AddModelError("TypeName", "A type with that name already exists.");
                    return View(type);
                }

                _context.Add(type);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(type);
        }

        // GET: Types/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Types == null)
            {
                return NotFound();
            }

            var @type = await _context.Types.FindAsync(id);
            if (@type == null)
            {
                return NotFound();
            }
            return View(@type);
        }

        // POST: Types/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TypeId,TypeName")] TranslationsWebApplication.Models.Type type)
        {
            if (id != type.TypeId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var typeExists = await _context.Types
                                                 .AnyAsync(t => t.TypeName == type.TypeName && t.TypeId != type.TypeId);
                if (typeExists)
                {
                    ModelState.AddModelError("TypeName", "A type with that name already exists.");
                    return View(type);
                }

                try
                {
                    _context.Update(type);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TypeExists(type.TypeId))
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
            return View(type);
        }

        // GET: Types/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Types == null)
            {
                return NotFound();
            }

            var type = await _context.Types
                .FirstOrDefaultAsync(m => m.TypeId == id);
            if (type == null)
            {
                return NotFound();
            }

            return View(type);
        }

        // POST: Types/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Types == null)
            {
                return Problem("Entity set 'DbtranslationAgencyContext.Types'  is null.");
            }
            var type = await _context.Types.FindAsync(id);
            if (type != null)
            {
                _context.Types.Remove(type);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TypeExists(int id)
        {
          return (_context.Types?.Any(e => e.TypeId == id)).GetValueOrDefault();
        }
    }
}
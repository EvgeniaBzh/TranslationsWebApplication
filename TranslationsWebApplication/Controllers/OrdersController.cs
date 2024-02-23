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
    public class OrdersController : Controller
    {
        private readonly DbtranslationAgencyContext _context;

        public OrdersController(DbtranslationAgencyContext context)
        {
            _context = context;
        }

        // GET: Orders
        public async Task<IActionResult> Index()
        {
            var topics = await _context.Topics.Select(t => new SelectListItem
            {
                Value = t.TopicId.ToString(),
                Text = t.TopicName
            }).ToListAsync();

            ViewData["TopicId"] = topics;

            var types = await _context.Types.Select(t => new SelectListItem
            {
                Value = t.TypeId.ToString(),
                Text = t.TypeName
            }).ToListAsync();

            ViewData["TypeId"] = types;

            var originalLangauges = await _context.Languages.Select(t => new SelectListItem
            {
                Value = t.LanguageId.ToString(),
                Text = t.LanguageName
            }).ToListAsync();

            ViewData["OriginalLanguageId"] = originalLangauges;

            var translationLangauges = await _context.Languages.Select(t => new SelectListItem
            {
                Value = t.LanguageId.ToString(),
                Text = t.LanguageName
            }).ToListAsync();

            ViewData["TranslationLangauge"] = translationLangauges;

            return _context.Orders != null ?
                View(await _context.Orders
                    .Include(o => o.Topic)
                    .Include(o => o.Type)
                    .Include(o => o.OriginalLanguage)
                    .Include(o => o.TranslationLanguage)
                    .ToListAsync()) :
                Problem("Entity set 'DbtranslationAgencyContext.Orders'  is null.");
        }

        // GET: Orders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Orders == null)
            {
                return NotFound();
            }

            var order = await _context.Orders
                .FirstOrDefaultAsync(m => m.OrderId == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // GET: Orders/Create
        public IActionResult Create()
        {
            ViewData["TypeId"] = new SelectList(_context.Types, "TypeId", "TypeName");
            ViewData["TopicId"] = new SelectList(_context.Topics, "TopicId", "TopicName");
            ViewData["OriginalLanguageId"] = new SelectList(_context.Languages, "LanguageId", "LanguageName");
            ViewData["TranslationLanguageId"] = new SelectList(_context.Languages, "LanguageId", "LanguageName");

            return View();
        }

        // POST: Orders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("OrderId,OrderName, OriginalLanguageId, TranslationLanguageId, TypeId, TopicId, OrderScope,OrderPrice,OrderSubmissionDate,OrderStatus")] Order order)
        {
            if (ModelState.IsValid)
            {
                _context.Add(order);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(order);
        }

        // GET: Orders/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Orders == null)
            {
                return NotFound();
            }

            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }
            ViewData["TypeId"] = new SelectList(_context.Types, "TypeId", "TypeName", order.TypeId);
            ViewData["TopicId"] = new SelectList(_context.Topics, "TopicId", "TopicName", order.TopicId);
            ViewData["OriginalLanguageId"] = new SelectList(_context.Languages, "LanguageId", "LanguageName", order.OriginalLanguageId);
            ViewData["TranslationLanguageId"] = new SelectList(_context.Languages, "LanguageId", "LanguageName", order.TranslationLanguageId);

            return View(order);
        }

        // POST: Orders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("OrderId,OrderName, OriginalLanguageId, TranslationLanguageId, TypeId, TopicId, OrderScope,OrderPrice,OrderSubmissionDate,OrderStatus")] Order order)
        {
            if (id != order.OrderId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(order);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderExists(order.OrderId))
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
            return View(order);
        }

        // GET: Orders/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Orders == null)
            {
                return NotFound();
            }

            var order = await _context.Orders
                .FirstOrDefaultAsync(m => m.OrderId == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Orders == null)
            {
                return Problem("Entity set 'DbtranslationAgencyContext.Orders'  is null.");
            }
            var order = await _context.Orders.FindAsync(id);
            if (order != null)
            {
                _context.Orders.Remove(order);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OrderExists(int id)
        {
            return (_context.Orders?.Any(e => e.OrderId == id)).GetValueOrDefault();
        }
    }
}

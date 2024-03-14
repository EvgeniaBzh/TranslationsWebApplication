using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TranslationsWebApplication.Infrastructure.Services;
using TranslationsWebApplication.Models;
using TranslationsWebApplication.Infrastructure.Services;

namespace TranslationsWebApplication.Controllers
{
    public class TopicsController : Controller
    {
        private readonly DbtranslationAgencyContext _context;

        public TopicsController(DbtranslationAgencyContext context)
        {
            _context = context;
        }

        // GET: Topics
        public async Task<IActionResult> Index()
        {
            return _context.Topics != null ?
                        View(await _context.Topics.ToListAsync()) :
                        Problem("Entity set 'DbtranslationAgencyContext.Topics'  is null.");
        }

        // GET: Topics/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Topics == null)
            {
                return NotFound();
            }

            var topic = await _context.Topics
                 .Include(t => t.Orders)
                     .ThenInclude(o => o.Type)
                .Include(t => t.Orders)
                    .ThenInclude(o => o.OriginalLanguage)
                .Include(t => t.Orders)
                    .ThenInclude(o => o.TranslationLanguage)
         .FirstOrDefaultAsync(m => m.TopicId == id);


            if (topic == null)
            {
                return NotFound();
            }

            return View(topic);
        }

        // GET: Topics/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Topics/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TopicId,TopicName")] Topic topic)
        {
            if (ModelState.IsValid)
            {
                var topicExists = await _context.Topics.AnyAsync(t => t.TopicName == topic.TopicName);
                if (topicExists)
                {
                    ModelState.AddModelError("TopicName", "A topic with that name already exists.");
                    return View(topic);
                }

                _context.Add(topic);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(topic);
        }

        // GET: Topics/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Topics == null)
            {
                return NotFound();
            }

            var topic = await _context.Topics.FindAsync(id);
            if (topic == null)
            {
                return NotFound();
            }
            return View(topic);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TopicId,TopicName")] Topic topic)
        {
            if (id != topic.TopicId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var topicExists = await _context.Topics
                                                 .AnyAsync(t => t.TopicName == topic.TopicName && t.TopicId != topic.TopicId);
                if (topicExists)
                {
                    ModelState.AddModelError("TopicName", "A topic with that name already exists.");
                    return View(topic);
                }

                try
                {
                    _context.Update(topic);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TopicExists(topic.TopicId))
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
            return View(topic);
        }
        // GET: Topics/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Topics == null)
            {
                return NotFound();
            }

            var topic = await _context.Topics
                .FirstOrDefaultAsync(m => m.TopicId == id);
            if (topic == null)
            {
                return NotFound();
            }

            return View(topic);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Topics == null)
            {
                return Problem("Entity set 'DbtranslationAgencyContext.Topics' is null.");
            }

            var topic = await _context.Topics
                                       .Include(t => t.Orders)
                                       .FirstOrDefaultAsync(m => m.TopicId == id);
            if (topic != null)
            {
                if (topic.Orders != null)
                {
                    _context.Orders.RemoveRange(topic.Orders);
                }

                _context.Topics.Remove(topic);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }


        private bool TopicExists(int id)
        {
            return (_context.Topics?.Any(e => e.TopicId == id)).GetValueOrDefault();
        }

        [HttpGet]
        public IActionResult Import()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Import(IFormFile topicsFile, CancellationToken cancellationToken)
        {
            if (topicsFile == null || topicsFile.Length == 0)
            {
                return View();
            }

            var topicDataPortServiceFactory = new TopicDataPortServiceFactory(_context);
            var importService = topicDataPortServiceFactory.GetImportService(topicsFile.ContentType);

            using var stream = topicsFile.OpenReadStream();
            await importService.ImportFromStreamAsync(stream, cancellationToken);

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Export([FromQuery] string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", CancellationToken cancellationToken = default)
        {
            var topicDataPortServiceFactory = new TopicDataPortServiceFactory(_context);
            var exportService = topicDataPortServiceFactory.GetExportService(contentType);

            var memoryStream = new MemoryStream();
            await exportService.WriteToAsync(memoryStream, cancellationToken);
            await memoryStream.FlushAsync(cancellationToken);
            memoryStream.Position = 0;

            return new FileStreamResult(memoryStream, contentType)
            {
                FileDownloadName = $"topics_{DateTime.UtcNow.ToString("yyyy-MM-dd")}.xlsx"
            };
        }


    }
}

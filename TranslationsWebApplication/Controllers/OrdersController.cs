using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TranslationsWebApplication.Models;
using DocumentFormat.OpenXml.Packaging;

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
                .Include(o => o.OriginalLanguage)
                .Include(o => o.TranslationLanguage)
                .Include(o => o.Type)
                .Include(o => o.Topic)
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("OrderId,OrderName,OriginalLanguageId,TranslationLanguageId,TypeId,TopicId,OrderScope,OrderPrice,FileName,OrderSubmissionDate")] Order order, IFormFile fileUpload)
        {
            if (order.OrderPrice < 1)
            {
                ModelState.AddModelError("OrderPrice", "The order price must be at least one hryvnia.");
            }

            if (order.OrderSubmissionDate < DateTime.Now)
            {
                ModelState.AddModelError("OrderSubmissionDate", "Submission date cannot be in the past.");
            }

            var allowedFileTypes = new List<string> { "text/plain", "application/pdf", "application/msword", "application/vnd.openxmlformats-officedocument.wordprocessingml.document", "application/vnd.ms-excel", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" };
            if (fileUpload != null && !allowedFileTypes.Contains(fileUpload.ContentType))
            {
                ModelState.AddModelError("fileUpload", "Unsupported file type. Please upload a text document.");
            }

            if (ModelState.IsValid)
            {
                if (fileUpload != null && fileUpload.Length > 0)
                {
                    string content;

                    using (var memoryStream = new MemoryStream())
                    {
                        await fileUpload.CopyToAsync(memoryStream);

                        if (fileUpload.ContentType == "application/vnd.openxmlformats-officedocument.wordprocessingml.document")
                        {
                            content = ReadDocxFile(memoryStream);
                        }
                        else
                        {
                            memoryStream.Position = 0;
                            using (var reader = new StreamReader(memoryStream))
                            {
                                content = await reader.ReadToEndAsync();
                            }
                        }

                        var onlySymbolsContent = Regex.Replace(content, "[^a-zA-Z0-9а-яА-Я]", "");

                        var characterCount = onlySymbolsContent.Length;

                        order.OrderScope = characterCount;
                    }
                }

                order.OrderStatus = OrderStatus.Offer;

                _context.Add(order);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["TypeId"] = new SelectList(_context.Types, "TypeId", "TypeName", order.TypeId);
            ViewData["TopicId"] = new SelectList(_context.Topics, "TopicId", "TopicName", order.TopicId);
            ViewData["OriginalLanguageId"] = new SelectList(_context.Languages, "LanguageId", "LanguageName", order.OriginalLanguageId);
            ViewData["TranslationLanguageId"] = new SelectList(_context.Languages, "LanguageId", "LanguageName", order.TranslationLanguageId);

            return View(order);
        }

        private string ReadDocxFile(Stream fileStream)
        {
            StringBuilder sb = new StringBuilder();
            using (WordprocessingDocument doc = WordprocessingDocument.Open(fileStream, false))
            {
                foreach (var text in doc.MainDocumentPart.Document.Descendants<DocumentFormat.OpenXml.Wordprocessing.Text>())
                {
                    sb.Append(text.Text);
                }
            }
            return sb.ToString();
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }

            if (order.OrderStatus == OrderStatus.InProgress || order.OrderStatus == OrderStatus.Done)
            {
                TempData["EditMessage"] = "It is not possible to edit an order due to its status.";
                return RedirectToAction(nameof(Index));
            }

            ViewData["TypeId"] = new SelectList(_context.Types, "TypeId", "TypeName", order.TypeId);
            ViewData["TopicId"] = new SelectList(_context.Topics, "TopicId", "TopicName", order.TopicId);
            ViewData["OriginalLanguageId"] = new SelectList(_context.Languages, "LanguageId", "LanguageName", order.OriginalLanguageId);
            ViewData["TranslationLanguageId"] = new SelectList(_context.Languages, "LanguageId", "LanguageName", order.TranslationLanguageId);

            return View(order);
        }


        // POST: Orders/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("OrderId,OrderName,OriginalLanguageId,TranslationLanguageId,TypeId,TopicId,OrderScope,OrderPrice,OrderSubmissionDate")] Order orderInput)
        {
            if (id != orderInput.OrderId)
            {
                return NotFound();
            }

            var existingOrder = await _context.Orders.FirstOrDefaultAsync(o => o.OrderId == id);
            if (existingOrder == null)
            {
                return NotFound();
            }

            if (existingOrder.OrderStatus == OrderStatus.InProgress || existingOrder.OrderStatus == OrderStatus.Done)
            {
                return RedirectToAction(nameof(Details), new { id = id });
            }

            if (ModelState.IsValid)
            {
                try
                {
                    existingOrder.OrderName = orderInput.OrderName;
                    existingOrder.OriginalLanguageId = orderInput.OriginalLanguageId;
                    existingOrder.TranslationLanguageId = orderInput.TranslationLanguageId;
                    existingOrder.TypeId = orderInput.TypeId;
                    existingOrder.TopicId = orderInput.TopicId;
                    existingOrder.OrderScope = orderInput.OrderScope;
                    existingOrder.OrderPrice = orderInput.OrderPrice;
                    existingOrder.OrderSubmissionDate = orderInput.OrderSubmissionDate;

                    _context.Update(existingOrder);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderExists(orderInput.OrderId))
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

            return View(orderInput);
        }


        // GET: Orders/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Orders == null)
            {
                return NotFound();
            }

            var order = await _context.Orders
               .Include(o => o.OriginalLanguage)
               .Include(o => o.TranslationLanguage)
               .Include(o => o.Type)
               .Include(o => o.Topic)
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

        public async Task<IActionResult> DownloadFile(int? id)
        {
            if (id == null)
            {
                return Content("id not provided");
            }

            var order = await _context.Orders.FirstOrDefaultAsync(m => m.OrderId == id);

            if (order == null || order.FileData == null || order.FileName == null)
            {
                return Content("file not found");
            }

            return File(order.FileData, "application/octet-stream", order.FileName);
        }

        // GET: Orders/Respond/5
        public async Task<IActionResult> Respond(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }

            order.OrderStatus = OrderStatus.InProgress;
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Details), new { id = id });
        }

        // GET: Orders/Cancel/5
        public async Task<IActionResult> Cancel(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }

            // Перевірте, чи статус замовлення дозволяє його скасування
            if (order.OrderStatus == OrderStatus.InProgress)
            {
                order.OrderStatus = OrderStatus.Offer;
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Details), new { id = id });
            }
            else
            {
                // Можете додати обробку для випадку, якщо скасування не можливе
                return RedirectToAction(nameof(Details), new { id = id });
            }
        }

        // GET: Orders/Submit/5
        public async Task<IActionResult> Submit(int? id)
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

            return View(order);
        }

        // POST: Orders/Submit/5
        // POST: Orders/Submit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Submit(int id, IFormFile submittedFile)
        {
            var order = await _context.Orders.FirstOrDefaultAsync(o => o.OrderId == id);
            if (order == null)
            {
                return NotFound();
            }

            if (submittedFile == null || submittedFile.Length == 0)
            {
                ModelState.AddModelError("submittedFile", "Submitting a file is required.");
                return View(order);
            }

            using (var memoryStream = new MemoryStream())
            {
                await submittedFile.CopyToAsync(memoryStream);
                order.SubmittedFileData = memoryStream.ToArray();
                order.SubmittedFileName = submittedFile.FileName;
            }

            order.OrderStatus = OrderStatus.Done;

            _context.Update(order);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Details), new { id = id });
        }

    }
}

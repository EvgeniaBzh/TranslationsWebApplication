using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using TranslationsWebApplication.Models;

namespace TranslationsWebApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Diagram1Controller : ControllerBase
    {
        private readonly DbtranslationAgencyContext _context;

        public Diagram1Controller(DbtranslationAgencyContext context)
        {
            _context = context;
        }

        public record LanguageTranslationStats(string Language, int TranslationsTo, int TranslationsFrom);

        [HttpGet("languageTranslationStats")]
        public async Task<ActionResult<IEnumerable<LanguageTranslationStats>>> GetLanguageTranslationStats()
        {
            var translationsTo = await _context.Orders
                .GroupBy(o => o.TranslationLanguageId)
                .Select(g => new { LanguageId = g.Key, Count = g.Count() })
                .ToListAsync();

            var translationsFrom = await _context.Orders
                .GroupBy(o => o.OriginalLanguageId)
                .Select(g => new { LanguageId = g.Key, Count = g.Count() })
                .ToListAsync();

            var languages = await _context.Languages.ToListAsync();

            var result = languages
                .Select(l => new LanguageTranslationStats(
                    l.LanguageName,
                    translationsTo.FirstOrDefault(t => t.LanguageId == l.LanguageId)?.Count ?? 0,
                    translationsFrom.FirstOrDefault(t => t.LanguageId == l.LanguageId)?.Count ?? 0
                )).ToList();

            return Ok(result);
        }
    }
}

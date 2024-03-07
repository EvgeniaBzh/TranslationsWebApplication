using Microsoft.AspNetCore.Mvc;
using System.Linq;
using TranslationsWebApplication.Models;
// Припустимо, у вас є DbContext, який називається dbContext
// Додайте відповідний using, якщо потрібно

namespace TranslationsWebApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Diagram2 : ControllerBase
    {
        private readonly DbtranslationAgencyContext dbContext;

        public Diagram2(DbtranslationAgencyContext context)
        {
            dbContext = context;
        }

        [HttpGet("orderTypeDistribution")]
        public IActionResult GetOrderTypeDistribution()
        {
            var result = dbContext.Orders
                .GroupBy(o => o.Type.TypeName)
                .Select(group => new
                {
                    TypeName = group.Key,
                    Count = group.Count()
                })
                .ToList();

            return Ok(result);
        }
    }
}

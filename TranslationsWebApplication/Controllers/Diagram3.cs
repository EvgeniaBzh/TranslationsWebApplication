﻿using Microsoft.AspNetCore.Mvc;
using System.Linq;
using TranslationsWebApplication.Models;

namespace TranslationsWebApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Diagram3 : ControllerBase
    {
        private readonly DbtranslationAgencyContext dbContext;

        public Diagram3(DbtranslationAgencyContext context)
        {
            dbContext = context;
        }

        [HttpGet("topicDistribution")]
        public IActionResult GetTopicDistribution()
        {
            var result = dbContext.Orders
                .GroupBy(o => o.Topic.TopicName)
                .Select(group => new
                {
                    TopicName = group.Key,
                    Count = group.Count()
                })
                .ToList();

            return Ok(result);
        }
    }
}

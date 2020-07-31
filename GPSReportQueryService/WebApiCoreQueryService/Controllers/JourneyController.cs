using Microsoft.AspNetCore.Mvc;
using QueryServiceEngine;
using QueryServiceEngine.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiCoreQueryService
{
    [Route("api/[controller]")]
    [ApiController]
    public class JourneysController : ControllerBase
    {
        private readonly IJourneyService _journeyService;

        public JourneysController(IJourneyService journeyService)
        {
            _journeyService = journeyService;
        }

        [HttpGet]
        public async Task<ActionResult<string>> Get()
        {
            return "Set VehycleId and time from and to, e.g.  api/journeys/v1/t1/t5";
        }

        [HttpGet("{id}/{from}/{to}", Name = "Get")]
        public async Task<ActionResult<List<Journey>>> Get(string id, string from, string to)
        {
            return await _journeyService.GetAsync(id, from, to);
        }
        
    }
}

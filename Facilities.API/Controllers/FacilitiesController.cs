using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Facilities.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FacilitiesController : ControllerBase
    {
        private readonly ILogger<FacilitiesController> _logger;

        public FacilitiesController(ILogger<FacilitiesController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        public IActionResult Get()
        {
            return Ok();
        }

        public record FacilityInfo(long Patient_Id, string Name, DateTimeOffset Date_Created, bool Submissions);
    }
}

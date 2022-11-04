using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Monitor.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Facilities.API.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class FacilitiesController : ControllerBase
    {
        private readonly ILogger<FacilitiesController> _logger;

        public FacilitiesController(ILogger<FacilitiesController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Endpoint that gets the facilities data and sends to another server <br/>
        /// The endpoint will be like <code>https://your-domain-dot-com/api/Facilities/UpdateFacilities</code>
        /// </summary>
        /// <param name="facilityInfo"></param>
        /// <returns><see cref="StatusCodeResult"/> depending on the success or failure of this action method </returns>
        [HttpPost("UpdateFacilities")]
        public IActionResult UpdateFacilities(IEnumerable<FacilityInfo> facilityInfo)
        {
            //get the list of facility data
            //update each facility data in the list with id**
            //push to the server. Throw an error if something bad happens
            return Ok(facilityInfo);
        }

    }
}

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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

        /// <summary>
        /// Models the facilities data
        /// </summary>
        /// <param name="Patient_Id"></param>
        /// <param name="Name"></param>
        /// <param name="Date_Created"></param>
        /// <param name="Submissions"></param>
        public record FacilityInfo(long Patient_Id, string Name, DateTimeOffset Date_Created, bool Submissions);
    }
}

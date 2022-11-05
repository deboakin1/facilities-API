using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Monitor.Model;
using System.Collections.Generic;
using System.Linq;

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
        public IActionResult UpdateFacilities(FacilityInfo[] facilityInfo)
        {
            if(!facilityInfo.Any())
                return StatusCode(StatusCodes.Status204NoContent);

            var updatedFacilities = UpdateFacilitiesId(facilityInfo);
            //update each facility data in the list with id**
            //push to the server. Throw an error if something bad happens
            return Ok(updatedFacilities);
        }

        private IEnumerable<UpdatedFacilty> UpdateFacilitiesId(FacilityInfo[] facilityInfo)
        {
            var totalFacilities = facilityInfo.Length;

            for (int index = 0; index < totalFacilities; index++)
            {
                var facility = facilityInfo[index];

                yield return new UpdatedFacilty
                {
                    Id = index,
                    Name = facility.Name,
                    PatientId = facility.PatientId,
                    Submissions = facility.Submissions,
                    DateCreated = facility.DateCreated,
                };
            }
        }
    }
}

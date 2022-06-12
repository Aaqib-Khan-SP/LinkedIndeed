using LinkedIndeed.API.Filters;
using LinkedIndeed.BLL.Models;
using LinkedIndeed.BLL.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace LinkedIndeed.API.Controllers
{
    [Route("api/[controller]")]
    [TokenAuthenticationFilter]
    [ApiController]
    public class LocationsController : ControllerBase
    {
        public readonly LocationsService _locationsService;
        public LocationsController(LocationsService locationsService)
        {
            _locationsService = locationsService;
        }

        [HttpGet("{id:int}")]
        public ActionResult<LocationDetails> GetLocation(int id)
        {
            try
            {
                LocationDetails location = _locationsService.GetLocationByID(id);
                if (location != null)
                {
                    return location;
                }
                else
                {
                    return StatusCode((int)HttpStatusCode.OK, "No Location Found with ID : " + id);
                }
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, "Error getting Location Data");
            }

        }

        [HttpGet("list")]
        public ActionResult<List<LocationDetails>> GetLocations()
        {
            return _locationsService.GetLocations();
        }

        [HttpPost]
        public object AddLocation([FromBody] LocationDetails location)
        {
            try
            {
                int locationId = _locationsService.AddLocation(location);
                object data = HttpContext.Request.GetEncodedUrl() + locationId;
                return StatusCode(201, data);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, "Unable to Add Location");
            }

        }

        [HttpPut("{id:int}")]
        public object UpdateLocation(int id, [FromBody] LocationDetails location)
        {
            try
            {
                bool result = _locationsService.UpdateLocation(location);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, "Unable to Update Location");
            }

        }
    }
}

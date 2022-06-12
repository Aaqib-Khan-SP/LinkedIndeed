using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LinkedIndeed.BLL.Services;
using LinkedIndeed.BLL.Models;
using Microsoft.AspNetCore.Http.Extensions;
using System.Net;
using LinkedIndeed.API.Filters;
using Microsoft.AspNetCore.Authorization;

namespace LinkedIndeed.API.Controllers
{
    [Route("api/[controller]")]
    [TokenAuthenticationFilter]
    [ApiController]
    public class JobsController : ControllerBase
    {
        public readonly JobsService _jobsService;
        public JobsController(JobsService jobsService)
        {
            _jobsService = jobsService;
        }

        [HttpGet("{id:int}")]
        public ActionResult<JobDetails> GetJob(int id)
        {
            try
            {
                JobDetails job = _jobsService.GetJobByID(id);
                if (job != null)
                {
                    return job;
                }
                else
                {
                    return StatusCode((int)HttpStatusCode.OK, "No Job Found with ID : "+id);
                }
            }
            catch(Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, "Error getting Job Data");
            }
        }

        [HttpGet("list")]
        public ActionResult<ListJobs> GetJobs([FromBody] PagingParameters parameters)
        {
            return _jobsService.GetJobs(parameters); ;
        }

        [HttpPost]
        public object AddJob([FromBody] Job job)
        {
            try
            {
                int jobId = _jobsService.AddJob(job);
                object data = HttpContext.Request.GetEncodedUrl() + jobId;
                return StatusCode(201, data);
            }
            catch(Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, "Unable to Add Job");
            }
            
        }

        [HttpPut("{id:int}")]
        public object UpdateJob( int id, [FromBody]  Job job)
        {
            try
            {
                bool result = _jobsService.UpdateJob(job);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, "Unable to Update Job");
            }

        }
    }
}

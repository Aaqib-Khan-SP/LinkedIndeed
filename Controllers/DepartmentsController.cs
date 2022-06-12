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
    public class DepartmentsController : ControllerBase
    {
        public readonly DepartmentsService _departmentsService;
        public DepartmentsController(DepartmentsService departmentsService)
        {
            _departmentsService = departmentsService;
        }

        [HttpGet("{id:int}")]
        public ActionResult<DepartmentDetails> GetDepartment(int id)
        {
            try
            {
                DepartmentDetails department = _departmentsService.GetDepartmentByID(id);
                if (department != null)
                {
                    return department;
                }
                else
                {
                    return StatusCode((int)HttpStatusCode.OK, "No Department Found with ID : " + id);
                }
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, "Error getting Department Data");
            }

        }

        [HttpGet("list")]
        public ActionResult<List<DepartmentDetails>> GetDepartments()
        {
            return _departmentsService.GetDepartments(); ;
        }

        [HttpPost]
        public object AddDepartment([FromBody] DepartmentDetails department)
        {
            try
            {
                int departmentID = _departmentsService.AddDepartment(department);
                object data = HttpContext.Request.GetEncodedUrl() + departmentID;
                return StatusCode(201, data);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, "Unable to Add Department");
            }

        }

        [HttpPut("{id:int}")]
        public object UpdateDepartment(int id, [FromBody] DepartmentDetails department)
        {
            try
            {
                bool result = _departmentsService.UpdateDepartment(department);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, "Unable to Update Department");
            }

        }
    }
}

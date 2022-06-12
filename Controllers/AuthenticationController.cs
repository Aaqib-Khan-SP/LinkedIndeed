using LinkedIndeed.BLL.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LinkedIndeed.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        public readonly IAuth _auth;
        public AuthenticationController(IAuth auth)
        {
            _auth = auth;
        }

        [HttpGet("getToken/{userId:int}")]
        public string GetToken(string userId)
        {
            var token = _auth.NewToken(userId);
            return token;
        }
    }
}

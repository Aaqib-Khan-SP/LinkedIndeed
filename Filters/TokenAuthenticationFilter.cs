using LinkedIndeed.BLL.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LinkedIndeed.API.Filters
{
    public class TokenAuthenticationFilter : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var Auth = (IAuth)context.HttpContext.RequestServices.GetService(typeof(IAuth));
            string token = String.Empty;
            if (context.HttpContext.Request.Headers.ContainsKey("Authorization"))
            {
                token = context.HttpContext.Request.Headers.First(x => x.Key == "Authorization").Value.ToString().Replace("Bearer ", string.Empty);
                try
                {
                    var claimPrinciple = Auth.VerifyToken(token);
                }
                catch (Exception ex)
                {
                    context.ModelState.AddModelError("Unauthorized", ex.ToString());
                    context.Result = new UnauthorizedObjectResult(context.ModelState);
                }
            }
            else
            {
                context.ModelState.AddModelError("Unauthorized","Bearer Token Missing");
                context.Result = new UnauthorizedObjectResult(context.ModelState);
            }

        }
    }
}

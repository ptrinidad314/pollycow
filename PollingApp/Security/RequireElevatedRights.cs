using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
//using Microsoft.Extensions.Configuration;
//using Microsoft.Extensions.DependencyInjection;

namespace PollingApp.Security
{
    public class RequireElevatedRights : AuthorizationHandler<RequireElevatedRights>, IAuthorizationRequirement
    {
        //private IConfiguration _configuration;

        private string _adminObjId;
        //private string _currentObjId;

        private IHttpContextAccessor _httpContextAccessor;

        public RequireElevatedRights(string adminObjId, IHttpContextAccessor httpContextAccessor)
        {
            _adminObjId = adminObjId;
            //_currentObjId = currentObjId;
            //_configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, RequireElevatedRights requirement)
        {
            //var _httpContextAccessor = new HttpContextAccessor();

            var currentAzureADObjId = _httpContextAccessor.HttpContext.User.Claims.Where(p => p.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").FirstOrDefault().Value;

            //var currentAzureADObjId = "543707fc-a8a7-49fc-a92c-1f9f64788b96";
            //_adminObjId = "543707fc-a8a7-49fc-a92c-1f9f64788b96";

            if (_adminObjId == currentAzureADObjId) //azureADObjId == _configuration["AzureAD:AdminObjectId"])//_adminObjId) 
            {
                context.Succeed(requirement);
            }
            
            return Task.CompletedTask;
        }
    }
}

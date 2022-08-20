using IntelliCRMAPIService.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace IntelliCRMAPIService.Attribute
{

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class AuthorizeAttribute : System.Attribute, IAuthorizationFilter
    {
        private readonly IList<Role> _roles;

        public AuthorizeAttribute(params Role[] roles)
        {
            _roles = roles ?? new Role[] { };
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            // skip authorization if action is decorated with [AllowAnonymous] attribute
            var allowAnonymous = context.ActionDescriptor.EndpointMetadata.OfType<CustomAllowAnonymousAttribute>().Any();
            if (allowAnonymous)
                return;

            // authorization
            var user = (AuthResponse)context.HttpContext.Items["User"];
            if (user == null || (_roles.Any() && !_roles.Contains(user.Role)))
            {
                // not logged in or role not authorized
                context.Result = new JsonResult(new { message = "Unauthorized" }) { StatusCode = StatusCodes.Status401Unauthorized };
            }
        }
    }
    public enum Role
    {
        admin,
        customer,
        subadmin
    }
}

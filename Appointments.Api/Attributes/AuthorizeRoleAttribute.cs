using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Appointments.Api.Attributes
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = true)]
    public class AuthorizeRoleAttribute : Attribute, IAuthorizationFilter
    {
        private readonly int _role; // int: 0 = User, 1 = Manager

        public AuthorizeRoleAttribute(int role)
        {
            _role = role;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (!context.HttpContext.Items.TryGetValue("UserRole", out var objRole))
            {
                context.Result = new ForbidResult();
                return;
            }

            int userRole = (int)objRole;
            if (userRole != _role)
            {
                context.Result = new ForbidResult();
            }
        }
    }
}

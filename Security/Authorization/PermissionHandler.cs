
using bca.api.Services;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace bca.api.Security.Authorization
{
    public class PermissionHandler : AuthorizationHandler<PermissionRequirement>
    {
        //private readonly IPermissionService _permissionService;

        //public PermissionHandler(IPermissionService permissionService)
        //{
        //    _permissionService = permissionService;
        //}

        //protected override async Task HandleRequirementAsync(
        //    AuthorizationHandlerContext context,
        //    PermissionRequirement requirement)
        //{
        //    var userName = context.User.FindFirstValue(ClaimTypes.NameIdentifier);
        //    if (userName == null) return;

        //    var hasPermission = await _permissionService.HasPermissionAsync(userName, requirement.Permission);

        //    if (hasPermission)
        //        context.Succeed(requirement);
        //}

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
        {
            if (context.User.HasClaim(c => c.Type == "permission" && c.Value == requirement.Permission))
            {
                context.Succeed(requirement);
            }
            return Task.CompletedTask;
        }
    }

}

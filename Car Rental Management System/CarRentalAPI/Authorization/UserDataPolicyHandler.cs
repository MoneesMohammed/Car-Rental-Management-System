using CarRentalAPIBusinessLayer;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace CarRentalAPI.Authorization
{
    public class UserDataPolicyHandler : AuthorizationHandler<UserDataPolicyRequirement, int>
    {
        protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        UserDataPolicyRequirement requirement,
        int UserID)
        {
            if (!context.User.IsInRole("Admin") && !context.User.IsInRole("Rental Agent") && !context.User.IsInRole("Manager"))
                return Task.CompletedTask;

            // Ownership check
            var userID = context.User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (int.TryParse(userID, out int AuthenticatedUserID))
            {
                if (AuthenticatedUserID == UserID)
                    context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}

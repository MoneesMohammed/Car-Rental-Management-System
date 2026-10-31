using CarRentalAPIBusinessLayer;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace CarRentalAPI.Authorization
{
    // This authorization handler enforces the ownership rule for Employee resources.
    // It checks whether the current user is either:
    // - An Admin (full access), OR
    // - The owner of the Employee record being requested
    public class EmployeeDataPolicyHandler : AuthorizationHandler<EmployeeDataPolicyRequirement, int>
    {
        protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        EmployeeDataPolicyRequirement requirement,
        int EmployeeID)
        {
            bool isAdminOnly = requirement.IsAdminOnly;

            // Admin Or Manager override

            if (isAdminOnly && context.User.IsInRole("Admin"))
            {
                context.Succeed(requirement);
                return Task.CompletedTask;
            }

            if (!isAdminOnly && (context.User.IsInRole("Admin") || context.User.IsInRole("Manager")))
            {
                context.Succeed(requirement);
                return Task.CompletedTask;
            }

            //if (!context.User.IsInRole("Rental Agent"))
            //    return Task.CompletedTask;

            // Ownership check
            var userID = context.User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (int.TryParse(userID, out int AuthenticatedUserID))
            {
                var currentUser = clsUser.Find(AuthenticatedUserID);

                if (currentUser == null || !currentUser.IsActive)
                    return Task.CompletedTask;

                if (currentUser.EmployeeID == EmployeeID)
                    context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }

    }
}

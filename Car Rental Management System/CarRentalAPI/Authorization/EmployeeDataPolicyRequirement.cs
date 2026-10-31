using Microsoft.AspNetCore.Authorization;

namespace CarRentalAPI.Authorization
{
    // This class represents the authorization rule itself.
    // It does NOT contain logic.
    // It simply defines the requirement:
    // "Owner OR Admin Or Manager can access the student resource."
    public class EmployeeDataPolicyRequirement : IAuthorizationRequirement
    {
        public bool IsAdminOnly { get; set; }

        public EmployeeDataPolicyRequirement(bool isAdminOnly)
        {
            IsAdminOnly = isAdminOnly;
        }
    }
}

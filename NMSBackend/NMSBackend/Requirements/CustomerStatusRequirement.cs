using Microsoft.AspNetCore.Authorization;

namespace NMSBackend.Requirements
{
    public class CustomerBlockedStatusRequirement : IAuthorizationRequirement
    {
        public bool IsBlocked { get; }
        public CustomerBlockedStatusRequirement(bool isBlocked)
        {
            IsBlocked = isBlocked;
        }

    }
}

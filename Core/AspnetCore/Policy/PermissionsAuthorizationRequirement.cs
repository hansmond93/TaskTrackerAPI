using Microsoft.AspNetCore.Authorization;
using Core.Permissions;
using System.Collections.Generic;

namespace Core.AspNetCore.Policy
{
    public class PermissionsAuthorizationRequirement : IAuthorizationRequirement
    {
        public IEnumerable<Permission> RequiredPermissions { get; }

        public PermissionsAuthorizationRequirement(IEnumerable<Permission> requiredPermissions)
        {
            RequiredPermissions = requiredPermissions;
        }
    }
}
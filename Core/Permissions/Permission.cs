using System.Collections.Generic;

namespace Core.Permissions
{
    public enum Permission
    {
        USER_MENU = 101,
        USER_LIST = 102,
        USER_ADD = 103,
        USER_DELETE = 104,
        USER_UPDATE = 105,
    }

    public static class PermisionProvider
    {
        public static Dictionary<string, IEnumerable<Permission>> GetSystemDefaultRoles()
        {

            var defaultRoles = new Dictionary<string, IEnumerable<Permission>>();


            return defaultRoles;
        }
    }
}
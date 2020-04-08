using Microsoft.AspNetCore.Identity;
using Entities.Auditing;
using Entities.Common;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace Entities
{
    [Table(nameof(User))]
    public class User : IdentityUser<int>, IHasCreationTime, IHasDeletionTime, ISoftDelete, IHasModificationTime
    {
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public bool IsFirstTimeLogin { get; set; }
        public DateTime CreationTime { get; set; }
        public DateTime? DeletionTime { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsLocked { get; set; }
        public DateTime? LastModificationTime { get; set; }
        public DateTime? LastLoginDate { get; set; }
        #nullable enable
        public string? Designation { get; set; }
        public string? EmployeeID { get; set; }
        #nullable restore
        public DateTime? Birthday { get; set; }

        public ICollection<ProjectUser> ProjectUsers { get; set; }
        public ICollection<Task> Tasks { get; set; }

        [NotMapped]
        public string FullName
        {
            get
            {
                return $"{LastName} {FirstName}";
            }
        }
    }

    public class UserClaim : IdentityUserClaim<int> { }

    public class UserRole : IdentityUserRole<int> { }

    public class UserLogin : IdentityUserLogin<int>
    {
        public int Id { get; set; }
    }

    public class RoleClaim : IdentityRoleClaim<int> { }

    public class UserToken : IdentityUserToken<int> { }

    public static class UserExtensions
    {
        //public static bool IsDefaultAccount(this User user)
        //{
        //    return CoreConstants.DefaultAccount == user.UserName;
        //}

        public static bool IsNull(this User user)
        {
            return user == null;
        }

        //public static bool IsConfirmed(this User user)
        //{
        //    return user.EmailConfirmed || user.PhoneNumberConfirmed;
        //}

        //public static bool AccountLocked(this User user)
        //{
        //    return user.LockoutEnabled == true;
        //}

        //public static bool HasNoPassword(this User user)
        //{
        //    return string.IsNullOrWhiteSpace(user.PasswordHash);
        //}
    }
}
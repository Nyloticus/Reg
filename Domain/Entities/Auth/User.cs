using Common;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace Domain.Entities.Auth
{
    public class User : IdentityUser, IBaseEntity
    {
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string FullName { get { return $"{FirstName} {MiddleName} {LastName}"; } }
        public bool Active { get; set; } = true;
        public DateTime? BirthDate { get; set; }
        public string MobileNumber { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public string DeletedBy { get; set; }
        public DateTime? DeletedDate { get; set; }
        public bool IsDeleted { get; set; }

        public HashSet<UserRole> UserRoles { get; set; } = new HashSet<UserRole>();
        public HashSet<UserImages> UserImages { get; set; } = new HashSet<UserImages>();
        public HashSet<Address> Addresses { get; set; } = new HashSet<Address>();

    }

}
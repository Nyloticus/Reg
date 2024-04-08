using Common;
using Microsoft.AspNetCore.Identity;

namespace Domain.Entities.Auth
{
    public class UserRole : IdentityUserRole<string>, IBaseEntity
    {
        public virtual User User { get; set; }
        public virtual Role Role { get; set; }
    }

}

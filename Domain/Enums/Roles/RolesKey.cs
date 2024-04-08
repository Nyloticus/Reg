using System.Runtime.Serialization;

namespace Domain.Enums.Roles
{
    public enum RolesKey
    {
        [EnumMember(Value = "SuperAdmin")] SuperAdmin,
        [EnumMember(Value = "Admin")] Admin
    }
}
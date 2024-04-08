using Common;

namespace Domain.Entities.Auth
{
    public class UserCount : BaseEntity<string>
    {
        public int Count { get; set; }
    }
}

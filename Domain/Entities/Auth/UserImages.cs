using Common;

namespace Domain.Entities.Auth
{
    public class UserImages : BaseEntity<string>
    {
        public string Url { get; set; }
        public string Path { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }
    }
}

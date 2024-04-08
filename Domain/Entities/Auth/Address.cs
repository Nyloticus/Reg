using Common;

namespace Domain.Entities.Auth
{
    public class Address : BaseEntity<string>
    {
        public string GovernateId { get; set; }
        public Governate Governate { get; set; }
        public string CityId { get; set; }
        public City City { get; set; }
        public string Street { get; set; }
        public string BuildNo { get; set; }
        public int FlatNo { get; set; }

        //
        public string UserId { get; set; }
        public User User { get; set; }
    }
}

using Common;
using System.Collections.Generic;

namespace Domain.Entities.Auth
{
    public class City : BaseEntity<string>
    {
        public string NameEn { get; set; }
        public string NameAr { get; set; }
        public HashSet<Address> Addresses { get; set; }
    }
}

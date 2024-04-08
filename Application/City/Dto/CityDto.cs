using Mapster;

namespace Application.City.Dto
{
    public class CityDto : IRegister
    {
        public string NameEn { get; set; }
        public string NameAr { get; set; }
        public string Id { get; set; }

        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Domain.Entities.Auth.City, CityDto>();
        }
    }
}

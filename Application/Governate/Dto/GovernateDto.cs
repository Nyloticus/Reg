using Mapster;

namespace Application.Governate.Dto
{
    public class GovernateDto : IRegister
    {
        public string NameEn { get; set; }
        public string NameAr { get; set; }
        public string Id { get; set; }

        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Domain.Entities.Auth.Governate, GovernateDto>();
        }
    }
}

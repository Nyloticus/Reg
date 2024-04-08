using Mapster;

namespace Application.UserImage.Dto
{
    public class UserImageDto : IRegister
    {
        public string Url { get; set; }
        public string Type { get; set; }
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Domain.Entities.Auth.UserImages, UserImageDto>();

        }
    }
}

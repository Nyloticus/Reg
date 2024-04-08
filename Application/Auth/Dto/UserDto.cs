using Mapster;

namespace Application.Auth.Dto
{
    public class UserDto : IRegister
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }        
        public bool Active { get; set; }

        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Domain.Entities.Auth.User, UserDto>();
        }
    }
}

using Bookshop_v5.Models.DTO;

namespace Bookshop_v5.Interfaces
{
    public interface IUserAuthServices
    {
        Task<Status> LoginAsync(LoginModel model);
        Task LogoutAsync();
        Task<Status> RegisterAsync(RegistrationModel model);
    }
}

using CarWash.Core.Dtos;
using CarWash.Entity.Dtos.UserDtos;

namespace CarWash.Service.Services.UserServices
{
    public interface IUserService
    {
        Task<Response<UserInfoDto>> GetUserInfo(int userId);
    }
}

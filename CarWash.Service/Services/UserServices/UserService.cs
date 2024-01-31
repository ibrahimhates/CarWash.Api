using AutoMapper.Internal.Mappers;
using CarWash.Core.Dtos;
using CarWash.Entity.Dtos.Appointment;
using CarWash.Entity.Dtos.UserDtos;
using CarWash.Entity.Entities;
using CarWash.Entity.Enums;
using CarWash.Repository.Repositories.Users;
using CarWash.Repository.UnitOfWork;
using CarWash.Service.Mapping;
using CarWash.Service.ServiceExtensions;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarWash.Service.Services.UserServices
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly ILogger<UserService> _logger;
        private readonly IUnitOfWork _unitOfWork;
        public UserService(IUserRepository userRepository, ILogger<UserService> logger, IUnitOfWork unitOfWork)
        {
            _userRepository = userRepository;
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public async Task<Response<UserInfoDto>> GetUserInfo(int userId)
        {
            _logger.SendInformation(nameof(GetUserInfo), "Started");
            try
            {
                var user = await _userRepository.GetByIdAsync(userId);
                var info = ObjectMapper.Mapper.Map<UserInfoDto>(user);

                _logger.SendInformation(nameof(GetUserInfo), "list successful");
                return Response<UserInfoDto>.Success(info,200);
            }
            catch (Exception ex)
            {
                _logger.SendWarning(nameof(GetUserInfo), ex.Message);
                return Response<UserInfoDto>.Fail("Bilinmedik bir hata oluştu", 500);
            }
        }
    }
}

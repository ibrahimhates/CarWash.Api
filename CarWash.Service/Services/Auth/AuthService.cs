
using Azure.Core;
using CarWash.Core.Dtos;
using CarWash.Entity.Dtos.Auth;
using CarWash.Entity.Dtos.Customer;
using CarWash.Entity.Dtos.Employee;
using CarWash.Entity.Entities;
using CarWash.Entity.Enums;
using CarWash.Repository.Repositories.Customers;
using CarWash.Repository.Repositories.Employees;
using CarWash.Repository.Repositories.Roles;
using CarWash.Repository.Repositories.Token;
using CarWash.Repository.Repositories.Users;
using CarWash.Repository.UnitOfWork;
using CarWash.Service.Mapping;
using CarWash.Service.Providers;
using CarWash.Service.ServiceExtensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CarWash.Service.Services.Auth
{
    public class AuthService : IAuthService
    {
        private readonly JwtGenerator _jwtGenerator;
        private readonly PasswordHasher _passwordHasher;
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly ILogger<AuthService> _logger;
        private readonly ITokenRepository _tokenRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IUnitOfWork _unitOfWork;
        public AuthService(JwtGenerator jwtGenerator, IUserRepository userRepository, PasswordHasher passwordHasher, IRoleRepository roleRepository, ILogger<AuthService> logger, ITokenRepository tokenRepository, ICustomerRepository customerRepository, IEmployeeRepository employeeRepository, IUnitOfWork unitOfWork)
        {
            _jwtGenerator = jwtGenerator;
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _roleRepository = roleRepository;
            _logger = logger;
            _tokenRepository = tokenRepository;
            _customerRepository = customerRepository;
            _employeeRepository = employeeRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<Response<User>> RegisterEmployee(CreateEmployeeDto createEmployeeDto)
        {
            try
            {
                // Kullanıcı adı ve e-posta benzersiz olmalı
                if (await IsUserNameTaken(createEmployeeDto.UserName))
                {
                    throw new InvalidOperationException("This username is already taken.");
                }

                if (await IsEmailTaken(createEmployeeDto.Email))
                {
                    throw new InvalidOperationException("This email is already taken.");
                }

                // Şifre ve şifre onayı eşleşmeli
                if (createEmployeeDto.Password != createEmployeeDto.PasswordConfirm)
                {
                    throw new InvalidOperationException("Password and password confirmation do not match.");
                }

                // Şifreyi şifrele
                string hashedPassword = _passwordHasher.HashPassword(createEmployeeDto.Password);

                // Yeni kullanıcı nesnesi oluştur
                var newUser = ObjectMapper.Mapper.Map<User>(createEmployeeDto);
                newUser.Password = hashedPassword;
                
                var newEmpAttendance = ObjectMapper.Mapper.Map<EmployeeAttendance>(createEmployeeDto.Attandace);

                var newEmployee = new Employee
                {
                    //UserId = newUser.Id,
                    RoleId = createEmployeeDto.RoleId,
                    EmployeeAttendance = newEmpAttendance,
                };

                newUser.Employee = newEmployee;


                // Kullanıcıyı veritabanına ekleyin
                await _userRepository.CreateAsync(newUser);
                //await _employeeRepository.CreateAsync(newEmployee);
                await _unitOfWork.SaveChangesAsync();

                return Response<User>.Success(newUser, 200);
            }
            catch (InvalidOperationException ex)
            {
                _logger.SendError(ex, nameof(RegisterEmployee));
                return Response<User>.Fail(ex.Message, 400);
            }
            catch (Exception ex)
            {
                _logger.SendError(ex, nameof(RegisterEmployee));
                return Response<User>.Fail("Bilinmedik bir hata oluştu.", 500);
            }

        }
        public async Task<Response<User>> RegisterCustomer(CreateCustomerDto createCustomerDto)
        {
            try
            {    // Kullanıcı adı ve e-posta benzersiz olmalı
                if (await IsUserNameTaken(createCustomerDto.UserName))
                {
                    throw new InvalidOperationException("This username is already taken.");
                }

                if (await IsEmailTaken(createCustomerDto.Email))
                {
                    throw new InvalidOperationException("This email is already taken.");
                }

                // Şifre ve şifre onayı eşleşmeli
                if (createCustomerDto.Password != createCustomerDto.PasswordConfirm)
                {
                    throw new InvalidOperationException("Password and password confirmation do not match.");
                }

                // Şifreyi şifrele
                string hashedPassword = _passwordHasher.HashPassword(createCustomerDto.Password);

                // Yeni kullanıcı nesnesi oluştur
                var newUser = ObjectMapper.Mapper.Map<User>(createCustomerDto);
                newUser.Password = hashedPassword;

                var newEmployee = new Customer
                {
                    UserId = newUser.Id,
                    CreatedAt = DateTime.Now,
                };

                newUser.Customer = newEmployee;

                // Kullanıcıyı veritabanına ekleyin
                await _userRepository.CreateAsync(newUser);
                await _unitOfWork.SaveChangesAsync();

                return Response<User>.Success(newUser, 200);
            }
            catch (InvalidOperationException ex)
            {
                _logger.SendError(ex, nameof(RegisterEmployee));
                return Response<User>.Fail(ex.Message, 400);
            }
            catch (Exception ex)
            {
                _logger.SendError(ex, nameof(RegisterEmployee));
                return Response<User>.Fail("Bilinmedik bir hata oluştu.", 500);
            }
        }

        public async Task<Response<LoginResDto>> CustomerLogin(LoginCustomerReqDto request)
        {
            try
            {
                var user = await _userRepository.FindByCondition(u => u.Email == request.Email, false).FirstOrDefaultAsync();
                if (user == null)
                {
                    return Response<LoginResDto>.Fail("Girdiğiniz mail veya şifre hatalı. Lütfen kontrol ediniz", 404);
                }

                // Employee Customer logini ile login olamamasi icin sart kosuldu
                var customer = await _customerRepository
                    .AnyAsync(x => x.UserId == user.Id);

                if (!customer)
                {
                    return Response<LoginResDto>.Fail("Girdiginiz bilgiler ile eslesen musteri hesabi bulunamadi", 404);
                }
                
                var signInResult = _passwordHasher.VerifyPassword(request.Password, user.Password);

                if (signInResult)
                {
                    var token = await _jwtGenerator.GenerateJwt(user, UserTypes.Customer, TokenTypes.AccessToken);
                    var refreshToken = await _jwtGenerator.GenerateRefreshToken(user, UserTypes.Customer);

                    return Response<LoginResDto>.Success(new LoginResDto { Token = token, RefreshToken = refreshToken }, 200);
                }

                return Response<LoginResDto>.Fail("Girilen şifre hatalı. Lütfen şifrenizi kontrol ediniz.", 404);
            }
            catch (Exception ex)
            {
                _logger.SendError(ex, nameof(CustomerLogin));
                return Response<LoginResDto>.Fail("Bilinmedik bir hata oluştu.", 500);
            }
        }
        public async Task<Response<LoginResDto>> EmployeeLogin(LoginEmployeeReqDto request)
        {
            try
            {
                var user = await _userRepository.FindByCondition(u => u.Email == request.Email, false).FirstOrDefaultAsync();
                if (user == null)
                {
                    return Response<LoginResDto>.Fail("Girdiğiniz mail veya şifre hatalı. Lütfen kontrol ediniz", 404);
                }

                var employee = await _employeeRepository
                    .AnyAsync(x => x.UserId == user.Id);
                
                if (!employee)
                {
                    return Response<LoginResDto>.Fail("Girdiginiz bilgiler ile eslesen calisan hesabi bulunamadi", 404);
                }
                
                var signInResult = _passwordHasher.VerifyPassword(request.Password, user.Password);

                if (signInResult)
                {
                    var token = await _jwtGenerator.GenerateJwt(user,UserTypes.Employee, TokenTypes.AccessToken);
                    var refreshToken = await _jwtGenerator.GenerateRefreshToken(user, UserTypes.Employee);

                    
                    
                    return Response<LoginResDto>.Success(new LoginResDto { Token = token, RefreshToken = refreshToken }, 200);
                }

                return Response<LoginResDto>.Fail("Girilen şifre hatalı. Lütfen şifrenizi kontrol ediniz.", 404);
            }
            catch (Exception ex)
            {
                _logger.SendError(ex, nameof(CustomerLogin));
                return Response<LoginResDto>.Fail("Bilinmedik bir hata oluştu.", 500);
            }
        }

        public async Task<Response<NoContent>> Logout(int userId)
        {
            try
            {
                var user = await _userRepository.FindByCondition(u => u.Id == userId, false).FirstOrDefaultAsync();
                await _jwtGenerator.Logout(user);
                return Response<NoContent>.Success(200);
            }
            catch (Exception ex)
            {
                _logger.SendError(ex, nameof(Logout));
                return Response<NoContent>.Fail("Bilinmedik bir hata oluştu.", 500);
            }
        }

        private async Task<bool> IsUserNameTaken(string userName)
        {
            return await _userRepository.AnyAsync(u => u.UserName == userName);
        }

        private async Task<bool> IsEmailTaken(string email)
        {
            return await _userRepository.AnyAsync(u => u.Email == email);
        }

    }
}

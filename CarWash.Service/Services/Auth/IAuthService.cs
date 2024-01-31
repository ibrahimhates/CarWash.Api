using CarWash.Core.Dtos;
using CarWash.Entity.Dtos.Auth;
using CarWash.Entity.Dtos.Customer;
using CarWash.Entity.Dtos.Employee;
using CarWash.Entity.Entities;

namespace CarWash.Service.Services.Auth
{
    public interface IAuthService
    {
        Task<Response<NoContent>> Logout(int userId);
        Task<Response<LoginResDto>> EmployeeLogin(LoginEmployeeReqDto request);
        Task<Response<LoginResDto>> CustomerLogin(LoginCustomerReqDto request);
        Task<Response<User>> RegisterCustomer(CreateCustomerDto createCustomerDto);
        Task<Response<User>> RegisterEmployee(CreateEmployeeDto createEmployeeDto);
    }
}

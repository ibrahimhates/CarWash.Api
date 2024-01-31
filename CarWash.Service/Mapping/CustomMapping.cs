using CarWash.Entity.Dtos.Appointment;
using CarWash.Entity.Dtos.Customer;
using CarWash.Entity.Dtos.Employee;
using CarWash.Entity.Dtos.UserDtos;
using CarWash.Entity.Dtos.VehicleDtos;
using CarWash.Entity.Dtos.WashPackage;
using CarWash.Entity.Entities;
using System.Collections.Generic;

namespace CarWash.Service.Mapping
{
    internal class CustomMapping : AutoMapper.Profile
    {
        public CustomMapping()
        {
            CreateMap<CreateEmployeeDto, User>()
                .ForMember(dest => dest.NormalizedEmail, opt => opt.MapFrom(src => src.Email.ToUpper()))
                .ForMember(dest => dest.NormalizedUserName, opt => opt.MapFrom(src => src.UserName.ToUpper()))
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => $"{src.FirstName} {src.LastName}"));


            #region VehicleDto Mapping

            CreateMap<VehicleUpdateDto, Vehicle>();

            CreateMap<VehicleCreateDto, Vehicle>();

            CreateMap<Vehicle, VehicleListDto>();

            CreateMap<Vehicle, VehicleListForAppointmentDto>()
                .ForMember(dest => dest.BrandName, opt =>
                    opt.MapFrom(src => src.Brand.Name));

            #endregion

            #region BrandDto Mapping

            CreateMap<Brand, BrandDto>();

            #endregion

            #region EmployeeListDto

            CreateMap<Employee, EmployeeListDto>()
                .ForMember(dest => dest.RoleName, opt =>
                    opt.MapFrom(src => src.Role.RoleName))
                .ForMember(dest => dest.HireDate, opt =>
                    opt.MapFrom(src => src.EmployeeAttendance.HireDate))
                .ForMember(dest => dest.FullName, opt =>
                    opt.MapFrom(src => src.User.FullName));

            #endregion

            CreateMap<CreateCustomerDto, User>()
                .ForMember(dest => dest.NormalizedEmail, opt => opt.MapFrom(src => src.Email.ToUpper()))
                .ForMember(dest => dest.NormalizedUserName, opt => opt.MapFrom(src => src.UserName.ToUpper()))
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => $"{src.FirstName} {src.LastName}"));

            CreateMap<CreateEmployeeAttandaceDto, EmployeeAttendance>();

            CreateMap<WashPackageDto, WashPackage>().ReverseMap();
            CreateMap<WashPackage, WashPackageForCustDto>();
            CreateMap<CreateAppointmentDto, Appointment>();

            CreateMap<Appointment, AppointmentListDto>().ForMember(dest => dest.Rating,
                    opt => opt.MapFrom(src => src.WashProcess.ServiceReview.Rating))
                .ForMember(dest => dest.PackageName, opt => opt.MapFrom(src => src.WashPackage.PackageName))
                .ForMember(dest => dest.CarWashStatus, opt => opt.MapFrom(src => src.WashProcess.CarWashStatus));

            CreateMap<Appointment, AppointmentForManagerDto>().ForMember(dest => dest.Rating, opt => opt.MapFrom(src => src.WashProcess.ServiceReview.Rating))
                .ForMember(dest => dest.PackageName, opt => opt.MapFrom(src => src.WashPackage.PackageName))
                .ForMember(dest => dest.CarWashStatus, opt => opt.MapFrom(src => src.WashProcess.CarWashStatus));
          
            CreateMap<User, UserInfoDto>().ReverseMap();
        }
    }
}
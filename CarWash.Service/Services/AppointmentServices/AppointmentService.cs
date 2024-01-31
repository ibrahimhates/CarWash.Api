using CarWash.Core.Dtos;
using CarWash.Entity.Dtos.Appointment;
using CarWash.Entity.Entities;
using CarWash.Entity.Enums;
using CarWash.Repository.Repositories.AppointmentRepo;
using CarWash.Repository.Repositories.Employees;
using CarWash.Repository.Repositories.EwpRepo;
using CarWash.Repository.Repositories.WashPackages;
using CarWash.Repository.UnitOfWork;
using CarWash.Service.Mapping;
using CarWash.Service.ServiceExtensions;
using CarWash.Service.Services.EmployeeServices;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Nest;
using System;
using System.Diagnostics;
using CarWash.Repository.Repositories.ServiceReviews;

namespace CarWash.Service.Services.AppointmentServices
{
    public class AppointmentService : IAppointmentService
    {
        private readonly ILogger<AppointmentService> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IWashPackageRepository _workPackageRepository;
        private readonly IEwbRepo _ewbRepo;
        private readonly IServiceReviewRepository _reviewRepository;

        public AppointmentService(IUnitOfWork unitOfWork, ILogger<AppointmentService> logger,
            IAppointmentRepository appointmentRepository, IEmployeeRepository employeeRepository,
            IWashPackageRepository workPackageRepository, IEwbRepo ewbRepo, IServiceReviewRepository reviewRepository)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _appointmentRepository = appointmentRepository;
            _employeeRepository = employeeRepository;
            _workPackageRepository = workPackageRepository;
            _ewbRepo = ewbRepo;
        }



        public async Task<Response<NoContent>> CreateAppointment(CreateAppointmentDto request)
        {
            _logger.SendInformation(nameof(CreateAppointment), "Started");

            try
            {
                var package = await _workPackageRepository.FindByCondition(p => p.Id == request.PackageId, true)
                    .FirstOrDefaultAsync();
                var duration = request.AppointmentDate.AddMinutes(package.Duration);
                var availableWorkers = await _employeeRepository.GetWorkersWithAvailableTimeSlots(request.AppointmentDate, duration);

                // Check employee availability
                if (!availableWorkers.Any())
                {
                    _logger.SendWarning(nameof(CreateAppointment), "Employee not available at the specified time");
                    return Response<NoContent>.Fail("Şuan bu tarih-saat için uygun randevumuz bulunmamakta", 400);
                }

                var empId = 0;
                // Check for overlapping wash processes
                foreach (var worker in availableWorkers)
                {

                    var emp = await _appointmentRepository.FindAll()
                        .Include(a => a.WashProcess)
                        .ThenInclude(a => a.Employees)
                        .Include(a => a.WashPackage)
                        .Where(wp => wp.WashProcess.CarWashStatus != CarWashStatus.Completed &&
                                     (request.AppointmentDate <
                                      wp.AppointmentDate.AddMinutes(wp.WashPackage.Duration) &&
                                      duration > wp.WashProcess.Appointment.AppointmentDate)).Select(a =>
                            a.WashProcess.Employees.Select(a => a.Employee).FirstOrDefault())
                        .Where(a => a.UserId == worker.UserId)
                        .FirstOrDefaultAsync();

                    
                    if (emp is null)
                    {
                        empId = worker.UserId;
                        break;
                    }
                }

                if (empId == 0)
                {
                    _logger.SendWarning(nameof(CreateAppointment), "Appointment overlaps with existing wash process");
                    return Response<NoContent>.Fail("Şuan bu tarih-saat için uygun randevumuz bulunmamakta", 400);
                }

                var appointment = ObjectMapper.Mapper.Map<Appointment>(request);
                appointment.WashProcess = new WashProcess()
                {
                    CarWashStatus = CarWashStatus.Waiting,
                    WashPackageId = package.Id,
                };

                // Additional checks for employee availability can be added here if needed

                await _appointmentRepository.CreateAsync(appointment);
                var varable = new EmployeeWashProcess()
                {
                    EmployeeId = empId,
                    WashProcess = appointment.WashProcess 
                };

                await _ewbRepo.CreateAsync(varable);
                await _unitOfWork.SaveChangesAsync();
                _logger.SendInformation(nameof(CreateAppointment), "Create successful");
                return Response<NoContent>.Success(201);
            }
            catch (Exception ex)
            {
                _logger.SendWarning(nameof(CreateAppointment), ex.Message);
                return Response<NoContent>.Fail("Randevu olusturulurken hata ile karsilandi", 500);
            }
        }

        public async Task<Response<NoContent>> DeleteAppointment(int id)
        {
            _logger.SendInformation(nameof(DeleteAppointment), "Started");
            try
            {
                var appointmentToDelete = await _appointmentRepository.FindByCondition(a => a.Id == id, true)
                    .Include(a => a.WashProcess).FirstOrDefaultAsync();

                if (appointmentToDelete == null )
                {
                    _logger.SendWarning(nameof(DeleteAppointment), "Appointment not found");
                    return Response<NoContent>.Fail("Appointment not found", 404);
                }
                else if(appointmentToDelete.WashProcess.CarWashStatus != CarWashStatus.Waiting)
                {
                    _logger.SendWarning(nameof(DeleteAppointment), "Randevu saati geldiği için iptal edemezsiniz");
                    return Response<NoContent>.Fail("Randevu saati geldiği için iptal edemezsiniz", 404);
                }
                
                _appointmentRepository.Delete(appointmentToDelete);
                await _unitOfWork.SaveChangesAsync();

                _logger.SendInformation(nameof(DeleteAppointment), "Delete successful");
                return Response<NoContent>.Success(204);
            }
            catch (Exception ex)
            {
                _logger.SendWarning(nameof(DeleteAppointment), ex.Message);
                return Response<NoContent>.Fail("Bilinmedik bir hata oluştu", 500);
            }
        }

        public async Task<Response<List<AppointmentListDto>>> GetAppointmentsByCustId(int custId)
        {
            _logger.SendInformation(nameof(GetAppointmentsByCustId), "Started");
            try
            {
                var appointments = await _appointmentRepository.GetAppointmentByCustIdAsync(custId);
                
                foreach ( var appointment in appointments)
                {
                    if(appointment.WashProcess.CarWashStatus != CarWashStatus.Completed)
                    {
                        if (appointment.AppointmentDate.AddMinutes(appointment.WashPackage.Duration) < DateTime.Now)
                        {
                            appointment.WashProcess.CarWashStatus = CarWashStatus.Completed;
                            appointment.Vehicle.LastWashDate = appointment.AppointmentDate.AddMinutes(appointment.WashPackage.Duration);
                            await _unitOfWork.SaveChangesAsync();
                        }
                        else if (appointment.AppointmentDate <= DateTime.Now && DateTime.Now <= appointment.AppointmentDate.AddMinutes(appointment.WashPackage.Duration))
                        {
                            appointment.WashProcess.CarWashStatus = CarWashStatus.InProcess;
                            await _unitOfWork.SaveChangesAsync();
                        }
                    }
                }
                var appointmentDtos = ObjectMapper.Mapper.Map<List<AppointmentListDto>>(appointments);
                
                _logger.SendInformation(nameof(GetAppointmentsByCustId), "Retrieve successful");
                return Response<List<AppointmentListDto>>.Success(appointmentDtos,200);
            }
            catch (Exception ex)
            {
                _logger.SendWarning(nameof(GetAppointmentsByCustId), ex.Message);
                return Response<List<AppointmentListDto>>.Fail("Bilinmedik bir hata oluştu", 500);
            }
        }

        public async Task<Response<List<AppointmentListDto>>> GetAppointmentsByEmpId(int empId)
        {
            _logger.SendInformation(nameof(GetAppointmentsByEmpId), "Started");
            try
            {
                var appointments = await _appointmentRepository.GetAppointmentByEmpIdAsync(empId);

                var appointmentDtos = ObjectMapper.Mapper.Map<List<AppointmentListDto>>(appointments);

                _logger.SendInformation(nameof(GetAppointmentsByEmpId), "Retrieve successful");
                return Response<List<AppointmentListDto>>.Success(appointmentDtos, 200);
            }
            catch (Exception ex)
            {
                _logger.SendWarning(nameof(GetAppointmentsByEmpId), ex.Message);
                return Response<List<AppointmentListDto>>.Fail("Bilinmedik bir hata oluştu", 500);
            }
        }

        public async Task<Response<NoContent>> Update(AppointmentListDto updatedAppointment)
        {
            _logger.SendInformation(nameof(Update), "Started");
            try
            {
                var appointment = await _appointmentRepository.FindByCondition(a => a.Id == updatedAppointment.Id, true)
                    .Include(a => a.WashProcess).FirstOrDefaultAsync();

                if (appointment == null)
                {
                    _logger.SendWarning(nameof(Update), "Appointment not found");
                    return Response<NoContent>.Fail("Appointment not found", 404);
                }
                else if (appointment.WashProcess.CarWashStatus != CarWashStatus.Waiting)
                {
                    _logger.SendWarning(nameof(Update), "Randevu saati geldiği için düzenleme yapamazsınız");
                    return Response<NoContent>.Fail("Randevu saati geldiği için düzenleme yapamazsınız", 404);
                }
                var appointmentDtos = ObjectMapper.Mapper.Map(updatedAppointment,appointment);

                _logger.SendInformation(nameof(Update), "update successful");
                return Response<NoContent>.Success(204);
            }
            catch (Exception ex)
            {
                _logger.SendWarning(nameof(Update), ex.Message);
                return Response<NoContent>.Fail("Bilinmedik bir hata oluştu", 500);
            }
        }

        public async Task<Response<NoContent>> AppointmentByScore(AppointmentScoreDto scoreDto)
        {
            var washProcess = await _appointmentRepository
                .FindByCondition(x => x.WashProcess.AppointmentId == scoreDto.Id)
                .Select(x => x.WashProcess)
                .FirstOrDefaultAsync();

            if (washProcess is not WashProcess)
            {
                return Response<NoContent>.Fail("Oylamak istediginiz randevu bulunamadi!", 404);
            }

            ServiceReview serviceReview = new()
            {
                WashProcess = washProcess,
                Comment = scoreDto.Comment,
                Rating = scoreDto.Rating
            };
            try
            {
                await _reviewRepository.CreateAsync(serviceReview);
                await _unitOfWork.SaveChangesAsync();
                return Response<NoContent>.Success(200);
            }
            catch (Exception ex)
            {
                _logger.SendWarning(nameof(Update), ex.Message);
                return Response<NoContent>.Fail("Bilinmedik bir hata oluştu", 500);
            }
        }

        public async Task<Response<List<AppointmentListDto>>> GetAll()
        {
            _logger.SendInformation(nameof(Update), "Started");
            try
            {
                var appointments = await _appointmentRepository.GetAll();
                foreach (var appointment in appointments)
                {
                    if (appointment.WashProcess.CarWashStatus != CarWashStatus.Completed)
                    {
                        if (appointment.AppointmentDate.AddMinutes(appointment.WashPackage.Duration) < DateTime.Now)
                        {
                            appointment.WashProcess.CarWashStatus = CarWashStatus.Completed;
                            appointment.Vehicle.LastWashDate = appointment.AppointmentDate.AddMinutes(appointment.WashPackage.Duration);
                            await _unitOfWork.SaveChangesAsync();
                        }
                        else if (appointment.AppointmentDate <= DateTime.Now && DateTime.Now <= appointment.AppointmentDate.AddMinutes(appointment.WashPackage.Duration))
                        {
                            appointment.WashProcess.CarWashStatus = CarWashStatus.InProcess;
                            await _unitOfWork.SaveChangesAsync();
                        }
                    }
                }
                var appointmentDtos = ObjectMapper.Mapper.Map< List <AppointmentListDto>> (appointments);
               

                _logger.SendInformation(nameof(Update), "update successful");
                return Response< List <AppointmentListDto>>.Success(appointmentDtos, 200);
            }
            catch (Exception ex)
            {
                _logger.SendWarning(nameof(Update), ex.Message);
                return Response< List <AppointmentListDto>>.Fail("Bilinmedik bir hata oluştu", 500);
            }
        }
    }
}

using CarWash.Core.Dtos;
using CarWash.Entity.Dtos.WashPackage;
using CarWash.Entity.Entities;
using CarWash.Repository.Repositories.WashPackages;
using CarWash.Repository.UnitOfWork;
using CarWash.Service.Mapping;
using CarWash.Service.ServiceExtensions;
using CarWash.Service.Services.EmployeeServices;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace CarWash.Service.Services.WashPackageServices
{
    public class WashPackageService : IWashPackageService
    {
        private readonly ILogger<EmployeService> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWashPackageRepository _washPackageRepository;

        public WashPackageService(IUnitOfWork unitOfWork, ILogger<EmployeService> logger, IWashPackageRepository washPackageRepository)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _washPackageRepository = washPackageRepository;
        }

        public async Task<Response<NoContent>> CreateWashPackage(WashPackageDto request)
        {
            _logger.SendInformation(nameof(CreateWashPackage), "Started");
            try
            {
                var package = ObjectMapper.Mapper.Map<WashPackage>(request);
                await _washPackageRepository.CreateAsync(package);
                await _unitOfWork.SaveChangesAsync();
                _logger.SendInformation(nameof(CreateWashPackage), "Create successful");
                return Response<NoContent>.Success(201);
            }
            catch (Exception ex)
            {
                _logger.SendWarning(nameof(CreateWashPackage), ex.Message);
                return Response<NoContent>.Fail("Bilinmedik bir hata oluştu", 500);
            }

        }

        public async Task<Response<NoContent>> UpdateWashPackage(WashPackageDto request)
        {
            _logger.SendInformation(nameof(UpdateWashPackage), "Started");
            try
            {
                var existingPackage = await _washPackageRepository.GetByIdAsync(request.Id);

                if (existingPackage == null)
                {
                    _logger.SendWarning(nameof(UpdateWashPackage), "Wash package not found");
                    return Response<NoContent>.Fail("Wash package not found", 404);
                }

                existingPackage = ObjectMapper.Mapper.Map(request, existingPackage);

                _washPackageRepository.Update(existingPackage);
                await _unitOfWork.SaveChangesAsync();

                _logger.SendInformation(nameof(UpdateWashPackage), "Update successful");
                return Response<NoContent>.Success(204);
            }
            catch (Exception ex)
            {
                _logger.SendError(ex, nameof(UpdateWashPackage));
                return Response<NoContent>.Fail("Bilinmedik bir hata oluştu", 500);
            }
        }

        public async Task<Response<NoContent>> DeleteWashPackage(int id)
        {
            _logger.SendInformation(nameof(DeleteWashPackage), "Started");
            try
            {
                var packageToDelete = await _washPackageRepository.GetByIdAsync(id);

                if (packageToDelete == null)
                {
                    _logger.SendWarning(nameof(DeleteWashPackage), "Wash package not found");
                    return Response<NoContent>.Fail("Wash package not found", 404);
                }

                packageToDelete.IsDeleted = true;
                _washPackageRepository.Update(packageToDelete);
                await _unitOfWork.SaveChangesAsync();

                _logger.SendInformation(nameof(DeleteWashPackage), "Soft delete successful");
                return Response<NoContent>.Success(204);
            }
            catch (Exception ex)
            {
                _logger.SendError(ex, nameof(DeleteWashPackage));
                return Response<NoContent>.Fail("Bilinmedik bir hata oluştu", 500);
            }
        }

        public async Task<Response<List<WashPackageDto>>> GetWashPackages()
        {
            _logger.SendInformation(nameof(GetWashPackages), "Started");
            try
            {
                var packages = await _washPackageRepository.FindAll().ToListAsync();
                var packageDtos = ObjectMapper.Mapper.Map<List<WashPackageDto>>(packages);

                _logger.SendInformation(nameof(GetWashPackages), "Retrieve successful");
                return Response<List<WashPackageDto>>.Success(packageDtos,200);
            }
            catch (Exception ex)
            {
                _logger.SendError(ex, nameof(GetWashPackages));
                return Response <List<WashPackageDto>>.Fail("Bilinmedik bir hata oluştu", 500);
            }
        }

        public async Task<Response<List<WashPackageForCustDto>>> GetAllPackageForCustomer()
        {
            _logger.SendInformation(nameof(GetWashPackages), "Started");
            try
            {
                var packages = await _washPackageRepository.FindAll().ToListAsync();
                var packageDtos = ObjectMapper.Mapper.Map<List<WashPackageForCustDto>>(packages);

                _logger.SendInformation(nameof(GetWashPackages), "Retrieve successful");
                return Response<List<WashPackageForCustDto>>.Success(packageDtos,200);
            }
            catch (Exception ex)
            {
                _logger.SendError(ex, nameof(GetWashPackages));
                return Response <List<WashPackageForCustDto>>.Fail("Bilinmedik bir hata oluştu", 500);
            }
        }
    }
}
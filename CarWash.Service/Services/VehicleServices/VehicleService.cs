using CarWash.Core.Dtos;
using CarWash.Entity.Dtos.VehicleDtos;
using CarWash.Entity.Entities;
using CarWash.Repository.Repositories.Brands;
using CarWash.Repository.Repositories.Vehicles;
using CarWash.Repository.UnitOfWork;
using CarWash.Service.Mapping;
using Microsoft.EntityFrameworkCore;

namespace CarWash.Service.Services.VehicleServices;

public class VehicleService : IVehicleService
{
    private readonly IVehicleRepository _vehicleRepository;
    private readonly IBrandRepository _brandRepository;
    private readonly IUnitOfWork _unitOfWork;

    public VehicleService(IVehicleRepository vehicleRepository, IBrandRepository brandRepository, IUnitOfWork unitOfWork)
    {
        _vehicleRepository = vehicleRepository;
        _brandRepository = brandRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Response<IEnumerable<VehicleListDto>>> GetAllVehicles(int id)
    {
        var vehicles = await _vehicleRepository.GetAllVehicles(id);

        var vehiclesDto = ObjectMapper.Mapper.Map<List<VehicleListDto>>(vehicles);

        return Response<IEnumerable<VehicleListDto>>.Success(vehiclesDto, 200);
    }

    public async Task<Response<IEnumerable<VehicleListForAppointmentDto>>> GetAllVehiclesForAppointment(int id)
    {
        var vehicles = await _vehicleRepository.GetAllVehicles(id);
        
        var vehiclesDto = ObjectMapper.Mapper.Map<List<VehicleListForAppointmentDto>>(vehicles);
        
        return Response<IEnumerable<VehicleListForAppointmentDto>>.Success(vehiclesDto, 200);
    }

    public async Task<Response<IEnumerable<BrandDto>>> GetAllBrands()
    {
        var brands = await _brandRepository
            .FindAll(false)
            .ToListAsync();

        var brandDtos = ObjectMapper.Mapper.Map<List<BrandDto>>(brands);
        
        return Response<IEnumerable<BrandDto>>.Success(brandDtos,200);
    }

    public async Task<Response<NoContent>> CreateVehicle(VehicleCreateDto vehicle)
    {
        var vehicleCreate = ObjectMapper.Mapper.Map<Vehicle>(vehicle);
        
        vehicleCreate.CreatedAt = DateTime.Now;
        vehicleCreate.LastWashDate = DateTime.MinValue;

        await _vehicleRepository.CreateAsync(vehicleCreate);

        await _unitOfWork.SaveChangesAsync();

        return Response<NoContent>.Success(201);
    }

    public async Task<Response<NoContent>> UpdateVehicle(VehicleUpdateDto vehicle)
    {
        var vehicleUpdate = await _vehicleRepository
            .FindByCondition(x => x.Id == vehicle.Id)
            .FirstOrDefaultAsync();

        if(vehicleUpdate is not Vehicle)
            return Response<NoContent>.Fail("Silinecek Arac bulunamadi",404);
        
        vehicleUpdate = ObjectMapper.Mapper.Map(vehicle, vehicleUpdate);

        _vehicleRepository.Update(vehicleUpdate);

        await _unitOfWork.SaveChangesAsync();
        
        return Response<NoContent>.Success(200);
        
    }

    public async Task<Response<NoContent>> DeleteVehicle(int id)
    {
        var vehicleDelete = await _vehicleRepository
            .GetByIdAsync(id);

        if (vehicleDelete is not Vehicle)
            return Response<NoContent>.Fail("Silmek istediginiz arac bulunamadi",404);

        _vehicleRepository.Delete(vehicleDelete);

        await _unitOfWork.SaveChangesAsync();
        
        return Response<NoContent>.Success(200);
    }
}
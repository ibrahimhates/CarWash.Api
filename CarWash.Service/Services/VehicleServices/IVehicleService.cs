using CarWash.Core.Dtos;
using CarWash.Entity.Dtos.VehicleDtos;

namespace CarWash.Service.Services.VehicleServices;

public interface IVehicleService
{
    Task<Response<IEnumerable<VehicleListDto>>> GetAllVehicles(int id);
    Task<Response<IEnumerable<VehicleListForAppointmentDto>>> GetAllVehiclesForAppointment(int id);
    Task<Response<IEnumerable<BrandDto>>> GetAllBrands();
    Task<Response<NoContent>> CreateVehicle(VehicleCreateDto vehicle);
    Task<Response<NoContent>> UpdateVehicle(VehicleUpdateDto vehicle);
    Task<Response<NoContent>> DeleteVehicle(int id);
}
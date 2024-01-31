using CarWash.Core.Dtos;
using CarWash.Entity.Dtos.WashPackage;

namespace CarWash.Service.Services.WashPackageServices
{
    public interface IWashPackageService 
    {
        Task<Response<NoContent>> CreateWashPackage(WashPackageDto request);
        Task<Response<NoContent>> UpdateWashPackage(WashPackageDto request);
        Task<Response<NoContent>> DeleteWashPackage(int id);
        Task<Response<List<WashPackageDto>>> GetWashPackages();
        Task<Response<List<WashPackageForCustDto>>> GetAllPackageForCustomer();

    }
}

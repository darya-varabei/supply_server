using SupplyIO.SupplyIO.Services.Models;
using SupplyIO.SupplyIO.Services.Models.CertificateModel;
using SupplyIO.SupplyIO.Services.ViewModel;

namespace SupplyIO.SupplyIO.Services
{
    public interface IMetalService
    {
        public Task<Certificate> CreateFromLinkAsync(CertificateLink link);
        public Task<Certificate> CheckSertificateAsync(CertificateLink link);
        public Task<int> CreateCertificateAsync(Certificate certificate);
        public Task<Certificate> GetCertificateAsync(int id);
        public Task<int> UpdateCertificateAsync(Certificate certificate);
        public Task<List<Certificate>> GetAllCertificatesAsync();
        public Task<List<PackageViewModel>> GetAllPackagesAsync();
        public Task UpdateStatusPackageAsync(int PackageId, string status);
        public Task<int> SetPackageInProcessingForPartAsync(int packageId, double? net, double? width);
        public Task<List<PackageViewModel>> GetPackagesByStatus(string statusName);
        public Task<int> AddPackage(Package package);
        public Task<int> AddDeffectToPackage(Defect defect);
        public Task<ExtendedPackageViewModel> GetPackage(int packageId);
        public Task<int> UpdatePackageAsync(ExtendedPackageViewModel package);
        public Task<int> AddPackageAsync(ExtendedPackageViewModel package);
        public Task<List<string>> GetNumbersOfCertificates();
        public Task<List<PackageViewModel>> SearchAsync(string searchString, string status);
        public Task<string> GetSupplierByNumberOfCertificate(string number);
    }
}

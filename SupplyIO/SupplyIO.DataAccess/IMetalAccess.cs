using SupplyIO.SupplyIO.Services.Models.CertificateModel;
using SupplyIO.SupplyIO.Services.ViewModel;

namespace SupplyIO.SupplyIO.DataAccess
{
    public interface IMetalAccess
    {
        public Task<int> AddCertificateAsync(Certificate certificate);
        public Task<Certificate> CheckSertificateAsync(string link);
        public Task<Certificate> AddNewPackagesToCertificate(Certificate certificate);
        public Task<Certificate> CheckPackageAsync(string link);
        public Task<Certificate> GetCertificateAsync(int id);
        public Task<Certificate> GetCertificateAsync(string number);
        public Task<List<Certificate>> GetAllCertificatesAsync();
        public Task<int> UpdateSertificateAsync(Certificate certificate);
        public Task<List<Package>> GetAllPackegesAsync();
        public Task UpdateStatusPackageAsync(int packageId, string status);
        public Task<int> SetPackageInProcessingForPartAsync(int packageId, double? net, double? width);
        public Task<List<Package>> GetPackagesByStatus(string statusName);
        public Task<int> UpdatePackageAsync(Package package);
        public Task<int> AddPackage(Package package);
        public Task<int> AddPackageAsync(string numberOfSert, Package package);
        public Task<int> AddDeffectToPackage(Defect defect);
        public Task<Package> GetPackage(int packageId);
        public Task<List<string>> GetNumbersOfCertificates();
        public Task<List<Package>> SearchAsync(string searchString, string status);
        public Task<string> GetSupplierByNumberOfCertificate(string number);
    }
}

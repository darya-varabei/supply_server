using Microsoft.EntityFrameworkCore;
//using Microsoft.IdentityModel.Protocols.WSTrust;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Npgsql.Internal.TypeHandlers.NumericHandlers;
using SupplyIO.SupplyIO.Services.Models.CertificateModel;
using SupplyIO.SupplyIO.Services.Models.Context;
using SupplyIO.SupplyIO.Services.ViewModel;
using System.Drawing;

namespace SupplyIO.SupplyIO.DataAccess.PostgreSQL
{
    public class MetalAccess : IMetalAccess
    {
        private readonly int UpdateTimeHours;
        private readonly int UpdateTimeMonth;
        private readonly int ExpectationTimeDays;

        private MetalContext _context;

        public MetalAccess(MetalContext context, IConfiguration configuration)
        {
            _context = context;
            UpdateTimeHours = int.Parse(configuration.GetSection("UpdateTimeHours").Value);
            UpdateTimeMonth = int.Parse(configuration.GetSection("UpdateTimeMonth").Value);
            ExpectationTimeDays = int.Parse(configuration.GetSection("ExpectationTimeDays").Value);
        }

        public async Task<int> AddCertificateAsync(Certificate certificate)
        {
            certificate = certificate ?? throw new ArgumentNullException(nameof(certificate));

            await SetStatus(certificate, "В ожидании");

            var cert = await _context.Certificate.AddAsync(certificate);
            await _context.SaveChangesAsync();

            await UpdateStatus();

            return cert.OriginalValues.GetValue<int>("CertificateId");
        }

        public async Task<Certificate> AddNewPackagesToCertificate(Certificate certificate)
        {
            var cert = await GetCertificateAsync(certificate.Number);

            await SetStatus(certificate, "В ожидании");
            var status = await GetStatus("В ожидании");

            var packages = cert.Packages.UnionBy(certificate.Packages, pac => pac.Batch).ToList();

            cert.Packages = packages;

            await _context.SaveChangesAsync();

            return cert;
        }

        public async Task<Certificate> CheckSertificateAsync(string link)
        {
            await UpdateStatus();

            var certificate = await _context.Certificate.Include(cert => cert.Product)
                                                        .Include(cert => cert.Packages)
                                                            .ThenInclude(pac => pac.Size)
                                                        .Include(cert => cert.Packages)
                                                            .ThenInclude(pac => pac.Weight)
                                                        .Include(cert => cert.Packages)
                                                            .ThenInclude(pac => pac.ChemicalComposition)
                                                        .Include(cert => cert.Packages)
                                                            .ThenInclude(pac => pac.ImpactStrength)
                                                        .Include(cert => cert.Packages)
                                                            .ThenInclude(pac => pac.Status)
                                                        .FirstOrDefaultAsync(cert => cert.Link.Contains(link));


            return certificate;
        }

        public async Task<Certificate> CheckPackageAsync(string link)
        {
            await UpdateStatus();

            var package = await _context.Package.Include(pac => pac.Certificate)
                                                .FirstOrDefaultAsync(pac => pac.Link == link);

            if (package is null)
                return null;

            var certificate = await GetCertificateAsync(package.Certificate.CertificateId);

            return certificate;
        }

        public async Task<List<Certificate>> GetAllCertificatesAsync()
        {
            await UpdateStatus();

            var certificates = await _context.Certificate.Include(cert => cert.Product)
                                                 .Include(cert => cert.Packages)
                                                     .ThenInclude(pac => pac.Size)
                                                 .Include(cert => cert.Packages)
                                                     .ThenInclude(pac => pac.Weight)
                                                 .Include(cert => cert.Packages)
                                                     .ThenInclude(pac => pac.ChemicalComposition)
                                                 .Include(cert => cert.Packages)
                                                     .ThenInclude(pac => pac.ImpactStrength)
                                                 .Include(cert => cert.Packages)
                                                     .ThenInclude(pac => pac.Status)
                                                 .ToListAsync();

            return certificates;
        }

        public async Task<Certificate> GetCertificateAsync(int id)
        {
            await UpdateStatus();

            var certificate = await _context.Certificate.Include(cert => cert.Product)
                                                 .Include(cert => cert.Packages)
                                                     .ThenInclude(pac => pac.Size)
                                                 .Include(cert => cert.Packages)
                                                     .ThenInclude(pac => pac.Weight)
                                                 .Include(cert => cert.Packages)
                                                     .ThenInclude(pac => pac.ChemicalComposition)
                                                 .Include(cert => cert.Packages)
                                                     .ThenInclude(pac => pac.ImpactStrength)
                                                 .Include(cert => cert.Packages)
                                                     .ThenInclude(pac => pac.Status)
                                                 .Where(cert => cert.CertificateId == id).FirstOrDefaultAsync();

            return certificate;
        }

        public async Task<Certificate> GetCertificateAsync(string number)
        {
            await UpdateStatus();

            var certificate = await _context.Certificate.Include(cert => cert.Product)
                .Include(cert => cert.Packages)
                .ThenInclude(pac => pac.Size)
                .Include(cert => cert.Packages)
                .ThenInclude(pac => pac.Weight)
                .Include(cert => cert.Packages)
                .ThenInclude(pac => pac.ChemicalComposition)
                .Include(cert => cert.Packages)
                .ThenInclude(pac => pac.ImpactStrength)
                .Include(cert => cert.Packages)
                .ThenInclude(pac => pac.Status)
                .Where(cert => cert.Number == number).FirstOrDefaultAsync();

            return certificate;
        }

        public async Task<int> UpdateSertificateAsync(Certificate certificate)
        {
            await UpdateStatus();

            var cert = await _context.Certificate.Include(cert => cert.Product)
                                                 .Include(cert => cert.Packages)
                                                     .ThenInclude(pac => pac.Size)
                                                 .Include(cert => cert.Packages)
                                                     .ThenInclude(pac => pac.Weight)
                                                 .Include(cert => cert.Packages)
                                                     .ThenInclude(pac => pac.ChemicalComposition)
                                                 .Include(cert => cert.Packages)
                                                     .ThenInclude(pac => pac.ImpactStrength)
                                                 .Include(cert => cert.Packages)
                                                     .ThenInclude(pac => pac.Status)
                                                 .Where(cert => cert.CertificateId == certificate.CertificateId).FirstOrDefaultAsync();

            for (var i = 0; i < certificate.Packages.Count(); i++)
            {
                cert.Packages[i].Weight.Net = certificate.Packages[i].Weight.Net;
                cert.Packages[i].Size.Thickness = certificate.Packages[i].Size.Thickness;
            }

            await _context.SaveChangesAsync();

            return certificate.CertificateId;
        }

        public async Task<List<Package>> GetAllPackegesAsync()
        {
            await UpdateStatus();

            var packeges = await _context.Package.Include(pac => pac.Certificate)
                                                 .Include(pac => pac.Weight)
                                                 .Include(pac => pac.Size)
                                                 .Include(pac => pac.Status)
                                                 .ToListAsync();

            return packeges;
        }

        public async Task<List<Package>> GetPackagesByStatus(string statusName)
        {
            await UpdateStatus();

            var status = await GetStatus(statusName);

            var packeges = await _context.Package.Include(pac => pac.Certificate)
                                                 .Include(pac => pac.Weight)
                                                 .Include(pac => pac.Size)
                                                 .Include(pac => pac.Status)
                                                 .Where(pac => pac.Status == status)
                                                 .ToListAsync();

            return packeges;
        }

        public async Task UpdateStatusPackageAsync(int packageId, string status)
        {
            await UpdateStatus();

            var statusDb = await GetStatus(status);

            var package = await _context.Package.Include(pac => pac.Status).FirstOrDefaultAsync(pac => pac.PackageId == packageId);

            package.Status = statusDb;
            package.DateChange = DateTime.UtcNow;

            await _context.SaveChangesAsync();
        }

        public async Task<int> SetPackageInProcessingForPartAsync(int packageId, double? net, double? width)
        {
            await UpdateStatus();

            if (net.HasValue || width.HasValue)
            {
                var statusDb = await GetStatus("В обработке");
                var statusAvailableDb = await GetStatus("Имеется");

                var package = await _context.Package.Include(pac => pac.ChemicalComposition)
                                                    .Include(pac => pac.ImpactStrength)
                                                    .Include(pac => pac.Size)
                                                    .Include(pac => pac.Weight)
                                                    .Include(pac => pac.Status)
                                                    .Include(pac => pac.Certificate)
                                                    .FirstOrDefaultAsync(pac => pac.PackageId == packageId && pac.Status == statusAvailableDb);

                var cert = package.Certificate;
                package.Certificate = null;
                package.Status = null;

                var pacStr = JsonConvert.SerializeObject(package);
                var newPackage = JsonConvert.DeserializeObject<Package>(pacStr);

                package.Certificate = cert;
                newPackage.Certificate = cert;

                newPackage.PackageId = 0;
                newPackage.ChemicalComposition.ChemicalCompositionId = 0;

                if (newPackage.ImpactStrength is not null)
                    newPackage.ImpactStrength.ImpactStrengthId = 0;

                newPackage.Size.SizeId = 0;
                newPackage.Weight.WeightId = 0;

                package.DateChange = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
                newPackage.DateChange = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);

                var netDb = package.Weight.Net;
                var widthDb = package.Size.Width;

                if (net.HasValue)
                {
                    if (netDb == net)
                    {
                        await UpdateStatusPackageAsync(packageId, "В обработке");
                    }
                    else if (netDb < net)
                        return -1;
                    else
                    {
                        var size = (net * widthDb) / netDb;

                        package.Size.Width = size;
                        package.Weight.Net = net;

                        newPackage.Size.Width = widthDb - size;
                        newPackage.Weight.Net = netDb - net;

                        package.Status = statusDb;
                        newPackage.Status = statusAvailableDb;

                        await _context.Package.AddAsync(newPackage);
                    }
                }
                else
                {
                    if (widthDb == width)
                    {
                        await UpdateStatusPackageAsync(packageId, "В обработке");
                    }
                    else if (widthDb < width)
                        return -1;
                    else
                    {
                        var weight = (width * netDb) / widthDb;

                        package.Size.Width = width;
                        package.Weight.Net = weight;

                        newPackage.Size.Width = widthDb - width;
                        newPackage.Weight.Net = netDb - weight;

                        package.Status = statusDb;
                        newPackage.Status = statusAvailableDb;

                        await _context.Package.AddAsync(newPackage);
                    }
                }

                return await _context.SaveChangesAsync();
            }

            return -1;
        }

        public async Task<int> AddPackage(Package package)
        {
            package.DateAdded = DateTime.UtcNow;
            package.DateChange = DateTime.UtcNow;
            package.Status = await GetStatus("Имеется");

            var pace = await _context.Package.Include(pac => pac.Weight)
                .Include(pac => pac.Size).FirstOrDefaultAsync(p => p.Batch == package.Batch);

            if (pace is null)
            {
                var pac = await _context.Package.AddAsync(package);
                await _context.SaveChangesAsync();

                await UpdateStatus();

                return pac.OriginalValues.GetValue<int>("PackageId");
            }
            else
            {
                pace.Status = package.Status;
                pace.DateChange = DateTime.UtcNow;
                pace.Grade = package.Grade;
                pace.Size.Thickness = package.Size.Thickness;
                pace.Size.Width = package.Size.Width;
                pace.Weight.Gross = package.Weight.Gross;

                await _context.SaveChangesAsync();

                await UpdateStatus();

                return -1;
            }
        }

        public async Task<int> AddPackageAsync(string numberOfCert, Package package)
        {
            package.DateChange = DateTime.UtcNow;
            package.DateAdded = DateTime.UtcNow;
            package.Status = await GetStatus("Имеется");

            var cert = await _context.Certificate.FirstOrDefaultAsync(cert => cert.Number == numberOfCert);
            package.Certificate = cert;

            var id = await _context.Package.AddAsync(package);
            await _context.SaveChangesAsync();
            return id.Entity.PackageId;
        }

        public async Task<Package> GetPackage(int packageId)
        {
            var package = await _context.Package.Include(pac => pac.ChemicalComposition)
                                                .Include(pac => pac.ImpactStrength)
                                                .Include(pac => pac.Size)
                                                .Include(pac => pac.Weight)
                                                .Include(pac => pac.Status)
                                                .Include(pac => pac.Certificate)
                                                .FirstOrDefaultAsync(pac => pac.PackageId == packageId);

            return package;
        }

        public async Task<int> AddDeffectToPackage(Defect defect)
        {
            var package = await _context.Package.FirstOrDefaultAsync(pac => pac.PackageId == defect.packageId);

            package.Photo = defect.Photo.Select(a => a.Select(b => (byte)b).ToArray()).ToList();
            package.Comment = defect.Comment;
            package.Status = await GetStatus("С дефектом");

            await _context.SaveChangesAsync();

            return package.PackageId;
        }

        public async Task<int> UpdatePackageAsync(Package package)
        {
            var packageDb = await _context.Package.Include(pac => pac.ChemicalComposition)
                                     .Include(pac => pac.ImpactStrength)
                                     .Include(pac => pac.Size)
                                     .Include(pac => pac.Weight)
                                     .FirstOrDefaultAsync(pac => pac.PackageId == package.PackageId);
            
            packageDb.Batch = package.Batch;
            packageDb.DateAdded = DateTime.SpecifyKind((DateTime)package.DateAdded, DateTimeKind.Utc);
            packageDb.Grade = package.Grade;

            packageDb.Size.Thickness = package.Size.Thickness;
            packageDb.Size.Width = package.Size.Width;

            packageDb.Weight.Net = package.Weight.Net;
            packageDb.Weight.Gross = package.Weight.Gross;

            packageDb.SurfaceQuality = package.SurfaceQuality;
            packageDb.Elongation = package.Elongation;
            packageDb.Variety = package.Variety;
            packageDb.Price = package.Price;
            packageDb.Heat = package.Heat;

            packageDb.ChemicalComposition.C = package.ChemicalComposition.C;
            packageDb.ChemicalComposition.Si = package.ChemicalComposition.Si;
            packageDb.ChemicalComposition.Mn = package.ChemicalComposition.Mn;
            packageDb.ChemicalComposition.S = package.ChemicalComposition.S;
            packageDb.ChemicalComposition.P = package.ChemicalComposition.P;
            packageDb.ChemicalComposition.Al = package.ChemicalComposition.Al;
            packageDb.ChemicalComposition.Cr = package.ChemicalComposition.Cr;
            packageDb.ChemicalComposition.Ni = package.ChemicalComposition.Ni;
            packageDb.ChemicalComposition.Cu = package.ChemicalComposition.Cu;
            packageDb.ChemicalComposition.Ti = package.ChemicalComposition.Ti;
            packageDb.ChemicalComposition.N2 = package.ChemicalComposition.N2;
            packageDb.ChemicalComposition.As = package.ChemicalComposition.As;

            packageDb.TrimOfEdge = package.TrimOfEdge;
            packageDb.TemporalResistance = package.TemporalResistance;
            packageDb.TensilePoint = package.TensilePoint;
            packageDb.GrainSize = package.GrainSize;

            packageDb.Comment = package.Comment;

            await _context.SaveChangesAsync();

            return packageDb.PackageId;
        }

        public async Task<List<string>> GetNumbersOfCertificates()
        {
            return await _context.Certificate.Select(cert => cert.Number).ToListAsync();
        }

        public async Task<List<Package>> SearchAsync(string searchString, string status)
        {
            var a = await _context.Package.Include(pac => pac.Certificate)
                                         .Include(pac => pac.Size)
                                         .Include(pac => pac.Weight)
                                         .Include(pac => pac.Status)
                                         .Where(pac => pac.Status.StatusName == status)
                                         .ToListAsync();

            var answer = new List<Package>();

            foreach (var pac in a)
            {
                var da = pac.DateAdded.Value.ToString("dd.MM.yyyy").Contains(searchString);
                var q = pac.Batch.Contains(searchString);
                var w = pac.Grade.Contains(searchString);
                var u = pac.Certificate.Number.Contains(searchString);
                var y = pac.Size.Width.ToString().Contains(searchString);
                var i = pac.Size.Thickness.ToString().Contains(searchString);
                var p = pac.Weight.Net.ToString().Contains(searchString);
                var s = string.IsNullOrEmpty(pac.Variety) ? false : pac.Variety.Contains(searchString);
                var d = pac.Certificate.Author.Contains(searchString);
                var f = pac.Elongation.ToString().Contains(searchString);
                var g = !pac.Price.HasValue ? false : pac.Price.ToString().Contains(searchString);
                var h = string.IsNullOrEmpty(pac.Comment) ? false : pac.Comment.Contains(searchString);

                var res = da || q || w || u || y || i || p || s || d || f || g || h;

                if (res)
                    answer.Add(pac);

            }

            return answer;
        }

        private async Task UpdateStatus()
        {
            var statusProcessing = await GetStatus("В обработке");
            var statusUses = await GetStatus("Использован");
            var statusExpectation = await GetStatus("В ожидании");

            //var a = await _context.Package.ToListAsync();

            var packagesDelete = _context.Package.Where(pac => pac.Status == statusUses)
                                                 .Where(pac => pac.DateChange != null)
                                                 .Where(pac => pac.DateChange <= DateTime.SpecifyKind(DateTime.Now.AddMonths(-UpdateTimeMonth), DateTimeKind.Utc));

            _context.Package.RemoveRange(packagesDelete);

            packagesDelete = _context.Package.Where(pac => pac.Status == statusExpectation)
                                             .Where(pac => pac.DateAdded != null)
                                             .Where(pac => pac.DateAdded <= DateTime.SpecifyKind(DateTime.Now.AddDays(-ExpectationTimeDays), DateTimeKind.Utc));

            _context.Package.RemoveRange(packagesDelete);

            var certificatesDelete = _context.Certificate.Where(cert => cert.Packages.Count == 0);

            _context.Certificate.RemoveRange(certificatesDelete);

            var packagesUpdate = _context.Package.Where(pac => pac.Status == statusProcessing)
                                                 .Where(pac => pac.DateChange != null)
                                                 .Where(pac => pac.DateChange <= DateTime.SpecifyKind(DateTime.Now.AddHours(-UpdateTimeHours), DateTimeKind.Utc))
                                                 .ToList();

            for (var i = 0; i < packagesUpdate.Count; i++)
            {
                packagesUpdate[i].Status = statusUses;
                packagesUpdate[i].DateChange = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
            }

            await _context.SaveChangesAsync();
        }

        private async Task SetStatus(Certificate certificate, string statusOfPackages)
        {
            var status = await GetStatus(statusOfPackages);

            for (var i = 0; i < certificate.Packages.Count; i++)
            {
                certificate.Packages[i].DateAdded = DateTime.UtcNow;
                certificate.Packages[i].DateChange = DateTime.UtcNow;
                certificate.Packages[i].Status = status;
            }
        }

        private async Task<Status> GetStatus(string name)
        {
            var status = await _context.Status.Where(status => status.StatusName == name)
                                              .FirstOrDefaultAsync();

            if (status is null)
            {
                status = new Status()
                {
                    StatusName = name,
                };

                await _context.Status.AddAsync(status);
                await _context.SaveChangesAsync();
            }

            return status;
        }

        public async Task<string> GetSupplierByNumberOfCertificate(string number)
        {
            var cert = await _context.Certificate.FirstOrDefaultAsync(cert => cert.Number.Trim() == number);

            return cert.Author;
        }
    }
}

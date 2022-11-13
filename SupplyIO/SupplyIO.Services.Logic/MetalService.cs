using Newtonsoft.Json;
using SupplyIO.SupplyIO.DataAccess;
using SupplyIO.SupplyIO.Services.Logic.ChainOfHosts;
using SupplyIO.SupplyIO.Services.Models;
using SupplyIO.SupplyIO.Services.Models.CertificateModel;
using SupplyIO.SupplyIO.Services.ViewModel;

namespace SupplyIO.SupplyIO.Services.Logic
{
    public class MetalService : IMetalService
    {
        private readonly IMetalAccess _access;
        private readonly Handler _nlmkCertificateHandler;

        public MetalService(IMetalAccess access)
        {
            _access = access;

            _nlmkCertificateHandler = new NlmkCertificateHandler();
            Handler nlmkPackageHandler = new NlmkPackageHandler();
            Handler metinvestHandler = new MetinvestHandler();
            Handler severstalHandler = new SeverstalHandler();

            _nlmkCertificateHandler.Successor = nlmkPackageHandler;
            nlmkPackageHandler.Successor = metinvestHandler;
            metinvestHandler.Successor = severstalHandler;
        }
        

        public async Task<int> AddPackage(Package package)
        {
            var id = await _access.AddPackage(package);

            return id;
        }

        public async Task<int> CreateCertificateAsync(Certificate certificate)
        {
            var id = await _access.AddCertificateAsync(certificate);

            return id;
        }

        public async Task<Certificate> CreateFromLinkAsync(CertificateLink link)
        {
            Console.WriteLine("Go to service");

            var uri = new Uri(link.Link);

            var certificate = await _nlmkCertificateHandler.HandleRequestAsync(uri);

            var certNumber = await _access.GetCertificateAsync(certificate.Number);
            var cert = await _access.CheckSertificateAsync(certificate.Link[0]);
            var pacCert = await _access.CheckPackageAsync(link.Link);

            if (cert is null && pacCert is null && certNumber is null)
            {
                var stringCert = JsonConvert.SerializeObject(certificate);
                var certificateDb = JsonConvert.DeserializeObject<Certificate>(stringCert);

                //certificateDb.Packages = new List<Package>();
                certificateDb.Date = DateTime.SpecifyKind((DateTime)certificateDb.Date, DateTimeKind.Utc);

                var id = await _access.AddCertificateAsync(certificateDb);

                certificate.CertificateId = id;
                return certificate;
            }
            else if (cert is not null && pacCert is null && certNumber is not null)
            {
                return await _access.AddNewPackagesToCertificate(certificate);
            }
            else if (cert is null && pacCert is null && certNumber is not null)
            {
                certNumber.Link.AddRange(certificate.Link);
                await _access.UpdateSertificateAsync(certNumber);

                certNumber.Packages = certificate.Packages;
                return certNumber;
            }
            else
            {
                return certificate;
            }
        }

        public async Task<Certificate> CheckSertificateAsync(CertificateLink link)
        {
            var certificate = await _access.CheckSertificateAsync(link.Link);

            if (certificate is null)
            {
                certificate = await _access.CheckPackageAsync(link.Link);

                var pac = certificate.Packages.FirstOrDefault(pac => pac.Link == link.Link);

                certificate.Packages = new List<Package>() { pac };
            }
            else
            {
                var packeges = certificate.Packages.Where(pac => pac.Status.StatusName != "В обработке").ToList();

                certificate.Packages = packeges;
            }

            return certificate;
        }

        public async Task<List<Certificate>> GetAllCertificatesAsync()
            => await _access.GetAllCertificatesAsync();

        public async Task<List<PackageViewModel>> GetAllPackagesAsync()
        {
            var packeges = await _access.GetAllPackegesAsync();

            return packeges.Select(pac => MapPackege(pac)).ToList();
        }

        public async Task<ExtendedPackageViewModel> GetPackage(int packageId)
        {
            var package = await _access.GetPackage(packageId);
            return MapExtendedPackageViewModel(package);
        }

        public async Task<Certificate> GetCertificateAsync(int id)
            => await _access.GetCertificateAsync(id);

        public async Task<List<PackageViewModel>> GetPackagesByStatus(string statusName)
        {
            var packages = await _access.GetPackagesByStatus(statusName);
            return packages.Select(pac => MapPackege(pac)).ToList();
        }

        public async Task<int> UpdateCertificateAsync(Certificate certificate)
            => await _access.UpdateSertificateAsync(certificate);

        public async Task UpdateStatusPackageAsync(int packageId, string status)
            => await _access.UpdateStatusPackageAsync(packageId, status);

        public async Task<int> SetPackageInProcessingForPartAsync(int packageId, double? net, double? width)
            => await _access.SetPackageInProcessingForPartAsync(packageId, net, width);

        public async Task<int> AddDeffectToPackage(Defect defect)
        {
            return await _access.AddDeffectToPackage(defect);
        }

        public async Task<int> UpdatePackageAsync(ExtendedPackageViewModel package)
        {
            return await _access.UpdatePackageAsync(MapExtendedPackage(package));
        }

        public async Task<int> AddPackageAsync(ExtendedPackageViewModel package)
        {
            var numberofCert = package.NumberOfCertificate;
            var pac = MapExtendedPackage(package);

            return await _access.AddPackageAsync(numberofCert, pac);
        }

        public async Task<List<string>> GetNumbersOfCertificates()
        {
            return await _access.GetNumbersOfCertificates();
        }

        public async Task<List<PackageViewModel>> SearchAsync(string searchString, string status)
        {
            return (await _access.SearchAsync(searchString, status)).Select(pac => MapPackege(pac))
                                                            .ToList();
        }

        public async Task<string> GetSupplierByNumberOfCertificate(string number)
        {
            return await _access.GetSupplierByNumberOfCertificate(number);
        }

        private ExtendedPackageViewModel MapExtendedPackageViewModel(Package package)
            => new()
            {
                PackageId = package.PackageId,

                NumberOfCertificate = package.Certificate.Number,
                Batch = package.Batch,
                SupplyDate = package.DateAdded,
                Supplier = package.Certificate.Author,
                Grade = package.Grade,
                Width = package.Size.Width,
                Thickness = package.Size.Thickness,
                Gros = package.Weight.Gross,
                Net = package.Weight.Net,
                CoatingClass = package.SurfaceQuality,
                Elongation = package.Elongation,
                Sort = package.Variety,
                Price = package.Price,

                NumberOfHeat = package.Heat,
                C = package.ChemicalComposition.C,
                Si = package.ChemicalComposition.Si,
                Mn = package.ChemicalComposition.Mn,
                S = package.ChemicalComposition.S,
                P = package.ChemicalComposition.P,
                Al = package.ChemicalComposition.Al,
                Cr = package.ChemicalComposition.Cr,
                Ni = package.ChemicalComposition.Ni,
                Cu = package.ChemicalComposition.Cu,
                Ti = package.ChemicalComposition.Ti,
                N2 = package.ChemicalComposition.N2,
                As = package.ChemicalComposition.As,

                TrimOfEdge = package.TrimOfEdge,
                TemporalResistance = package.TemporalResistance,
                TensilePoint = package.TensilePoint,
                GrainSize = package.GrainSize,

                Photo = package.Photo,
                Comment = package.Comment,
            };

        private Package MapExtendedPackage(ExtendedPackageViewModel package)
            => new()
            {
                PackageId = package.PackageId,

                Batch = package.Batch,
                DateAdded = package.SupplyDate,
                Grade = package.Grade,
                Size = new()
                {
                    Width = package.Width,
                    Thickness = package.Thickness,
                },
                Weight = new()
                {
                    Gross = package.Gros,
                    Net = package.Net,
                },
                SurfaceQuality = package.CoatingClass,
                Elongation = package.Elongation,
                Variety = package.Sort,
                Price = package.Price,
                Heat = package.NumberOfHeat,
                ChemicalComposition = new()
                {
                    C = package.C,
                    Si = package.Si,
                    Mn = package.Mn,
                    S = package.S,
                    P = package.P,
                    Al = package.Al,
                    Cr = package.Cr,
                    Ni = package.Ni,
                    Cu = package.Cu,
                    Ti = package.Ti,
                    N2 = package.N2,
                    As = package.As,
                },
                TrimOfEdge = package.TrimOfEdge,
                TemporalResistance = package.TemporalResistance,
                TensilePoint = package.TensilePoint,
                GrainSize = package.GrainSize,

                Photo = package.Photo,
                Comment = package.Comment,
            };

        private Package MapPackegeView(PackageViewModel package)
        {
            var cert = new Certificate()
            {
                Number = package.NumberOfCertificate,
                Author = package.Supplier,
            };

            var size = new Size()
            {
                Width = package.Width,
                Thickness = package.Thickness,
            };

            var weight = new Weight()
            {
                Net = package.Net,
                Gross = package.Weight,
            };

            var pac = new Package()
            {
                PackageId = package.PackageId,
                DateAdded = package.SupplyDate,
                Batch = package.Batch,
                Grade = package.Grade,
                Certificate = cert,
                Size = size,
                Weight = weight,
                Elongation = package.Elongation,
                Price = package.Price,
                Comment = package.Comment,
                Photo = package.Photo
            };

            return pac;
        }

        private PackageViewModel MapPackege(Package package)
            => new()
            {
                PackageId = package.PackageId,
                SupplyDate = package.DateAdded,
                Batch = package.Batch,
                Grade = package.Grade,
                NumberOfCertificate = package.Certificate.Number,
                Width = package.Size.Width,
                Thickness = package.Size.Thickness,
                Weight = package.Weight.Net,
                Mill = null,
                CoatingClass = package.Batch,
                Sort = package.Variety,
                Supplier = package.Certificate.Author,
                Elongation = package.Elongation,
                Price = package.Price,
                Comment = package.Comment,
                Status = package.Status.StatusName,
                Net = package.Weight.Net,
            };
    }
}

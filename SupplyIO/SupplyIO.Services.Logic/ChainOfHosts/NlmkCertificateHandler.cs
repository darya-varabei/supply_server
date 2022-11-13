using Newtonsoft.Json;
using SupplyIO.SupplyIO.Services.Models.CertificateModel;
using SupplyIO.SupplyIO.Services.Models.NlmkCertificateJson;
using System.Globalization;

namespace SupplyIO.SupplyIO.Services.Logic.ChainOfHosts
{
    public class NlmkCertificateHandler : Handler
    {
        private readonly string context = "nlmk.shop/c";

        public override async Task<Certificate> HandleRequestAsync(Uri link)
        {
            if (link.AbsoluteUri.Contains(context))
            {
                return await GetCertificateAsync(link);
            }
            else if (Successor != null)
            {
                return await Successor.HandleRequestAsync(link);
            }

            return null;
        }

        private async Task<Certificate> GetCertificateAsync(Uri link)
        {
            var _httpClient = new HttpClient();

            _httpClient.DefaultRequestHeaders.Add("accept", "application/json");
            _httpClient.DefaultRequestHeaders.Add("Accept-Encoding", "gzip, deflate, br");
            _httpClient.DefaultRequestHeaders.Add("Accept-Language", "ru-RU,ru;q=0.9,en-US;q=0.8,en;q=0.7");
            _httpClient.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/98.0.4758.82 Safari/537.36");
            _httpClient.DefaultRequestHeaders.Add("x-requested-with", "XMLHttpRequest");

            _httpClient.BaseAddress = new Uri("https://doc.nlmk.shop/api/v1/views/certificates/");

            var identyOfCertificate = link.GetComponents(UriComponents.Query, UriFormat.UriEscaped);

            var page = await _httpClient.GetAsync($"{identyOfCertificate[2..]}?lang=ru");
            var bodyOfPage = await page.Content.ReadAsStringAsync();
            var root = JsonConvert.DeserializeObject<RootCertificate>(bodyOfPage);

            var certificate = new Certificate();

            certificate.Link = new List<string>() { link.AbsoluteUri };
            certificate.Author = "НЛМК";
            certificate.Number = root.Elements[1].Elements[0].Value.ToString()[13..19];
            certificate.Date = DateTime.SpecifyKind(DateTime.Parse(root.Elements[1].Elements[0].Value.ToString()[22..33]), DateTimeKind.Utc);
            certificate.Product = GetProduct(root);
            certificate.ShipmentShop = root.Elements[5].Elements[0].Value.ToString();
            certificate.WagonNumber = root.Elements[5].Elements[1].Value.ToString();
            certificate.OrderNumber = root.Elements[1].Elements[0].Value.ToString()[43..];
            certificate.TypeOfRollingStock = root.Elements[5].Elements[2].Value.ToString();
            certificate.Notes = root.Elements[5].Elements[3].Value.ToString();
            certificate.Packages = new List<Package>();

            for (var i = 0; i < root.Elements[4].Elements[0].Elements[0].Body.Count; i++)
            {
                certificate.Packages.Add(GetPackage(root, i));
            }

            return certificate;
        }

        private static Product GetProduct(RootCertificate root)
        {
            var product = new Product();

            product.Name = root.Elements[1].Elements[1].Value.ToString();

            return product;
        }

        private static Package GetPackage(RootCertificate root, int id)
        {
            var package = new Package();

            var quantityId = root.Elements[4].Elements[0].Elements[0].Head.IndexOf("Количество");
            var gostId = root.Elements[4].Elements[0].Elements[0].Head.IndexOf("ГОСТ, ТУ");
            var surfaceQualityId = root.Elements[4].Elements[2].Elements[0].Head.IndexOf("Группа отделки");
            var categoryOfDrawingId = root.Elements[4].Elements[2].Elements[0].Head.IndexOf("Категория вытяжки");
            var trimOfEdgeId = root.Elements[4].Elements[2].Elements[0].Head.IndexOf("Характер кромки");
            var temporalResistanceId = -1;
            var temporalResistanceList = root.Elements[4].Elements[2].Elements[0].Head.ToList();
            for (var i = 0; i < temporalResistanceList.Count; i++)
            {
                if (temporalResistanceList[i].Contains("соп"))
                    temporalResistanceId = i;
            }
            var elongationId = root.Elements[4].Elements[2].Elements[0].Head.IndexOf("Относит.удл., %");
            var sphericalHoleDepthList = root.Elements[4].Elements[2].Elements[0].Head.ToList();
            int sphericalHoleDepthId = -1;
            for (var i = 0; i < sphericalHoleDepthList.Count; i++)
            {
                if (sphericalHoleDepthList[i].Contains("Глубина сфер. лунки"))
                    sphericalHoleDepthId = i;
            }
            var microBallCemId = root.Elements[4].Elements[2].Elements[0].Head.IndexOf("Мик-ра Балл цем.");
            var r90Id = root.Elements[4].Elements[2].Elements[0].Head.IndexOf("Коэф. пл. ан., R90");
            var n90Id = root.Elements[4].Elements[2].Elements[0].Head.IndexOf("Показ. деф. упр., n90");
            var koafNavodoragId = root.Elements[4].Elements[2].Elements[0].Head.IndexOf("Коэф. наводораж., %");
            var gradeId = root.Elements[4].Elements[0].Elements[0].Head.IndexOf("Марка стали");

            package.Link = $"https://doc.nlmk.shop/p?q={root.Elements[4].Elements[0].Elements[0].Body[id].Meta.ProductCode}";
            package.NamberConsignmentPackage = root.Elements[4].Elements[1].Elements[0].Body[id].Tr[1];
            package.Heat = root.Elements[4].Elements[1].Elements[0].Body[id].Tr[2];
            package.Batch = root.Elements[4].Elements[2].Elements[0].Body[id].Tr[0];
            package.Grade = gradeId != -1 ? root.Elements[4].Elements[0].Elements[0].Body[id].Tr[6] : "-";
            package.Size = GetSize(root, id);
            package.Quantity = int.Parse(root.Elements[4].Elements[0].Elements[0].Body[id].Tr[quantityId]);
            package.Variety = root.Elements[4].Elements[0].Elements[0].Body[id].Tr[3];
            package.Gost = root.Elements[4].Elements[0].Elements[0].Body[id].Tr[gostId];
            package.Weight = GetWeight(root, id);
            package.SurfaceQuality = surfaceQualityId != -1 ? root.Elements[4].Elements[2].Elements[0].Body[id].Tr[3] : null;
            package.CategoryOfDrawing = root.Elements[4].Elements[2].Elements[0].Body[id].Tr[categoryOfDrawingId];
            package.TrimOfEdge = root.Elements[4].Elements[2].Elements[0].Body[id].Tr[trimOfEdgeId];
            package.ChemicalComposition = GetChemicalComposition(root, id);
            package.TemporalResistance = temporalResistanceId != -1 ? double.Parse(root.Elements[4].Elements[2].Elements[0].Body[id].Tr[temporalResistanceId] + ".0", CultureInfo.InvariantCulture) : null;
            package.Elongation = double.Parse(root.Elements[4].Elements[2].Elements[0].Body[id].Tr[elongationId] + ".0", CultureInfo.InvariantCulture);
            double sphericalHoleDepth = -1;
            double.TryParse(root.Elements[4].Elements[2].Elements[0].Body[id].Tr[sphericalHoleDepthId], NumberStyles.Any, CultureInfo.CurrentCulture, out sphericalHoleDepth);
            package.SphericalHoleDepth = sphericalHoleDepth == 0 ? null : sphericalHoleDepth;
            package.MicroBallCem = microBallCemId != -1 ? double.Parse(root.Elements[4].Elements[2].Elements[0].Body[id].Tr[microBallCemId], CultureInfo.InvariantCulture) : null;
            package.R90 = r90Id != -1 ? double.Parse(root.Elements[4].Elements[2].Elements[0].Body[id].Tr[r90Id], CultureInfo.InvariantCulture) : null;
            package.N90 = n90Id != -1 ? double.Parse(root.Elements[4].Elements[2].Elements[0].Body[id].Tr[n90Id], CultureInfo.InvariantCulture) : null;
            package.KoafNavodorag = koafNavodoragId != -1 ? double.Parse(root.Elements[4].Elements[2].Elements[0].Body[id].Tr[koafNavodoragId], CultureInfo.InvariantCulture) : null;

            return package;
        }

        private static Size GetSize(RootCertificate root, int id)
        {
            var size = new Size();

            var sizeId = root.Elements[4].Elements[0].Elements[0].Head.IndexOf("Размеры, мм");

            size.Width = double.Parse(root.Elements[4].Elements[0].Elements[0].Body[id].Tr[sizeId][7..], CultureInfo.InvariantCulture);
            size.Thickness = double.Parse(root.Elements[4].Elements[0].Elements[0].Body[id].Tr[sizeId][..4], CultureInfo.InvariantCulture);

            return size;
        }

        private static Weight GetWeight(RootCertificate root, int id)
        {
            var weight = new Weight();

            var grossId = root.Elements[4].Elements[0].Elements[0].Head.IndexOf("Масса брутто, Т");
            var netId = root.Elements[4].Elements[0].Elements[0].Head.IndexOf("Масса нетто, Т");

            weight.Gross = double.Parse(root.Elements[4].Elements[0].Elements[0].Body[id].Tr[grossId], CultureInfo.InvariantCulture) * 1000;
            weight.Net = double.Parse(root.Elements[4].Elements[0].Elements[0].Body[id].Tr[netId], CultureInfo.InvariantCulture) * 1000;

            return weight;
        }

        private static ChemicalComposition GetChemicalComposition(RootCertificate root, int id)
        {
            var chemical = new ChemicalComposition();

            var cId = root.Elements[4].Elements[1].Elements[0].Head.IndexOf("C");
            var siId = root.Elements[4].Elements[1].Elements[0].Head.IndexOf("Si");
            var mnId = root.Elements[4].Elements[1].Elements[0].Head.IndexOf("Mn");
            var sId = root.Elements[4].Elements[1].Elements[0].Head.IndexOf("S");
            var pId = root.Elements[4].Elements[1].Elements[0].Head.IndexOf("P");
            var alId = root.Elements[4].Elements[1].Elements[0].Head.IndexOf("Al");
            var crId = root.Elements[4].Elements[1].Elements[0].Head.IndexOf("Cr");
            var niId = root.Elements[4].Elements[1].Elements[0].Head.IndexOf("Ni");
            var cuId = root.Elements[4].Elements[1].Elements[0].Head.IndexOf("Cu");
            var n2Id = root.Elements[4].Elements[1].Elements[0].Head.IndexOf("N2");

            chemical.C = double.Parse("0" + root.Elements[4].Elements[1].Elements[0].Body[id].Tr[cId], CultureInfo.InvariantCulture);
            chemical.Si = double.Parse("0" + root.Elements[4].Elements[1].Elements[0].Body[id].Tr[siId], CultureInfo.InvariantCulture);
            chemical.Mn = double.Parse("0" + root.Elements[4].Elements[1].Elements[0].Body[id].Tr[mnId], CultureInfo.InvariantCulture);
            chemical.S = double.Parse("0" + root.Elements[4].Elements[1].Elements[0].Body[id].Tr[sId], CultureInfo.InvariantCulture);
            chemical.P = double.Parse("0" + root.Elements[4].Elements[1].Elements[0].Body[id].Tr[pId], CultureInfo.InvariantCulture);
            chemical.Al = double.Parse("0" + root.Elements[4].Elements[1].Elements[0].Body[id].Tr[alId], CultureInfo.InvariantCulture);
            chemical.Cr = double.Parse("0" + root.Elements[4].Elements[1].Elements[0].Body[id].Tr[crId], CultureInfo.InvariantCulture);
            chemical.Ni = double.Parse("0" + root.Elements[4].Elements[1].Elements[0].Body[id].Tr[niId], CultureInfo.InvariantCulture);
            chemical.Cu = double.Parse("0" + root.Elements[4].Elements[1].Elements[0].Body[id].Tr[cuId], CultureInfo.InvariantCulture);
            //chemical.Ti = double.Parse("0" + root.Elements[4].Elements[1].Elements[0].Body[id].Tr[12], CultureInfo.InvariantCulture);
            //chemical.V = double.Parse("0" + root.Elements[4].Elements[1].Elements[0].Body[id].Tr[14], CultureInfo.InvariantCulture);
            chemical.N2 = double.Parse("0" + root.Elements[4].Elements[1].Elements[0].Body[id].Tr[n2Id], CultureInfo.InvariantCulture);

            return chemical;
        }
    }
}

using AngleSharp.Html.Dom;
using AngleSharp.Html.Parser;
using Newtonsoft.Json;
using SupplyIO.SupplyIO.Services.Models.CertificateModel;
using System.Globalization;

namespace SupplyIO.SupplyIO.Services.Logic.ChainOfHosts
{
    public class MetinvestHandler : Handler
    {
        private readonly string context = "metinvest";

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
            using var _httpClient = new HttpClient();

            var page = await _httpClient.GetAsync(link);
            var pageContext = await page.Content.ReadAsStringAsync();

            var parser = new HtmlParser();
            var document = parser.ParseDocument(pageContext);

            var certificate = new Certificate();

            certificate.Link = new List<string>() { link.AbsoluteUri };
            certificate.Number = GetElemets(document, "name_doc").FirstOrDefault()[47..58];
            certificate.Date = DateTime.SpecifyKind(DateTime.Parse(GetElemets(document, "name_doc").FirstOrDefault()[68..78]), DateTimeKind.Utc);
            certificate.Author = GetElemets(document, "title_ru_en").FirstOrDefault();
            certificate.AuthorAddress = GetElemets(document, "adres_ru_en").FirstOrDefault();
            certificate.Fax = GetElemets(document, "fax").FirstOrDefault();
            certificate.Recipient = GetElemets(document, "npot_ru_en").FirstOrDefault();
            certificate.RecipientCountry = GetElemets(document, "cntr_ru_en").FirstOrDefault();
            certificate.Contract = GetElemets(document, "nrvr_ru_en").FirstOrDefault();
            certificate.Product = GetProduct(document);
            certificate.WagonNumber = GetElemets(document, "ntr_ru_en").FirstOrDefault();
            certificate.OrderNumber = GetElemets(document, "nsnz_ru_en").FirstOrDefault();
            certificate.TypeOfPackaging = GetElemets(document, "vgrm").FirstOrDefault();
            certificate.PlaceNumber = GetElemets(document, "kol_mest").FirstOrDefault();
            certificate.Notes = GetElemets(document, "prim").FirstOrDefault();

            certificate.Packages = new List<Package>();

            for (var i = 0; i < GetElemets(document, "nssr_03").Count; i++)
                certificate.Packages.Add(GetPackage(document, i));

            var json = JsonConvert.SerializeObject(certificate);

            return certificate;
        }

        private static Product GetProduct(IHtmlDocument document)
        {
            var product = new Product();

            var name = GetElemets(document, "prod_ru_en").FirstOrDefault()[12..];
            var code = GetElemets(document, "prod_ru_en").FirstOrDefault()[..11];

            product.Name = name == "" ? null : name;
            product.Code = code == "" ? null : code;

            return product;
        }

        private static Package GetPackage(IHtmlDocument document, int id)
        {
            var package = new Package();

            var namberConsignmentPackage = GetElemets(document, "nssr_03")[id];
            var heat = GetElemets(document, "shpl_03")[id];
            var batch = GetElemets(document, "nprt_03")[id];
            var grade = GetElemets(document, "nmms")[id];
            var category = GetElemets(document, "tt08")[id];
            var strengthGroup = GetElemets(document, "tt01")[id];
            var quantity = GetElemets(document, "kwos")[id];
            var variety = GetElemets(document, "sort")[id];
            var customerItemNumber = GetElemets(document, "npoz")[id];
            var treatment = GetElemets(document, "tt18_27")[id];
            var groupCode = GetElemets(document, "kkgr")[id];
            var patternCutting = GetElemets(document, "upmn")[id];
            var surfaceQuality = GetElemets(document, "tt11_12_22")[id];
            var rollingAccuracy = GetElemets(document, "tt02_03_04_05")[id];
            var categoryOfDrawing = GetElemets(document, "tt09")[id];
            var stateOfMatirial = GetElemets(document, "tt10")[id];
            var roughness = GetElemets(document, "tt13_14")[id];
            var flatness = GetElemets(document, "tt06")[id];
            var trimOfEdge = GetElemets(document, "tt07")[id];
            var weldability = GetElemets(document, "tt30")[id];
            var orderFeatures = GetElemets(document, "other")[id];
            var sampleLocation = GetElemets(document, "place")[id];
            var directOfTestPicses = GetElemets(document, "direction")[id];
            var temporalResistance = GetElemets(document, "prpr")[id];
            var yieldPoint = GetElemets(document, "prtk")[id];
            var elongation = GetElemets(document, "ud")[id];
            var bend = GetElemets(document, "holz")[id];
            var hardness = GetElemets(document, "kdtw")[id];
            var rockwell = GetElemets(document, "twrr")[id];
            var brinel = GetElemets(document, "twrb")[id];
            var eriksen = GetElemets(document, "erik")[id];
            var grainSize = GetElemets(document, "zern")[id];
            var decarburiization = GetElemets(document, "gobs")[id];
            var cementite = GetElemets(document, "cemn")[id];
            var banding = GetElemets(document, "pols")[id];
            var corrosion = GetElemets(document, "krrz")[id];
            var testingMethod = GetElemets(document, "meti")[id];
            var unitTemporaryResistance = GetElemets(document, "prpr_measure")[id];
            var unitYieldStrength = GetElemets(document, "prtk_measure")[id];
            var notes = GetElemets(document, "zmtk")[id];

            package.NamberConsignmentPackage = namberConsignmentPackage == "" ? null : namberConsignmentPackage;
            package.Heat = heat == "" ? null : heat;
            package.Batch = batch == "" ? null : batch;
            package.Grade = grade == "" ? null : grade;
            package.Category = category == "" ? null : category;
            package.StrengthGroup = strengthGroup == "" ? null : strengthGroup;
            package.Size = GetSize(document, id);
            package.Quantity = quantity == "" ? null : int.Parse(quantity);
            package.Variety = variety == "" ? null : variety;
            package.Weight = GetWeight(document, id);
            package.CustomerItemNumber = customerItemNumber == "" ? null : int.Parse(customerItemNumber);
            package.Treatment = treatment == "" ? null : treatment;
            package.GroupCode = groupCode == "" ? null : int.Parse(groupCode);
            package.PattemCutting = patternCutting == "" ? null : patternCutting;
            package.SurfaceQuality = surfaceQuality == "" ? null : surfaceQuality;
            package.RollingAccuracy = rollingAccuracy == "" ? null : rollingAccuracy;
            package.CategoryOfDrawing = categoryOfDrawing == "" ? null : categoryOfDrawing;
            package.StateOfMatirial = stateOfMatirial == "" ? null : stateOfMatirial;
            package.Roughness = roughness == "" ? null : roughness;
            package.Flatness = flatness == "" ? null : flatness;
            package.TrimOfEdge = trimOfEdge == "" ? null : trimOfEdge;
            package.Weldability = weldability == "" ? null : weldability;
            package.OrderFeatures = orderFeatures == "" ? null : orderFeatures;
            package.ChemicalComposition = GetChemicalComposition(document, id);
            package.SampleLocation = sampleLocation == "" ? null : sampleLocation;
            package.DirectOfTestPicses = directOfTestPicses == "" ? null : directOfTestPicses;
            package.TemporalResistance = temporalResistance == "" ? null : double.Parse(temporalResistance, CultureInfo.InvariantCulture);
            package.YieldPoint = yieldPoint == "" ? null : yieldPoint;
            package.Elongation = elongation == "" ? null : double.Parse(elongation, CultureInfo.InvariantCulture);
            package.Bend = bend == "" ? null : bend;
            package.Hardness = hardness == "" ? null : hardness;
            package.Rockwell = rockwell == "" ? null : rockwell;
            package.Brinel = brinel == "" ? null : brinel;
            package.Eriksen = eriksen == "" ? null : eriksen;
            package.ImpactStrength = GetImpactStrength(document, id);
            package.GrainSize = grainSize == "" ? null : grainSize;
            package.Decarburiization = decarburiization == "" ? null : decarburiization;
            package.Cementite = cementite == "" ? null : cementite;
            package.Banding = banding == "" ? null : banding;
            package.Corrosion = corrosion == "" ? null : corrosion;
            package.TestingMethod = testingMethod == "" ? null : testingMethod;
            package.UnitTemporaryResistance = unitTemporaryResistance == "" ? null : unitTemporaryResistance;
            package.UnitYieldStrength = unitYieldStrength == "" ? null : unitYieldStrength;
            package.Notes = notes == "" ? null : notes;

            return package;
        }

        private static Weight GetWeight(IHtmlDocument document, int id)
        {
            var weight = new Weight();

            var gross = GetElemets(document, "vsbr")[id];
            var gross2 = GetElemets(document, "vsbr2")[id];
            var net = GetElemets(document, "vsst")[id];

            weight.Gross = gross == "" ? null : double.Parse(gross, CultureInfo.InvariantCulture);
            weight.Gross2 = gross2 == "" ? null : double.Parse(gross2, CultureInfo.InvariantCulture);
            weight.Net = net == "" ? null : double.Parse(net, CultureInfo.InvariantCulture);

            return weight;
        }

        private static Size GetSize(IHtmlDocument document, int id)
        {
            var size = new Size();

            var thickness = GetElemets(document, "tlmn_tlmx")[id];
            var width = GetElemets(document, "shmn_shmx")[id];
            var length = GetElemets(document, "dlmn_dlmx")[id];

            size.Thickness = thickness == "" ? null : double.Parse(thickness, CultureInfo.InvariantCulture);
            size.Width = width == "" ? null : double.Parse(width, CultureInfo.InvariantCulture);
            size.Length = length == "" ? null : length;

            return size;
        }

        private static ChemicalComposition GetChemicalComposition(IHtmlDocument document, int id)
        {
            var chemical = new ChemicalComposition();

            var c = GetElemets(document, "c")[id];
            var mn = GetElemets(document, "mn")[id];
            var si = GetElemets(document, "si")[id];
            var s = GetElemets(document, "s_him")[id];
            var p = GetElemets(document, "p").TakeLast(6).ToList()[id];
            var cr = GetElemets(document, "cr")[id];
            var ni = GetElemets(document, "ni")[id];
            var cu = GetElemets(document, "cu")[id];
            var as1 = GetElemets(document, "as")[id];
            var n2 = GetElemets(document, "n2")[id];
            var al = GetElemets(document, "al")[id];
            var ti = GetElemets(document, "ti")[id];
            var mo = GetElemets(document, "mo")[id];
            var w = GetElemets(document, "w")[id];
            var v = GetElemets(document, "v")[id];
            var aln2 = GetElemets(document, "al_n2")[id];
            var cev = GetElemets(document, "cev")[id];
            var notes = GetElemets(document, "him_sost_prim")[id];

            chemical.C = c == "" ? null : double.Parse(c, CultureInfo.InvariantCulture) / 100;
            chemical.Mn = mn == "" ? null : double.Parse(mn, CultureInfo.InvariantCulture) / 100;
            chemical.Si = si == "" ? null : double.Parse(si, CultureInfo.InvariantCulture) / 100;
            chemical.S = s == "" ? null : double.Parse(s, CultureInfo.InvariantCulture) / 1000;
            chemical.P = p == "" ? null : double.Parse(p, CultureInfo.InvariantCulture) / 1000;
            chemical.Cr = cr == "" ? null : double.Parse(cr, CultureInfo.InvariantCulture) / 100;
            chemical.Ni = ni == "" ? null : double.Parse(ni, CultureInfo.InvariantCulture) / 100;
            chemical.Cu = cu == "" ? null : double.Parse(cu, CultureInfo.InvariantCulture) / 100;
            chemical.As = as1 == "" ? null : double.Parse(as1, CultureInfo.InvariantCulture) / 100;
            chemical.N2 = n2 == "" ? null : double.Parse(n2, CultureInfo.InvariantCulture) / 1000;
            chemical.Al = al == "" ? null : double.Parse(al, CultureInfo.InvariantCulture) / 100;
            chemical.Ti = ti == "" ? null : double.Parse(ti, CultureInfo.InvariantCulture) / 100;
            chemical.Mo = mo == "" ? null : double.Parse(mo, CultureInfo.InvariantCulture) / 1000;
            chemical.W = w == "" ? null : double.Parse(w, CultureInfo.InvariantCulture) / 100;
            chemical.V = v == "" ? null : double.Parse(v, CultureInfo.InvariantCulture) / 1000;
            chemical.AlWithN2 = aln2 == "" ? null : double.Parse(aln2, CultureInfo.InvariantCulture);
            chemical.Cev = cev == "" ? null : double.Parse(cev, CultureInfo.InvariantCulture);
            chemical.Notes = notes == "" ? null : notes;

            return chemical;
        }

        private static ImpactStrength GetImpactStrength(IHtmlDocument document, int id)
        {
            var strength = new ImpactStrength();

            var kcu = GetElemets(document, "tem1")[id];
            var kcu1 = GetElemets(document, "rs11")[id];
            var kcv = GetElemets(document, "tem4")[id];
            var kcv1 = GetElemets(document, "rs41")[id];
            var afterMechAgeing = GetElemets(document, "tem3")[id];
            var afterMechAgeing1 = GetElemets(document, "rs31")[id];

            strength.KCU = kcu == "" ? null : double.Parse(kcu, CultureInfo.InvariantCulture);
            strength.KCU1 = kcu1 == "" ? null : double.Parse(kcu1, CultureInfo.InvariantCulture);
            strength.KCV = kcv == "" ? null : double.Parse(kcv, CultureInfo.InvariantCulture);
            strength.KCV1 = kcv1 == "" ? null : double.Parse(kcv1, CultureInfo.InvariantCulture);
            strength.AfterMechAgeing = afterMechAgeing == "" ? null : double.Parse(afterMechAgeing, CultureInfo.InvariantCulture);
            strength.AfterMechAgeing1 = afterMechAgeing1 == "" ? null : double.Parse(afterMechAgeing1, CultureInfo.InvariantCulture);

            return strength;
        }

        private static List<string> GetElemets(IHtmlDocument doc, string tag)
   => doc.QuerySelectorAll(tag).Select(element => element.TextContent).ToList();
    }
}

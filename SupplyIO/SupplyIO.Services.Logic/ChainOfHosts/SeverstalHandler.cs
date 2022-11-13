using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using AngleSharp.Html.Parser;
using SupplyIO.SupplyIO.Services.Models.CertificateModel;
using System.Globalization;
using System.Net;

namespace SupplyIO.SupplyIO.Services.Logic.ChainOfHosts
{
    public class SeverstalHandler : Handler
    {
        private readonly string context = "severstal";
        private int numberOfPackages = -1;
        private Dictionary<string, List<int>> indexDictionary;
        private Dictionary<string, List<int>> indexesOFEdgeTable;
        private Dictionary<string, List<int>> indexesOFElongationTable;


        public override async Task<Certificate> HandleRequestAsync(Uri link)
        {
            if (link.AbsoluteUri.Contains(context))
                return await GetCertificateAsync(link);
            
            else if (Successor != null)
                return await Successor.HandleRequestAsync(link);

            return null;
        }

        private async Task<Certificate> GetCertificateAsync(Uri link)
        {
            var httpClient = new HttpClient();

            var page = await httpClient.GetAsync(link);
            var bodyOfPage = await page.Content.ReadAsStringAsync();

            var parser = new HtmlParser();
            var document = parser.ParseDocument(bodyOfPage);

            numberOfPackages = document.QuerySelectorAll("tbody")
                                          .Where(element => element.Text()
                                                                   .Contains("№ П/П"))
                                          .Last()
                                          .GetElementsByTagName("tr")
                                          .Count() - 1;

            var certificate = new Certificate();

            certificate.Author = "СеверСталь";

            certificate.Link = new List<string>() { link.AbsoluteUri };

            certificate.Number = document.QuerySelectorAll("tr")
                                         .Where(element => element.Text()
                                                                  .Contains("Сертификат №:"))
                                         .Last()
                                         .GetElementsByTagName("td")
                                         .Select(element => element.Text())
                                         .ToList()[1]
                                         .Trim();

            var a = document.QuerySelectorAll("tr")
                                                      .Where(element => element.Text()
                                                                               .Contains("Сертификат №:"))
                                                      .Last()
                                                      .GetElementsByTagName("td")
                                                      .Select(element => element.Text())
                                                      .ToList()[3]
                                                      .Trim();

            certificate.Date = DateTime.ParseExact(document.QuerySelectorAll("tr")
                                                      .Where(element => element.Text()
                                                                               .Contains("Сертификат №:"))
                                                      .Last()
                                                      .GetElementsByTagName("td")
                                                      .Select(element => element.Text())
                                                      .ToList()[3]
                                                      .Trim(), "dd.MM.yyyy", null);

            certificate.Recipient = document.QuerySelectorAll("tr")
                                            .Where(element => element.Text()
                                                                     .Contains("ГРУЗОПОЛУЧАТЕЛЬ, АДРЕС"))
                                            .Last()
                                            .GetElementsByTagName("td")
                                            .Select(element => element.Text())
                                            .ToList()[1]
                                            .Trim();

            certificate.SpecificationNumber = document.QuerySelectorAll("tr")
                                                      .Where(element => element.Text()
                                                                               .Contains("СПЕЦИФИКАЦИЯ №"))
                                                      .Last()
                                                      .GetElementsByTagName("td")
                                                      .Select(element => element.Text())
                                                      .ToList()[1]
                                                      .Trim();

            certificate.RecipientCountry = document.QuerySelectorAll("tr")
                                                   .Where(element => element.Text()
                                                                            .Contains("СТРАНА НАЗНАЧЕНИЯ"))
                                                   .Last()
                                                   .GetElementsByTagName("td")
                                                   .Select(element => element.Text())
                                                   .ToList()[1]
                                                   .Trim();

            certificate.Gosts = document.QuerySelectorAll("td")
                                        .Where(element => element.Text()
                                                                 .Contains("ГОСТ"))
                                        .Select(element => element.Text())
                                        .Last()
                                        .Trim();

            certificate.TypeOfPackaging = document.QuerySelectorAll("tbody")
                                                  .Where(element => element.Text()
                                                                           .Contains("НАИМЕНОВАНИЕ И КОД ТОВАРА"))
                                                  .Last()
                                                  .Children
                                                  .Last()
                                                  .Children
                                                  .First()
                                                  .Children
                                                  .First()
                                                  .Children
                                                  .First()
                                                  .Children
                                                  .First()
                                                  .Text()
                                                  .Trim();

            certificate.Notes = document.QuerySelectorAll("tr")
                                        .Where(element => element.Text()
                                                                 .Contains("Примечания"))
                                        .Last()
                                        .GetElementsByTagName("td")
                                        .Last()
                                        .Text()
                                        .Trim();

            certificate.Product = GetProduct(document);

            certificate.Packages = new List<Package>();

            indexDictionary = GetIndexesOfChemicals(document);
            indexesOFEdgeTable = GetIndexesOfPackageTable(document);
            indexesOFElongationTable = GetIndexesOfElongationTable(document);

            for (var i = 0; i < numberOfPackages; i++)
                certificate.Packages.Add(GetPackage(document, i));

            return certificate;
        }

        private static Product GetProduct(IHtmlDocument document)
        {
            var product = new Product();

            var productInfo = document.QuerySelectorAll("tbody")
                                      .Where(element => element.Text()
                                                               .Contains("НАИМЕНОВАНИЕ И КОД ТОВАРА"))
                                      .Last()
                                      .GetElementsByTagName("tr")
                                      .ToArray()[1]
                                      .GetElementsByTagName("td")
                                      .Select(element => element.Text())
                                      .First()
                                      .Trim();

            product.Name = string.IsNullOrWhiteSpace(productInfo) ? null : productInfo;

            return product;
        }

        private Package GetPackage(IHtmlDocument document, int id)
        {
            var package = new Package();

            var packageTable = document.QuerySelectorAll("tbody")
                                       .Where(element => element.Text()
                                                                .Contains("Номер партии"))
                                       .ToArray()[1]
                                       .GetElementsByTagName("tr")
                                       .ToArray()[id + 1]
                                       .GetElementsByTagName("td")
                                       .Select(element => element.Text())
                                       .ToArray();

            var orderPosition = string.IsNullOrWhiteSpace(packageTable[7]) ? null : packageTable[7];
            var count = string.IsNullOrWhiteSpace(packageTable[29]) ? null : packageTable[29];

            package.NamberConsignmentPackage = string.IsNullOrWhiteSpace(packageTable[4]) ? null : packageTable[4];
            package.Heat = string.IsNullOrWhiteSpace(packageTable[5]) ? null : packageTable[5];
            package.Batch = string.IsNullOrWhiteSpace(packageTable[6]) ? null : packageTable[6];
            package.OrderPosition = orderPosition is null ? null : int.Parse(orderPosition);
            package.Quantity = count is null ? null : int.Parse(count);
            package.NumberOfClientMaterial = string.IsNullOrWhiteSpace(packageTable[20]) ? null : packageTable[20];
            package.Category = string.IsNullOrWhiteSpace(packageTable[25]) ? null : packageTable[25];
            package.SerialNumber = string.IsNullOrWhiteSpace(packageTable[27]) ? null : packageTable[27];
            package.Grade = string.IsNullOrWhiteSpace(packageTable[8]) ? null : packageTable[8];
            package.Profile = string.IsNullOrWhiteSpace(packageTable[30]) ? null : packageTable[30];
            package.Barcode = string.IsNullOrWhiteSpace(packageTable[31]) ? null : packageTable[31];
            package.Flatness = string.IsNullOrWhiteSpace(packageTable[35]) ? null : packageTable[35];

            var packageNamingEdgeTable = document.QuerySelectorAll("tbody")
                            .Where(element => element.Text()
                                                     .Contains("Кромка"))
                            .Last()
                            .GetElementsByTagName("tr")
                            .Select(element => element.GetElementsByTagName("td")
                                                      .Select(element => element.Text())
                                                      .ToArray())
                            .First()
                            .ToArray();

            var packageEdgeTable = document.QuerySelectorAll("tbody")
                            .Where(element => element.Text()
                                                     .Contains("Кромка"))
                            .Last()
                            .GetElementsByTagName("tr")
                            .Select(element => element.GetElementsByTagName("td")
                                                      .Select(element => element.Text())
                                                      .ToArray())
                            .Skip(1)
                            .ToArray();

            var indexOfPackageString = indexesOFEdgeTable.Where(pair => pair.Value.Contains(id))
                                                      .First().Key;

            var indexOfPackageElement = packageEdgeTable.Select((el, i) => new { el = el, i = i })
                                                        .First(el => el.el[0].Contains(indexOfPackageString)).i;

            var edgeInd = packageNamingEdgeTable.Select((el, i) => new { el, i })
                                                .FirstOrDefault(el => el.el.Contains("Edge"));

            var strengthGroupInd = packageNamingEdgeTable.Select((el, i) => new { el, i })
                                                         .FirstOrDefault(el => el.el.Contains("Strength Class"));

            var surfaceQuality = packageNamingEdgeTable.Select((el, i) => new { el, i })
                                                       .FirstOrDefault(el => el.el.Contains("Surface Group"));

            var categoryOfDrawing = packageNamingEdgeTable.Select((el, i) => new { el, i })
                                                          .FirstOrDefault(el => el.el.Contains("Rollout Category"));


            package.TrimOfEdge = edgeInd is null ? null : packageEdgeTable[indexOfPackageElement][edgeInd.i];
            package.StrengthGroup = strengthGroupInd is null ? null : packageEdgeTable[indexOfPackageElement][strengthGroupInd.i];
            package.SurfaceQuality = surfaceQuality is null ? null : packageEdgeTable[indexOfPackageElement][surfaceQuality.i];
            package.CategoryOfDrawing = categoryOfDrawing is null ? null : packageEdgeTable[indexOfPackageElement][categoryOfDrawing.i];

            var packageNamingElongationTable = document.QuerySelectorAll("tbody")
                            .Where(element => element.Text()
                                                     .Contains("Elongation"))
                            .Last()
                            .GetElementsByTagName("tr")
                            .Select(element => element.GetElementsByTagName("td")
                                                      .Select(element => element.Text())
                                                      .ToArray())
                            .First()
                            .ToArray();

            var packageElongationTable = document.QuerySelectorAll("tbody")
                            .Where(element => element.Text()
                                                     .Contains("Elongation"))
                            .Last()
                            .GetElementsByTagName("tr")
                            .Select(element => element.GetElementsByTagName("td")
                                                      .Select(element => element.Text())
                                                      .ToArray())
                            .Skip(1)
                            .ToArray();

            var indexOfPackageElongationString = indexesOFElongationTable.Where(pair => pair.Value.Contains(id))
                                                                         .First().Key;

            var indexOfPackageElongationElement = packageElongationTable.Select((el, i) => new { el = el, i = i })
                                                                        .First(el => el.el[0].Contains(indexOfPackageElongationString)).i;

            var tensilePointInd = packageNamingElongationTable.Select((el, i) => new { el, i })
                                                              .FirstOrDefault(el => el.el.Contains("Tensile"));

            var elongationInd = packageNamingElongationTable.Select((el, i) => new { el, i })
                                                            .FirstOrDefault(el => el.el.Contains("Elongation"));

            var bendInd = packageNamingElongationTable.Select((el, i) => new { el, i })
                                                      .FirstOrDefault(el => el.el.Contains("Cold bend"));

            var grainSizeInd = packageNamingElongationTable.Select((el, i) => new { el, i })
                                                           .FirstOrDefault(el => el.el.Contains("Grain"));

            var sphericalHoleDepthInd = packageNamingElongationTable.Select((el, i) => new { el, i })
                                                                 .FirstOrDefault(el => el.el.Contains("Dimple depth"));

            package.TensilePoint = tensilePointInd is null ? null : packageElongationTable[indexOfPackageElongationElement][tensilePointInd.i];
            package.Elongation = elongationInd is null ? null : double.Parse(packageElongationTable[indexOfPackageElongationElement][elongationInd.i], CultureInfo.InvariantCulture);
            package.Bend = bendInd is null ? null : packageElongationTable[indexOfPackageElongationElement][bendInd.i];
            package.GrainSize = grainSizeInd is null ? null : packageElongationTable[indexOfPackageElongationElement][grainSizeInd.i];
            package.SphericalHoleDepth = sphericalHoleDepthInd is null ? null : double.Parse(packageElongationTable[indexOfPackageElongationElement][sphericalHoleDepthInd.i], CultureInfo.InvariantCulture);

            package.ChemicalComposition = GetChemicalComposition(document, id);
            package.Size = GetSize(document, id);
            package.Weight = GetWeight(document, id);

            return package;
        }

        private ChemicalComposition GetChemicalComposition(IHtmlDocument document, int id = 0)
        {
            var chemical = new ChemicalComposition();
            
            var chemicalsNamingTable = document.QuerySelectorAll("tbody")
                     .Where(element => element.Text()
                                              .Contains("Химический состав"))
                     .Last()
                     .GetElementsByTagName("tr")
                     .Where((ar, i) => i % 2 == 1)
                     .Select(element => element.GetElementsByTagName("td")
                                               .Select(element => element.Text())
                                               .ToArray())
                     .ToArray();

            var chemicalsTable = document.QuerySelectorAll("tbody")
                                 .Where(element => element.Text()
                                                          .Contains("Химический состав"))
                                 .Last()
                                 .GetElementsByTagName("tr")
                                 .Where((ar, i) => i % 2 == 0)
                                 .Skip(1)
                                 .Select(element => element.GetElementsByTagName("td")
                                                           .Select(element => element.Text())
                                                           .ToArray())
                                 .ToArray();

            var indexOfPackageString = indexDictionary.Where(pair => pair.Value.Contains(id))
                                                      .First().Key;

            var indexOfPackageElement = chemicalsNamingTable.Select((el, i) => new { el = el, i = i })
                                                            .First(el => el.el.Contains(indexOfPackageString)).i;

            var CInd = chemicalsNamingTable[indexOfPackageElement].Select((el, i) => new { el, i })
                                                                   .FirstOrDefault(el => el.el.Contains("C %"));

            var SiInd = chemicalsNamingTable[indexOfPackageElement].Select((el, i) => new { el, i })
                                                                   .FirstOrDefault(el => el.el.Contains("Si %"));

            var MnInd = chemicalsNamingTable[indexOfPackageElement].Select((el, i) => new { el, i })
                                                                   .FirstOrDefault(el => el.el.Contains("Mn %"));

            var SInd = chemicalsNamingTable[indexOfPackageElement].Select((el, i) => new { el, i })
                                                                  .FirstOrDefault(el => el.el.Contains("S %"));

            var PInd = chemicalsNamingTable[indexOfPackageElement].Select((el, i) => new { el, i })
                                                                  .FirstOrDefault(el => el.el.Contains("P %"));

            var CrInd = chemicalsNamingTable[indexOfPackageElement].Select((el, i) => new { el, i })
                                                                   .FirstOrDefault(el => el.el.Contains("Cr %"));

            var NiInd = chemicalsNamingTable[indexOfPackageElement].Select((el, i) => new { el, i })
                                                                   .FirstOrDefault(el => el.el.Contains("Ni %"));

            var CuInd = chemicalsNamingTable[indexOfPackageElement].Select((el, i) => new { el, i })
                                                                   .FirstOrDefault(el => el.el.Contains("Cu %"));

            var AlInd = chemicalsNamingTable[indexOfPackageElement].Select((el, i) => new { el, i })
                                                                   .FirstOrDefault(el => el.el.Contains("Al %"));

            var NInd = chemicalsNamingTable[indexOfPackageElement].Select((el, i) => new { el, i })
                                                                  .FirstOrDefault(el => el.el.Contains("N %"));

            var AsInd = chemicalsNamingTable[indexOfPackageElement].Select((el, i) => new { el, i })
                                                                   .FirstOrDefault(el => el.el.Contains("As %"));

            chemical.C = CInd is null ? null : double.Parse(chemicalsTable[indexOfPackageElement][CInd.i - 1], CultureInfo.InvariantCulture);
            chemical.Si = SiInd is null ? null : double.Parse(chemicalsTable[indexOfPackageElement][SiInd.i - 1], CultureInfo.InvariantCulture);
            chemical.Mn = MnInd is null ? null : double.Parse(chemicalsTable[indexOfPackageElement][MnInd.i - 1], CultureInfo.InvariantCulture);
            chemical.S = SInd is null ? null : double.Parse(chemicalsTable[indexOfPackageElement][SInd.i - 1], CultureInfo.InvariantCulture);
            chemical.P = PInd is null ? null : double.Parse(chemicalsTable[indexOfPackageElement][PInd.i - 1], CultureInfo.InvariantCulture);
            chemical.Cr = CrInd is null ? null : double.Parse(chemicalsTable[indexOfPackageElement][CrInd.i - 1], CultureInfo.InvariantCulture);
            chemical.Ni = NiInd is null ? null : double.Parse(chemicalsTable[indexOfPackageElement][NiInd.i - 1], CultureInfo.InvariantCulture);
            chemical.Cu = CuInd is null ? null : double.Parse(chemicalsTable[indexOfPackageElement][CuInd.i - 1], CultureInfo.InvariantCulture);
            chemical.Al = AlInd is null ? null : double.Parse(chemicalsTable[indexOfPackageElement][AlInd.i - 1], CultureInfo.InvariantCulture);
            chemical.N2 = NInd is null ? null : double.Parse(chemicalsTable[indexOfPackageElement][NInd.i - 1], CultureInfo.InvariantCulture);
            chemical.As = AsInd is null ? null : double.Parse(chemicalsTable[indexOfPackageElement][AsInd.i - 1], CultureInfo.InvariantCulture);

            return chemical;
        }

        private static Size GetSize(IHtmlDocument document, int id)
        {
            var size = new Size();

            var packageTable = document.QuerySelectorAll("tbody")
                                 .Where(element => element.Text()
                                                          .Contains("Номер партии"))
                                 .ToArray()[1]
                                 .GetElementsByTagName("tr")
                                 .ToArray()[id + 1]
                                 .GetElementsByTagName("td")
                                 .Select(element => element.Text())
                                 .ToArray();

            var lengthMm = string.IsNullOrWhiteSpace(packageTable[23]) ? null : packageTable[23];
            var lengthD = string.IsNullOrWhiteSpace(packageTable[24]) ? null : (double.Parse(packageTable[24], CultureInfo.InvariantCulture) / 0.03937).ToString();

            size.Width = string.IsNullOrWhiteSpace(packageTable[21]) ? null : double.Parse(packageTable[21][8..], CultureInfo.InvariantCulture);
            size.Thickness = string.IsNullOrWhiteSpace(packageTable[21]) ? null : double.Parse(packageTable[21][..5], CultureInfo.InvariantCulture);
            size.Length = lengthMm ?? lengthD;

            return size;
        }

        private static Weight GetWeight(IHtmlDocument document, int id)
        {
            var weight = new Weight();

            var packageTable = document.QuerySelectorAll("tbody")
                                 .Where(element => element.Text()
                                                          .Contains("Номер партии"))
                                 .ToArray()[1]
                                 .GetElementsByTagName("tr")
                                 .ToArray()[id + 1]
                                 .GetElementsByTagName("td")
                                 .Select(element => element.Text())
                                 .ToArray();

            var grossKg = string.IsNullOrWhiteSpace(packageTable[13]) ? null : packageTable[13];
            var grossFnt = string.IsNullOrWhiteSpace(packageTable[17]) ? null : (double.Parse(packageTable[17], CultureInfo.InvariantCulture) / 2.2046).ToString();
            double? gross = grossKg is not null ? double.Parse(grossKg, CultureInfo.InvariantCulture) : (grossFnt is not null ? double.Parse(grossFnt, CultureInfo.InvariantCulture) : null);

            var netKg = string.IsNullOrWhiteSpace(packageTable[15]) ? null : packageTable[15];
            var netFnt = string.IsNullOrWhiteSpace(packageTable[18]) ? null : (double.Parse(packageTable[18], CultureInfo.InvariantCulture) / 2.2046).ToString();
            double? net = netKg is not null ? double.Parse(netKg, CultureInfo.InvariantCulture) : (netFnt is not null ? double.Parse(netFnt, CultureInfo.InvariantCulture) : null);

            weight.Gross = gross;
            weight.Net = net;

            return weight;
        }

        private Dictionary<string, List<int>> GetIndexesOfChemicals(IHtmlDocument document)
        {
            var chemicalTable = document.QuerySelectorAll("tbody")
                                        .Where(element => element.Text()
                                                                 .Contains("Химический состав"))
                                        .Last()
                                        .GetElementsByTagName("tr")
                                        .Where((ar, i) => i % 2 == 1)
                                        .Select(element => element.GetElementsByTagName("td")
                                                                  .Select(element => element.Text())
                                                                  .ToArray())
                                        .ToArray();

            var indexes = chemicalTable.Select(el => el[0]).ToArray();

            var indexDictionary = new Dictionary<string, List<int>>();

            for (int i = 0, j = 0; i < numberOfPackages; i++)
            {
                var isPairIndex = indexes[j].Contains('-');

                if (isPairIndex)
                {
                    var indexesPair = indexes[j].Split('-')
                                                .Select(ind => int.Parse(ind))
                                                .ToArray();

                    if (indexDictionary.ContainsKey(indexes[j]))
                        indexDictionary[indexes[j]].Add(i);
                    else
                        indexDictionary.Add(indexes[j], new List<int>() { i });

                    if (indexesPair[1] == i + 1)
                        j++;
                }
                else
                {
                    indexDictionary.Add(indexes[j], new List<int>() { i });
                    j++;
                }
            }

            return indexDictionary;
        }

        private Dictionary<string, List<int>> GetIndexesOfPackageTable(IHtmlDocument document)
        {
            var packageTable = document.QuerySelectorAll("tbody")
                                        .Where(element => element.Text()
                                                                 .Contains("Кромка"))
                                        .Last()
                                        .GetElementsByTagName("tr")
                                        .Skip(1)
                                        .Select(element => element.GetElementsByTagName("td")
                                                                  .Select(element => element.Text())
                                                                  .ToArray())
                                        .ToArray();

            var indexes = packageTable.Select(el => el[0]).ToArray();

            var indexDictionary = new Dictionary<string, List<int>>();

            for (int i = 0, j = 0; i < numberOfPackages; i++)
            {
                var isPairIndex = indexes[j].Contains('-');

                if (isPairIndex)
                {
                    var indexesPair = indexes[j].Split('-')
                                                .Select(ind => int.Parse(ind))
                                                .ToArray();

                    if (indexDictionary.ContainsKey(indexes[j]))
                        indexDictionary[indexes[j]].Add(i);
                    else
                        indexDictionary.Add(indexes[j], new List<int>() { i });

                    if (indexesPair[1] == i + 1)
                        j++;
                }
                else
                {
                    indexDictionary.Add(indexes[j], new List<int>() { i });
                    j++;
                }
            }

            return indexDictionary;
        }

        private Dictionary<string, List<int>> GetIndexesOfElongationTable(IHtmlDocument document)
        { 
            var packageTable = document.QuerySelectorAll("tbody")
                                        .Where(element => element.Text()
                                                                 .Contains("Elongation"))
                                        .Last()
                                        .GetElementsByTagName("tr")
                                        .Skip(1)
                                        .Select(element => element.GetElementsByTagName("td")
                                                                  .Select(element => element.Text())
                                                                  .ToArray())
                                        .ToArray();

            var indexes = packageTable.Select(el => el[0]).ToArray();

            var indexDictionary = new Dictionary<string, List<int>>();

            for (int i = 0, j = 0; i < numberOfPackages; i++)
            {
                var isPairIndex = indexes[j].Contains('-');

                if (isPairIndex)
                {
                    var indexesPair = indexes[j].Split('-')
                                                .Select(ind => int.Parse(ind))
                                                .ToArray();

                    if (indexDictionary.ContainsKey(indexes[j]))
                        indexDictionary[indexes[j]].Add(i);
                    else
                        indexDictionary.Add(indexes[j], new List<int>() { i });

                    if (indexesPair[1] == i + 1)
                        j++;
                }
                else
                {
                    indexDictionary.Add(indexes[j], new List<int>() { i });
                    j++;
                }
            }

            return indexDictionary;
        }
    }
}

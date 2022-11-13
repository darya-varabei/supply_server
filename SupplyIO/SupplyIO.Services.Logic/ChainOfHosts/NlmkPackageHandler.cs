using Newtonsoft.Json;
using SupplyIO.SupplyIO.Services.Models.CertificateModel;
using SupplyIO.SupplyIO.Services.Models.NlmkPackageJson;

namespace SupplyIO.SupplyIO.Services.Logic.ChainOfHosts
{
    public class NlmkPackageHandler : Handler
    {
        private readonly string context = "nlmk.shop/p";

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

            _httpClient.BaseAddress = new Uri("https://doc.nlmk.shop/api/v1/views/");

            var identyOfCertificate = link.GetComponents(UriComponents.Query, UriFormat.UriEscaped);

            var page = await _httpClient.GetAsync($"certificates?product={identyOfCertificate[2..]}&lang=ru");
            var bodyOfPage = await page.Content.ReadAsStringAsync();
            var root = JsonConvert.DeserializeObject<List<RootPackage>>(bodyOfPage).FirstOrDefault();

            var nlmkCertificateHandler = new NlmkCertificateHandler();
            var certificate = await nlmkCertificateHandler.HandleRequestAsync(new Uri($"https://doc.nlmk.shop/c?q={root.Product.Elements[0].Elements[0].Value}"));

            certificate.Packages.RemoveAll(pac => pac.Batch != root.Product.Elements[1].Elements[0].Value.ToString()[18..27]);

            return certificate;
        }
    }
}

using Newtonsoft.Json;

namespace SupplyIO.SupplyIO.Services.Models.NlmkCertificateJson
{
    public class RootCertificate
    {
        [JsonProperty("elements")]
        public List<ElementCertificate> Elements { get; set; }
    }
}

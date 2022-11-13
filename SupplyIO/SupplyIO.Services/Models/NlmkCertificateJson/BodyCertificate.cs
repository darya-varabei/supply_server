using Newtonsoft.Json;

namespace SupplyIO.SupplyIO.Services.Models.NlmkCertificateJson
{
    public class BodyCertificate
    {
        [JsonProperty("@meta")]
        public MetaCertificate Meta { get; set; }

        [JsonProperty("tr")]
        public List<string> Tr { get; set; }
    }
}

using Newtonsoft.Json;

namespace SupplyIO.SupplyIO.Services.Models.NlmkCertificateJson
{
    public class MetaCertificate
    {
        [JsonProperty("product_code")]
        public string ProductCode { get; set; }

        [JsonProperty("cert_pos")]
        public string CertPos { get; set; }
    }
}

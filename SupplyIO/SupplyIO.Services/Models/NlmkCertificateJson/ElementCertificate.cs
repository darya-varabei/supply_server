using Newtonsoft.Json;

namespace SupplyIO.SupplyIO.Services.Models.NlmkCertificateJson
{
    public class ElementCertificate
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("sortField")]
        public string SortField { get; set; }

        [JsonProperty("head")]
        public List<string> Head { get; set; }

        [JsonProperty("body")]
        public List<BodyCertificate> Body { get; set; }

        [JsonProperty("key")]
        public string Key { get; set; }

        [JsonProperty("value")]
        public object Value { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("elements")]
        public List<ElementCertificate> Elements { get; set; }
    }
}

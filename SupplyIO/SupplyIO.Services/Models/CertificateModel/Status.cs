using System.Text.Json.Serialization;

namespace SupplyIO.SupplyIO.Services.Models.CertificateModel
{
    public class Status
    {
        public int StatusId { get; set; }
        public string StatusName { get; set; }
        [JsonIgnore]
        public List<Package> Packages { get; set; } = new List<Package>();
    }
}

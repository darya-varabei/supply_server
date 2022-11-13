namespace SupplyIO.SupplyIO.Services.Models.CertificateModel
{
    public class Certificate
    {
        public int CertificateId { get; set; }
        public List<string> Link { get; set; }
        public string? Number { get; set; }
        public DateTime? Date { get; set; }
        public string? Author { get; set; }
        public string? AuthorAddress { get; set; }
        public string? Fax { get; set; }
        public string? Recipient { get; set; }
        public string? RecipientCountry { get; set; }
        public string? Contract { get; set; }
        public string? SpecificationNumber { get; set; }
        public Product? Product { get; set; }
        public string? ShipmentShop { get; set; }
        public string? WagonNumber { get; set; }
        public string? OrderNumber { get; set; }
        public string? TypeOfRollingStock { get; set; }
        public string? TypeOfPackaging { get; set; }
        public string? PlaceNumber { get; set; }
        public string? Gosts { get; set; }
        public string? Notes { get; set; }
        public List<Package>? Packages { get; set; }
    }
}

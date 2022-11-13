namespace SupplyIO.SupplyIO.Services.ViewModel
{
    public class PackageViewModel
    {
        public DateTime? SupplyDate { get; set; }
        public int PackageId { get; set; }
        public string Batch { get; set; }
        public string? Grade { get; set; }
        public string? NumberOfCertificate { get; set; }
        public double? Width { get; set; }
        public double? Thickness { get; set; }
        public double? Weight { get; set; }
        public string? Mill { get; set; }
        public string? CoatingClass { get; set; }
        public string? Sort { get; set; }
        public double? Net { get; set; }
        public string? Supplier { get; set; }
        public double? Elongation { get; set; }
        public double? Price { get; set; }
        public string? Comment { get; set; }
        public List<byte[]>? Photo { get; set; }
        public string? Status { get; set; }
    }
}

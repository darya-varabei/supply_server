namespace SupplyIO.SupplyIO.Services.ViewModel
{
    public class ExtendedPackageViewModel
    {
        public int PackageId { get; set; }

        public string? NumberOfCertificate { get; set; }
        public string? Batch { get; set; }

        public DateTime? SupplyDate { get; set; }
        public string? Supplier { get; set; }
        public string? Grade { get; set; }
        public double? Width { get; set; }
        public double? Thickness { get; set; }
        public double? Gros { get; set; }
        public double? Net { get; set; }
        public string? CoatingClass { get; set; }
        public double? Elongation { get; set; }
        public string? Sort { get; set; }
        public double? Price { get; set; }

        public string? NumberOfHeat { get; set; }
        public double? C { get; set; }
        public double? Si { get; set; }
        public double? Mn { get; set; }
        public double? S { get; set; }
        public double? P { get; set; }
        public double? Al { get; set; }
        public double? Cr { get; set; }
        public double? Ni { get; set; }
        public double? Cu { get; set; }
        public double? Ti { get; set; }
        public double? N2 { get; set; }
        public double? As { get; set; }

        public string? TrimOfEdge { get; set; }
        public double? TemporalResistance { get; set; }
        public double? CoatingThickness { get; set; }
        public string? TensilePoint { get; set; }
        public string? GrainSize { get; set; }

        public string? Comment { get; set; }
        public List<byte[]>? Photo { get; set; }
    }
}

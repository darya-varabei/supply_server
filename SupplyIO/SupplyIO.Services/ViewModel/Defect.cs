namespace SupplyIO.SupplyIO.Services.ViewModel
{
    public class Defect
    {
        public int? packageId { get; set; }
        public string? Comment { get; set; }
        public List<int[]> Photo { get; set; }
    }
}

namespace SupplyIO.SupplyIO.Services.Models.NlmkPackageJson
{
    public class ElementPackage
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public List<ElementPackage> Elements { get; set; }
        public string Key { get; set; }
        public object Value { get; set; }
    }
}

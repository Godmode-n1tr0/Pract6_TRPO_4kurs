namespace Pract6.Models
{
    public class MedicineUpdate
    {
        public string? Name { get; set; }
        public string? Manufacturer { get; set; }
        public decimal? Price { get; set; }
        public DateOnly? ExpirationDate { get; set; }
        public bool? IsAvailable { get; set; }
    }
}

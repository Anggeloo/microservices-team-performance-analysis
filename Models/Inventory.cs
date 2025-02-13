using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace microservices_raw_material_management.Models
{
    public class Inventory
    {
        public int InventoryId { get; set; }
        public string InventoryCode { get; set; }
        public string ProductCode { get; set; }
        public int AvailableQuantity { get; set; }
        public bool Status { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}

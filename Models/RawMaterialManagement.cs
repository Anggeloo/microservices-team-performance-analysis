using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace microservices_raw_material_management.Models
{
    [Table("raw_material_management")]
    public class RawMaterialManagement
    {
        [Key]
        [Column("material_id")]
        public int MaterialId { get; set; }

        [Required]
        [Column("material_code")]
        [MaxLength(100)]
        public string MaterialCode { get; set; }

        [Column("inventory_code")]
        [MaxLength(100)]
        public string? InventoryCode { get; set; }

        [Required]
        [Column("material_name")]
        [MaxLength(255)]
        public string MaterialName { get; set; }

        [Column("available_quantity")]
        public int AvailableQuantity { get; set; } = 0;

        [Column("total_used_cost")]
        public decimal? TotalUsedCost { get; set; }

        [Column("used_quantity")]
        public int UsedQuantity { get; set; } = 0;

        [Column("material_status")]
        public bool MaterialStatus { get; set; } = true;

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Column("updated_at")]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}

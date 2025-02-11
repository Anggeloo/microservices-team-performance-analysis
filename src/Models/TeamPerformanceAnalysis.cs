using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace microservices_team_performance_analysis.Models
{
    [Table("team_performance_analysis")]
    public class TeamPerformanceAnalysis
    {
        [Key]
        [Column("team_performance_id")]
        public int TeamPerformanceId { get; set; }

        [Required]
        [Column("team_performance_code")]
        [StringLength(100)]
        public string TeamPerformanceCode { get; set; }

        [Required]
        [Column("team_code")]
        [StringLength(100)]
        public string TeamCode { get; set; }

        [Required]
        [Column("team_name")]
        public string TeamName { get; set; }

        [Column("completed_orders")]
        public int CompletedOrders { get; set; } = 0;

        [Column("pending_orders")]
        public int PendingOrders { get; set; } = 0;

        [Column("avg_completion_time")]
        public decimal? AvgCompletionTime { get; set; }

        [Column("avg_quality")]
        public decimal? AvgQuality { get; set; }

        [Column("efficiency")]
        public decimal? Efficiency { get; set; }

        [Column("status")]
        public bool Status { get; set; } = true;

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Column("updated_at")]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}

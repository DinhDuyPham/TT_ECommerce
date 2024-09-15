using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TT_ECommerce.Models.EF
{
    [Table("__MigrationHistory")]
    public class MigrationHistory
    {
        [Key]
        [StringLength(150)]
        public string MigrationId { get; set; } = null!;

        [Key]
        [StringLength(300)]
        public string ContextKey { get; set; } = null!;

        [Required]
        public byte[] Model { get; set; } = null!;

        [Required]
        [StringLength(32)]
        public string ProductVersion { get; set; } = null!;
    }
}

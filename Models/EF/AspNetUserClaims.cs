using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TT_ECommerce.Models.EF
{
    [Table("AspNetUserClaims")]
    public class AspNetUserClaims
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [StringLength(128)]
        public string UserId { get; set; } = null!;

        public string? ClaimType { get; set; }

        public string? ClaimValue { get; set; }

        // Mối quan hệ nhiều-một với AspNetUser
        [ForeignKey(nameof(UserId))]
        public AspNetUsers User { get; set; } = null!;
    }
}

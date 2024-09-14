using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TT_ECommerce.Models.EF
{
    [Table("AspNetUserRoles")]
    public class AspNetUserRoles
    {
        [Required]
        [StringLength(128)]
        public string UserId { get; set; } = null!;

        [Required]
        [StringLength(128)]
        public string RoleId { get; set; } = null!;

        // Mối quan hệ với AspNetUser
        [ForeignKey(nameof(UserId))]
        public AspNetUsers User { get; set; } = null!;

        // Mối quan hệ với AspNetRole
        [ForeignKey(nameof(RoleId))]
        public AspNetRoles Role { get; set; } = null!;
    }
}

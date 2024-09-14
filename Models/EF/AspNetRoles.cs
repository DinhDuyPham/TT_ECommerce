using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TT_ECommerce.Models.EF
{
    [Table("AspNetRoles")]
    public class AspNetRoles
    {
        [Key]
        [StringLength(128)]
        public string Id { get; set; } = null!;

        [Required]
        [StringLength(256)]
        public string Name { get; set; } = null!;
        public ICollection<AspNetUserRoles> UserRoles { get; set; } = new HashSet<AspNetUserRoles>();
    }
}

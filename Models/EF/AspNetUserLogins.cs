using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TT_ECommerce.Models.EF
{
    [Table("AspNetUserLogins")]
    public class AspNetUserLogin
    {
        [Key, Column(Order = 0)]
        [StringLength(128)]
        public string LoginProvider { get; set; } = null!;

        [Key, Column(Order = 1)]
        [StringLength(128)]
        public string ProviderKey { get; set; } = null!;

        [Key, Column(Order = 2)]
        [StringLength(128)]
        public string UserId { get; set; } = null!;

        // Mối quan hệ với AspNetUser
        [ForeignKey(nameof(UserId))]
        public AspNetUsers User { get; set; } = null!;
    }
}

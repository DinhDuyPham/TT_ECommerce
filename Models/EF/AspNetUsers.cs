using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TT_ECommerce.Models.EF
{
    [Table("AspNetUsers")]
    public class AspNetUsers
    {
        [Key]
        [StringLength(128)]
        public string Id { get; set; } = null!;

        [StringLength(int.MaxValue)] 
        public string? FullName { get; set; }

        [StringLength(int.MaxValue)] 
        public string? Phone { get; set; }

        [StringLength(256)]
        public string? Email { get; set; }

        public bool EmailConfirmed { get; set; }

        [StringLength(int.MaxValue)] 
        public string? PasswordHash { get; set; }

        [StringLength(int.MaxValue)] 
        public string? SecurityStamp { get; set; }

        [StringLength(int.MaxValue)] 
        public string? PhoneNumber { get; set; }

        public bool PhoneNumberConfirmed { get; set; }

        public bool TwoFactorEnabled { get; set; }

        public DateTimeOffset? LockoutEndDateUtc { get; set; }

        public bool LockoutEnabled { get; set; }

        public int AccessFailedCount { get; set; }

        [Required]
        [StringLength(256)]
        public string UserName { get; set; } = null!;

        public ICollection<AspNetUserClaims> UserClaims { get; set; } = new HashSet<AspNetUserClaims>();

        public ICollection<AspNetUserLogins> UserLogins { get; set; } = new HashSet<AspNetUserLogins>();

        public ICollection<AspNetUserRoles> UserRoles { get; set; } = new HashSet<AspNetUserRoles>();
    }
}

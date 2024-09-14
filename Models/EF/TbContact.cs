using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TT_ECommerce.Models.EF
{
    [Table("tb_Contact")]
    public class TbContact
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [StringLength(150)]
        public string Name { get; set; } = null!;

        [StringLength(150)]
        public string? Email { get; set; }

        [StringLength(int.MaxValue)]
        public string? Website { get; set; }

        [StringLength(4000)]
        public string? Message { get; set; }

        [Required]
        public bool IsRead { get; set; }

        [StringLength(int.MaxValue)]
        public string? CreatedBy { get; set; }

        [Required]
        public DateTime CreatedDate { get; set; }

        [Required]
        public DateTime ModifiedDate { get; set; }

        [StringLength(int.MaxValue)]
        public string? Modifiedby { get; set; }
    }
}

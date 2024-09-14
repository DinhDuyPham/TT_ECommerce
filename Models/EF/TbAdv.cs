using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TT_ECommerce.Models.EF
{
    [Table("tb_Adv")]
    public class TbAdv
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [StringLength(150)]
        public string Title { get; set; } = null!;

        [StringLength(500)]
        public string? Description { get; set; }

        [StringLength(500)]
        public string? Image { get; set; }

        [StringLength(500)]
        public string? Link { get; set; }

        [Required]
        public int Type { get; set; }

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

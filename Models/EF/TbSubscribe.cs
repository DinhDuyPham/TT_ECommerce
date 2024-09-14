using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TT_ECommerce.Models.EF
{
    [Table("tb_Subscribe")]
    public class TbSubscribe
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [StringLength(int.MaxValue)]
        public string Email { get; set; } = null!;

        [Required]
        public DateTime CreatedDate { get; set; }
    }
}

using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TT_ECommerce.Models.EF
{
    [Table("tb_ProductImage")]
    public class TbProductImage
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int ProductId { get; set; }

        [StringLength(int.MaxValue)]
        public string? Image { get; set; }

        [Required]
        public bool IsDefault { get; set; }

        [ForeignKey(nameof(ProductId))]
        public TbProduct Product { get; set; } = null!;
    }
}

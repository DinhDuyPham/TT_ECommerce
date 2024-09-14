using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TT_ECommerce.Models.EF
{
    [Table("tb_ProductCategory")]
    public class TbProductCategory
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [StringLength(150)]
        public string Title { get; set; } = null!;

        [StringLength(int.MaxValue)]
        public string? Description { get; set; }

        [StringLength(250)]
        public string? Icon { get; set; }

        [StringLength(250)]
        public string? SeoTitle { get; set; }

        [StringLength(500)]
        public string? SeoDescription { get; set; }

        [StringLength(250)]
        public string? SeoKeywords { get; set; }

        [StringLength(int.MaxValue)]
        public string? CreatedBy { get; set; }

        [Required]
        public DateTime CreatedDate { get; set; }

        [Required]
        public DateTime ModifiedDate { get; set; }

        [StringLength(int.MaxValue)]
        public string? ModifiedBy { get; set; }

        [Required]
        [StringLength(150)]
        public string Alias { get; set; } = null!;

        // Navigation property
        public ICollection<TbProduct> Products { get; set; } = new HashSet<TbProduct>();
    }
}

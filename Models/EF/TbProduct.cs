using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TT_ECommerce.Models.EF
{
    [Table("tb_Product")]
    public class TbProduct
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [StringLength(250)]
        public string Title { get; set; } = null!;

        [StringLength(50)]
        public string? ProductCode { get; set; }

        [StringLength(int.MaxValue)]
        public string? Description { get; set; }

        [StringLength(int.MaxValue)]
        public string? Detail { get; set; }

        [StringLength(250)]
        public string? Image { get; set; }

        [Required]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Price { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal? PriceSale { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        public bool IsHome { get; set; }

        [Required]
        public bool IsSale { get; set; }

        [Required]
        public bool IsFeature { get; set; }

        [Required]
        public bool IsHot { get; set; }

        [Required]
        public int ProductCategoryId { get; set; }

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
        public string? Modifiedby { get; set; }

        [StringLength(250)]
        public string? Alias { get; set; }

        [Required]
        public bool IsActive { get; set; }

        [Required]
        public int ViewCount { get; set; }

        [Required]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal OriginalPrice { get; set; }

        // Navigation property
        [ForeignKey("ProductCategoryId")]
        public TbCategory ProductCategory { get; set; } = null!;
    }
}

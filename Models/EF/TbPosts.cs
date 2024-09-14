﻿using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TT_ECommerce.Models.EF
{
    [Table("tb_Posts")]
    public class TbPosts
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [StringLength(150)]
        public string Title { get; set; } = null!;

        [StringLength(int.MaxValue)]
        public string? Description { get; set; }

        [StringLength(int.MaxValue)]
        public string? Detail { get; set; }

        [StringLength(250)]
        public string? Image { get; set; }

        [Required]
        public int CategoryId { get; set; }

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

        [StringLength(150)]
        public string? Alias { get; set; }

        [Required]
        public bool IsActive { get; set; }

        [ForeignKey("CategoryId")]
        public TbCategory Category { get; set; } = null!;
    }
}

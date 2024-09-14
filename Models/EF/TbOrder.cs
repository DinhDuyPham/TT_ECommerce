using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TT_ECommerce.Models.EF
{
    [Table("tb_Order")]
    public class TbOrder
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string Code { get; set; } = null!;

        [Required]
        public string CustomerName { get; set; } = null!;

        [Required]
        public string Phone { get; set; } = null!;

        [Required]
        public string Address { get; set; } = null!;

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalAmount { get; set; }

        [Required]
        public int Quantity { get; set; }

        public string? CreatedBy { get; set; }

        [Required]
        public DateTime CreatedDate { get; set; }

        [Required]
        public DateTime ModifiedDate { get; set; }

        public string? Modifiedby { get; set; }

        [Required]
        public int TypePayment { get; set; }

        public string? Email { get; set; }

        public int? Status { get; set; }
        public ICollection<TbOrderDetail> OrderDetails { get; set; } = new HashSet<TbOrderDetail>();
    }
}

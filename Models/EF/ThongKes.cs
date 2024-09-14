using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TT_ECommerce.Models.EF
{
    [Table("ThongKes")]
    public class ThongKe
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public DateTime ThoiGian { get; set; }

        [Required]
        public long SoTruyCap { get; set; }
    }
}

using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TT_ECommerce.Models.EF
{
    [Table("tb_SystemSetting")]
    public class TbSystemSetting
    {
        [Key]
        [StringLength(50)]
        public string SettingKey { get; set; } = null!;

        [StringLength(4000)]
        public string? SettingValue { get; set; }

        [StringLength(4000)]
        public string? SettingDescription { get; set; }
    }
}

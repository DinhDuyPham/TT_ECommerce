using System.ComponentModel.DataAnnotations;

namespace TT_ECommerce.Areas.Admin.Models
{
    public class EditUserModel
    {
        public string UserId { get; set; }

        [Required(ErrorMessage = "Email là bắt buộc.")]
        [EmailAddress(ErrorMessage = "Địa chỉ email không hợp lệ.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Tên tài khoản là bắt buộc.")]
        public string UserName { get; set; }
    }

}


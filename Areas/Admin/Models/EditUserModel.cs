using System.ComponentModel.DataAnnotations;

namespace TT_ECommerce.Areas.Admin.Models
{
    public class EditUserModel
    {
        public string UserId { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
    }

}


using System;
using System.Collections.Generic;

namespace TT_ECommerce.Models.EF;

public partial class AspNetUserRole
{
    public string UserId { get; set; } = null!;

    public string RoleId { get; set; } = null!;
     public virtual AspNetUser User { get; set; } = null!;
    // Khóa ngoại liên kết với AspNetRole
    public virtual AspNetRole Role { get; set; } = null!;
}

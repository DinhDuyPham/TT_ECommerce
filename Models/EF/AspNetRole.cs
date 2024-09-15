using System;
using System.Collections.Generic;

namespace TT_ECommerce.Models.EF;

public partial class AspNetRole
{
    public string Id { get; set; } = null!;

    public string Name { get; set; } = null!;

    public virtual ICollection<AspNetUserRole> UserRoles { get; set; } = new List<AspNetUserRole>();
}

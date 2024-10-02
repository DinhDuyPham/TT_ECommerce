using System;
using System.Collections.Generic;

namespace TT_ECommerce.Data;

public partial class TbAdv
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public string? Image { get; set; }

    public string? Link { get; set; }

    public int Type { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime CreatedDate { get; set; }

    public DateTime ModifiedDate { get; set; }

    public string? Modifiedby { get; set; }
}

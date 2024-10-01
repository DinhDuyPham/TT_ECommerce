using System;
using System.Collections.Generic;

namespace TT_ECommerce.Models.EF;

public partial class TbCategory
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public string? SeoTitle { get; set; }

    public string? SeoDescription { get; set; }

    public string? SeoKeywords { get; set; }

    public int Position { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime CreatedDate { get; set; }

    public DateTime ModifiedDate { get; set; }

    public string? Modifiedby { get; set; }

    public string? Alias { get; set; }

    public bool IsActive { get; set; }

    public string? Link { get; set; }

    public virtual ICollection<TbNews> TbNews { get; set; } = new List<TbNews>();

    public virtual ICollection<TbPost> TbPosts { get; set; } = new List<TbPost>();
}

using System;
using System.Collections.Generic;

namespace TT_ECommerce.Data;

public partial class TbSystemSetting
{
    public string SettingKey { get; set; } = null!;

    public string? SettingValue { get; set; }

    public string? SettingDescription { get; set; }
}

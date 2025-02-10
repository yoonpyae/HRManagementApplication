using System;
using System.Collections.Generic;

namespace HRManagement.Models;

public partial class HrTownship
{
    public int TownshipId { get; set; }

    public string? TownshipName { get; set; }

    public string? TownshipNameMm { get; set; }

    public int StateId { get; set; }
}

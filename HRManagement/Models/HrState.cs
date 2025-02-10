using System;
using System.Collections.Generic;

namespace HRManagement.Models;

public partial class HrState
{
    public int StateId { get; set; }

    public string? StateName { get; set; }

    public string? StateNameMm { get; set; }
}

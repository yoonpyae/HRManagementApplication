using System;
using System.Collections.Generic;

namespace HRManagement.Models;

public partial class HrStreet
{
    public int StreetId { get; set; }

    public string? StreetName { get; set; }

    public string? StreetNameMm { get; set; }

    public int TownshipId { get; set; }

    public double? Lat { get; set; }

    public double? Long { get; set; }
}

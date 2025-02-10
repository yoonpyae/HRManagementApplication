﻿using System;
using System.Collections.Generic;

namespace HRManagement.Models;

public partial class ViHrDailyReportTitle
{
    public long ReportTitleId { get; set; }

    public string? ReportTitle { get; set; }

    public string CompanyId { get; set; } = null!;

    public string? CompanyName { get; set; }

    public bool Status { get; set; }

    public DateTime? CreatedOn { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime? UpdatedOn { get; set; }

    public string? UpdatedBy { get; set; }

    public DateTime? DeletedOn { get; set; }

    public string? DeletedBy { get; set; }

    public string? Remark { get; set; }
}

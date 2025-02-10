﻿using System;
using System.Collections.Generic;

namespace HRManagement.Models;

public partial class ViHrPosition
{
    public long PositionId { get; set; }

    public string? PositionName { get; set; }

    public long DeptId { get; set; }

    public string? DeptName { get; set; }

    public long BranchId { get; set; }

    public string? BranchName { get; set; }

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

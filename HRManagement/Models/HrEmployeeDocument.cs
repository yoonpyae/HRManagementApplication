﻿using System;
using System.Collections.Generic;

namespace HRManagement.Models;

public partial class HrEmployeeDocument
{
    public long DocId { get; set; }

    public string EmployeeId { get; set; } = null!;

    public string? Description { get; set; }

    public string? DocLink { get; set; }

    public bool Status { get; set; }

    public DateTime? CreatedOn { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime? UpdatedOn { get; set; }

    public string? UpdatedBy { get; set; }

    public DateTime? DeletedOn { get; set; }

    public string? DeletedBy { get; set; }

    public string? Remark { get; set; }

    public virtual HrEmployee Employee { get; set; } = null!;
}

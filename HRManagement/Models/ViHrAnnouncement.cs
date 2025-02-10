﻿using System;
using System.Collections.Generic;

namespace HRManagement.Models;

public partial class ViHrAnnouncement
{
    public long Id { get; set; }

    public string? Title { get; set; }

    public string? Description { get; set; }

    public string? Img { get; set; }

    public string? CompanyId { get; set; }

    public string? CompanyName { get; set; }

    public long? DeptId { get; set; }

    public string? DeptName { get; set; }

    public string? EmployeeId { get; set; }

    public string? EmployeeName { get; set; }

    public DateOnly? StartOn { get; set; }

    public DateOnly? EndOn { get; set; }

    public DateTime? CreatedOn { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime? UpdatedOn { get; set; }

    public string? UpdatedBy { get; set; }

    public DateTime? DeletedOn { get; set; }

    public string? DeletedBy { get; set; }

    public string? Remark { get; set; }
}

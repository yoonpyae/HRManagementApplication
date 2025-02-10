using System;
using System.Collections.Generic;

namespace HRManagement.Models;

public partial class TokenClaim
{
    public string UserId { get; set; } = null!;

    public string Device { get; set; } = null!;

    public DateTime? RefreshDate { get; set; }

    public string? AccessToken { get; set; }

    public string? RefreshToken { get; set; }

    public DateTime? TokenExpiry { get; set; }
}

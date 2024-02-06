using System;
using System.Collections.Generic;

namespace API_Complete_ASP.Models
{
    public partial class ResetPass
    {
        public int IdResetPass { get; set; }
        public string? Password { get; set; }
        public string? ConfirmPassword { get; set; }
    }
}

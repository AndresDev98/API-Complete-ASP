using System;
using System.Collections.Generic;

namespace API_Complete_ASP.Models
{
    public partial class Contact
    {
        public int IdContact { get; set; }
        public string? Name { get; set; }
        public string Email { get; set; } = null!;
        public string? Reference { get; set; }
        public string? Description { get; set; }
    }
}

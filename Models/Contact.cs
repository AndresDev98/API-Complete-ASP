using System;
using System.Collections.Generic;

namespace API_Complete_ASP.Models
{
    public partial class Contact
    {
        public int IdContact { get; set; }
        public string? Title { get; set; }
        public string? Reference { get; set; }
        public string? Description { get; set; }
    }
}

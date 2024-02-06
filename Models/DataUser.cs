using System;
using System.Collections.Generic;

namespace API_Complete_ASP.Models
{
    public partial class DataUser
    {
        public int IdData { get; set; }
        public string? Name { get; set; }
        public string? LastName { get; set; }
        public int? Age { get; set; }
        public int? Phone { get; set; }
        public string? Country { get; set; }
        public string? City { get; set; }
        public string? Genero { get; set; }
    }
}

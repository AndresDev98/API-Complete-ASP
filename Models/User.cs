﻿using System;
using System.Collections.Generic;

namespace API_Complete_ASP.Models
{
    public partial class User
    {
        public int IdUser { get; set; }
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
    }
}

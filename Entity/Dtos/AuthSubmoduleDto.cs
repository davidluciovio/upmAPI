﻿using Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Dtos
{
    public class AuthSubmoduleDto
    {
        public Guid Id { get; set; }

        public string Submodule { get; set; } = string.Empty;
        public Guid ModuleId { get; set; }

        public List<AuthPermissionsDto>? Permissions { get; set; }
    }
}

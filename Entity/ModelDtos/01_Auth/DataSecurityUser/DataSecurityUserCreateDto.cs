using System;

namespace Entity.Dtos._00_DataUPM
{
    public class DataSecurityUserCreateDto
    {
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string RoleId { get; set; }
        public string CreateBy { get; set; } = string.Empty;
    }
}

using System;

namespace Entity.Dtos._00_DataUPM
{
    public class DataSecurityUserUpdateDto
    {
        public string UserName { get; set; } = string.Empty;
        public string PrettyName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public bool Active { get; set; }
        public Guid RoleId { get; set; }
        public string UpdateBy { get; set; } = string.Empty;
    }
}

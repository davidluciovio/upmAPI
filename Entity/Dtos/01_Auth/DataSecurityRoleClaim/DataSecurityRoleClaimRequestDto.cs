namespace Entity.Dtos._01_Auth.DataSecurityRoleClaim
{
    public class DataSecurityRoleClaimRequestDto
    {
        public string RoleId { get; set; } = string.Empty;
        public string ClaimType { get; set; } = string.Empty;
        public string ClaimValue { get; set; } = string.Empty;
    }
}

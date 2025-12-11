namespace Entity.Dtos._01_Auth.DataSecurityRoleClaim
{
    public class DataSecurityRoleClaimResponseDto
    {
        public int Id { get; set; }
        public string RoleId { get; set; } = string.Empty;
        public string ClaimType { get; set; } = string.Empty;
        public string ClaimValue { get; set; } = string.Empty;
    }
}

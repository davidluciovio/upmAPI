namespace Entity.Dtos._01_Auth.RoleClaim
{
    public class RoleClaimRequestDto
    {
        public string RoleId { get; set; } = string.Empty;
        public string ClaimType { get; set; } = string.Empty;
        public string ClaimValue { get; set; } = string.Empty;
    }
}

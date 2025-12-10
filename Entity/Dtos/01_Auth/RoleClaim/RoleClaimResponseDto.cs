namespace Entity.Dtos._01_Auth.RoleClaim
{
    public class RoleClaimResponseDto
    {
        public int Id { get; set; }
        public string RoleId { get; set; } = string.Empty;
        public string ClaimType { get; set; } = string.Empty;
        public string ClaimValue { get; set; } = string.Empty;
    }
}

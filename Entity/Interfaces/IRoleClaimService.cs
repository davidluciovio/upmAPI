using Entity.Dtos._01_Auth.RoleClaim;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Interfaces
{
    public interface IRoleClaimService
    {
        Task<List<RoleClaimResponseDto>> GetClaimsByRoleId(string roleId);
        Task<RoleClaimResponseDto> AddClaimToRole(RoleClaimRequestDto request);
        Task<bool> RemoveClaimFromRole(int claimId);
    }
}

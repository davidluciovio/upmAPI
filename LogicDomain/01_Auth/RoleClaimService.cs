using Entity.Dtos._01_Auth.RoleClaim;
using Entity.Interfaces;
using LogicData.Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicDomain._01_Auth
{
    public class RoleClaimService : IRoleClaimService
    {
        private readonly AuthContext _authContext;
        public RoleClaimService(AuthContext authContext)
        {
            _authContext = authContext;
        }

        public async Task<RoleClaimResponseDto> AddClaimToRole(RoleClaimRequestDto request)
        {
            var roleExists = await _authContext.Roles.AnyAsync(r => r.Id == request.RoleId);
            if (!roleExists)
            {
                throw new KeyNotFoundException($"Role with ID '{request.RoleId}' not found.");
            }

            var claimExists = await _authContext.RoleClaims.AnyAsync(rc => rc.RoleId == request.RoleId && rc.ClaimType == request.ClaimType && rc.ClaimValue == request.ClaimValue);
            if (claimExists)
            {
                throw new InvalidOperationException("This claim already exists for this role.");
            }

            var roleClaim = new IdentityRoleClaim<string>
            {
                RoleId = request.RoleId,
                ClaimType = request.ClaimType,
                ClaimValue = request.ClaimValue
            };

            _authContext.RoleClaims.Add(roleClaim);
            await _authContext.SaveChangesAsync();

            return new RoleClaimResponseDto
            {
                Id = roleClaim.Id,
                RoleId = roleClaim.RoleId,
                ClaimType = roleClaim.ClaimType,
                ClaimValue = roleClaim.ClaimValue
            };
        }

        public async Task<List<RoleClaimResponseDto>> GetClaimsByRoleId(string roleId)
        {
            var roleExists = await _authContext.Roles.AnyAsync(r => r.Id == roleId);
            if (!roleExists)
            {
                throw new KeyNotFoundException($"Role with ID '{roleId}' not found.");
            }

            return await _authContext.RoleClaims
                .Where(rc => rc.RoleId == roleId)
                .Select(rc => new RoleClaimResponseDto
                {
                    Id = rc.Id,
                    RoleId = rc.RoleId,
                    ClaimType = rc.ClaimType ?? "",
                    ClaimValue = rc.ClaimValue ?? ""
                }).ToListAsync();
        }

        public async Task<bool> RemoveClaimFromRole(int claimId)
        {
            var roleClaim = await _authContext.RoleClaims.FindAsync(claimId);
            if (roleClaim == null)
            {
                throw new KeyNotFoundException($"Claim with ID '{claimId}' not found.");
            }

            _authContext.RoleClaims.Remove(roleClaim);
            var result = await _authContext.SaveChangesAsync();

            return result > 0;
        }
    }
}

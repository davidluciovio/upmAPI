using Entity.Dtos._01_Auth.DataSecurityRoleClaim;
using Entity.Interfaces;
using Entity.Models.Auth;
using LogicData.Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace LogicDomain._01_Auth
{
    public class DataSecurityRoleClaimService : IServiceRoleClaim<DataSecurityRoleClaimResponseDto, DataSecurityRoleClaimRequestDto>
    {
        private readonly RoleManager<AuthRole> _roleManager;
        private readonly AuthContext _context;

        public DataSecurityRoleClaimService(RoleManager<AuthRole> roleManager, AuthContext context)
        {
            _roleManager = roleManager;
            _context = context;
        }

        public async Task<DataSecurityRoleClaimResponseDto> Create(DataSecurityRoleClaimRequestDto dtocreate)
        {
            var role = await _roleManager.FindByIdAsync(dtocreate.RoleId);
            if (role == null)
            {
                throw new KeyNotFoundException($"Role with ID '{dtocreate.RoleId}' not found.");
            }

            var claim = new Claim(dtocreate.ClaimType, dtocreate.ClaimValue);

            var existingClaims = await _roleManager.GetClaimsAsync(role);
            if (existingClaims.Any(c => c.Type == claim.Type && c.Value == claim.Value))
            {
                throw new InvalidOperationException("Claim already exists for this role.");
            }

            var result = await _roleManager.AddClaimAsync(role, claim);

            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new Exception($"Failed to add claim: {errors}");
            }

            // Find the claim we just added to get its ID
            var addedClaim = await _context.RoleClaims
                .AsNoTracking()
                .FirstOrDefaultAsync(rc => rc.RoleId == dtocreate.RoleId && rc.ClaimType == dtocreate.ClaimType && rc.ClaimValue == dtocreate.ClaimValue);

            if (addedClaim == null)
            {
                // This should not happen if AddClaimAsync succeeded
                throw new Exception("Could not retrieve the claim after adding it.");
            }

            return new DataSecurityRoleClaimResponseDto
            {
                Id = addedClaim.Id,
                RoleId = addedClaim.RoleId,
                ClaimType = addedClaim.ClaimType ?? string.Empty,
                ClaimValue = addedClaim.ClaimValue ?? string.Empty
            };
        }

        public async Task<List<DataSecurityRoleClaimResponseDto>> GetAlls()
        {
            return await _context.RoleClaims
                .AsNoTracking()
                .Select(rc => new DataSecurityRoleClaimResponseDto
                {
                    Id = rc.Id,
                    RoleId = rc.RoleId,
                    ClaimType = rc.ClaimType ?? string.Empty,
                    ClaimValue = rc.ClaimValue ?? string.Empty
                })
                .ToListAsync();
        }

        public async Task<DataSecurityRoleClaimResponseDto?> GetById(int id)
        {
            var roleClaim = await _context.RoleClaims.FindAsync(id);

            if (roleClaim == null)
            {
                return null;
            }

            return new DataSecurityRoleClaimResponseDto
            {
                Id = roleClaim.Id,
                RoleId = roleClaim.RoleId,
                ClaimType = roleClaim.ClaimType ?? string.Empty,
                ClaimValue = roleClaim.ClaimValue ?? string.Empty
            };
        }

        public async Task<DataSecurityRoleClaimResponseDto> Update(int id, DataSecurityRoleClaimRequestDto dtoUpdate)
        {
            var roleClaim = await _context.RoleClaims.FindAsync(id);

            if (roleClaim == null)
            {
                throw new KeyNotFoundException($"RoleClaim with ID '{id}' not found.");
            }
            
            // To properly update a claim via RoleManager, we need to remove the old one and add the new one.
            var role = await _roleManager.FindByIdAsync(roleClaim.RoleId);
            if(role == null)
            {
                // This would indicate data inconsistency
                throw new KeyNotFoundException($"Associated role with ID '{roleClaim.RoleId}' not found.");
            }

            var oldClaim = new Claim(roleClaim.ClaimType, roleClaim.ClaimValue);
            var newClaim = new Claim(dtoUpdate.ClaimType, dtoUpdate.ClaimValue);

            // 1. Remove the old claim
            var removeResult = await _roleManager.RemoveClaimAsync(role, oldClaim);
            if (!removeResult.Succeeded)
            {
                var errors = string.Join(", ", removeResult.Errors.Select(e => e.Description));
                throw new Exception($"Failed to remove old claim during update: {errors}");
            }

            // 2. Add the new claim
            var addResult = await _roleManager.AddClaimAsync(role, newClaim);
            if (!addResult.Succeeded)
            {
                // Attempt to roll back by re-adding the old claim
                await _roleManager.AddClaimAsync(role, oldClaim);
                var errors = string.Join(", ", addResult.Errors.Select(e => e.Description));
                throw new Exception($"Failed to add new claim during update: {errors}");
            }

            // 3. Find the newly created claim to get its ID
            var updatedClaim = await _context.RoleClaims
                .AsNoTracking()
                .FirstOrDefaultAsync(rc => rc.RoleId == role.Id && rc.ClaimType == newClaim.Type && rc.ClaimValue == newClaim.Value);
            
            if(updatedClaim == null)
            {
                throw new Exception("Could not retrieve the updated claim after update operation.");
            }

            return new DataSecurityRoleClaimResponseDto
            {
                Id = updatedClaim.Id,
                RoleId = updatedClaim.RoleId,
                ClaimType = updatedClaim.ClaimType ?? string.Empty,
                ClaimValue = updatedClaim.ClaimValue ?? string.Empty
            };
        }
    }
}

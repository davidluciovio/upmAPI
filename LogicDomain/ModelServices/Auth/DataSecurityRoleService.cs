using Entity.Dtos._00_DataUPM.DataSecurityRole;
using Entity.Interfaces;
using Entity.Models.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LogicDomain._00_DataUPM
{
    public class DataSecurityRoleService : IService<DataSecurityRoleResponseDto, DataSecurityRoleRequestDto>
    {
        private readonly RoleManager<AuthRole> _roleManager;

        public DataSecurityRoleService(RoleManager<AuthRole> roleManager)
        {
            _roleManager = roleManager;
        }

        public async Task<DataSecurityRoleResponseDto> Create(DataSecurityRoleRequestDto dtocreate)
        {
            if (await _roleManager.RoleExistsAsync(dtocreate.Name))
            {
                throw new InvalidOperationException($"Role with name '{dtocreate.Name}' already exists.");
            }

            var role = new AuthRole
            {
                Name = dtocreate.Name
            };

            var result = await _roleManager.CreateAsync(role);

            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new Exception($"Failed to create role: {errors}");
            }

            // The role object is updated with the ID and NormalizedName after creation.
            return new DataSecurityRoleResponseDto
            {
                Id = role.Id,
                Name = role.Name,
                NormalizedName = role.NormalizedName ?? string.Empty
            };
        }

        public async Task<List<DataSecurityRoleResponseDto>> GetAlls()
        {
            return await _roleManager.Roles
                .Select(r => new DataSecurityRoleResponseDto
                {
                    Id = r.Id,
                    Name = r.Name ?? string.Empty,
                    NormalizedName = r.NormalizedName ?? string.Empty
                })
                .ToListAsync();

        }

        public async Task<DataSecurityRoleResponseDto?> GetById(Guid id)
        {
            var role = await _roleManager.FindByIdAsync(id.ToString());

            if (role == null)
            {
                return null;
            }

            return new DataSecurityRoleResponseDto
            {
                Id = role.Id,
                Name = role.Name ?? string.Empty,
                NormalizedName = role.NormalizedName ?? string.Empty
            };
        }

        public async Task<DataSecurityRoleResponseDto> Update(Guid id, DataSecurityRoleRequestDto dtoUpdate)
        {
            var role = await _roleManager.FindByIdAsync(id.ToString());

            if (role == null)
            {
                throw new KeyNotFoundException($"Role with ID '{id}' not found.");
            }

            var existingRoleWithSameName = await _roleManager.FindByNameAsync(dtoUpdate.Name);
            if (existingRoleWithSameName != null && existingRoleWithSameName.Id != role.Id)
            {
                throw new InvalidOperationException($"Another role with name '{dtoUpdate.Name}' already exists.");
            }

            role.Name = dtoUpdate.Name;
            var result = await _roleManager.UpdateAsync(role);

            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new Exception($"Failed to update role: {errors}");
            }
            
            return new DataSecurityRoleResponseDto
            {
                Id = role.Id,
                Name = role.Name,
                NormalizedName = role.NormalizedName ?? string.Empty
            };
        }
    }
}

using Entity.Dtos._00_DataUPM.DataSecurityRole;
using Entity.Interfaces;
using Entity.Models.Auth;
using LogicData.Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicDomain._00_DataUPM
{
    public class DataSecurityRoleService : IService<DataSecurityRoleResponseDto, DataSecurityRoleRequestDto>
    {
        private readonly AuthContext _authContext;

        public DataSecurityRoleService(AuthContext authContext)
        {
            _authContext = authContext;
        }

        public Task<DataSecurityRoleResponseDto> Create(DataSecurityRoleRequestDto dtocreate)
        {
            throw new NotImplementedException();
        }

        public async Task<List<DataSecurityRoleResponseDto>> GetAlls()
        {
            var roles = await _authContext.Roles
                .Select(r => new DataSecurityRoleResponseDto
                {
                    Id = r.Id.ToString(),
                    Name = r.Name ?? string.Empty,
                    NormalizedName = r.NormalizedName ?? string.Empty
                })
                .ToListAsync();

            return roles.Select(r => new DataSecurityRoleResponseDto
            {
                Id = r.Id,
                Name = r.Name,
                NormalizedName = r.NormalizedName

            }).ToList();

        }

        public Task<DataSecurityRoleResponseDto?> GetById(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<DataSecurityRoleResponseDto> Update(Guid id, DataSecurityRoleRequestDto dtoUpdate)
        {
            throw new NotImplementedException();
        }
    }
}

using Entity.AplicationDtos._01_ProductionAcvhievementDtos;
using LogicData.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicDomain.ApplicationServices
{
    public class ProductionAchievementService
    {
        private readonly TemporalContext _temporalContext;

        public ProductionAchievementService(TemporalContext temporalContext)
        {
            _temporalContext = temporalContext;
        }

        public async Task<List<ProductionAchievementResponseDto.ProductionReportDto>> GetProductionAchievement(ProductionAchievementRequestDto request)
        {
            var query = await _temporalContext.ProductionAchievements
                .Where(pa => pa.ProductionDate >= request.StarDate && pa.ProductionDate <= request.EndDate)
                .ToListAsync();

            var groupedData = query
                .GroupBy(pa => pa.PartNumberId)
                .Select(g => new ProductionAchievementResponseDto.ProductionReportDto
                {
                    PartInfo = new ProductionAchievementResponseDto.PartInfoDto
                    {
                        Number = g.First().PartNumberName,
                        Name = g.First().PartNumberName,
                        Area = g.First().Area,
                        Supervisor = g.First().Supervisor,
                        Leader = g.First().Leader
                    },
                    DailyRecords = g.Select(r => new ProductionAchievementResponseDto.DailyRecordDto
                    {
                        Date = r.ProductionDate,
                        Time = r.WorkingTime,
                        Obj = r.ProductionObjetive,
                        Real = r.ProductionReal
                    }).ToList()
                }).ToList();

            return groupedData;
        }
    }
}

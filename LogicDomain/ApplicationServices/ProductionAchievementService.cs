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
            var query = _temporalContext.ProductionAchievements
                .Where(pa => pa.ProductionDate >= request.StarDate && pa.ProductionDate <= request.EndDate);

            if (!string.IsNullOrEmpty(request.PartNumberName))
            {
                query = query.Where(pa => pa.PartNumberName == request.PartNumberName);
            }

            if (!string.IsNullOrEmpty(request.Area))
            {
                query = query.Where(pa => pa.Area == request.Area);
            }

            if (!string.IsNullOrEmpty(request.Leader))
            {
                query = query.Where(pa => pa.Leader == request.Leader);
            }

            if (!string.IsNullOrEmpty(request.Supervisor))
            {
                query = query.Where(pa => pa.Supervisor == request.Supervisor);
            }
            
            var result = await query.ToListAsync();

            var groupedData = result
                .GroupBy(pa => pa.PartNumberName)
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

        public async Task<ProductionAchievementDashboardDto> GetProcessedAchievementAsync(ProductionAchievementRequestDto request)
        {
            // 1. Fetch data
            var query = _temporalContext.ProductionAchievements.AsNoTracking()
                .Where(pa => pa.ProductionDate >= request.StarDate && pa.ProductionDate <= request.EndDate);
    
            // Filters
            if (!string.IsNullOrEmpty(request.PartNumberName))
            {
                query = query.Where(pa => pa.PartNumberName == request.PartNumberName);
            }
            if (!string.IsNullOrEmpty(request.Area))
            {
                query = query.Where(pa => pa.Area == request.Area);
            }
            if (!string.IsNullOrEmpty(request.Leader))
            {
                query = query.Where(pa => pa.Leader == request.Leader);
            }
            if (!string.IsNullOrEmpty(request.Supervisor))
            {
                query = query.Where(pa => pa.Supervisor == request.Supervisor);
            }

            var flatData = await query.ToListAsync();

            // 2. Initialize response object
            var dashboardData = new ProductionAchievementDashboardDto
            {
                Kpis = new AchievementKpis(),
                Hierarchy = new List<SupervisorAchievementNode>(),
                Parts = new List<PartAchievement>(),
                ChartData = new ChartData
                {
                    DateStats = new Dictionary<string, DateStats>(),
                    AreaDateMap = new Dictionary<string, Dictionary<string, AreaDateSubtotals>>(),
                    ActiveAreas = new List<string>(),
                    Dates = new List<string>()
                }
            };

            if (!flatData.Any()) return dashboardData;
    
            // 3. Process data
            var dateSet = new HashSet<string>();
            var areaSet = new HashSet<string>();

            foreach (var item in flatData)
            {
                var dateKey = item.ProductionDate.ToString("yyyy-MM-dd");
                dateSet.Add(dateKey);
                if (!string.IsNullOrEmpty(item.Area))
                {
                    areaSet.Add(item.Area);
                }

                // Global KPIs
                dashboardData.Kpis.TotalObj += item.ProductionObjetive;
                dashboardData.Kpis.TotalReal += item.ProductionReal;

                // ChartData - DateStats
                if (!dashboardData.ChartData.DateStats.ContainsKey(dateKey))
                    dashboardData.ChartData.DateStats[dateKey] = new DateStats();
        
                var dateStat = dashboardData.ChartData.DateStats[dateKey];
                dateStat.Obj += item.ProductionObjetive;
                dateStat.Real += item.ProductionReal;
                dateStat.TotalCount++;
                if (item.ProductionReal >= item.ProductionObjetive)
                {
                    dateStat.SuccessCount++;
                }

                // ChartData - AreaDateMap
                if (!string.IsNullOrEmpty(item.Area))
                {
                    if (!dashboardData.ChartData.AreaDateMap.ContainsKey(dateKey))
                        dashboardData.ChartData.AreaDateMap[dateKey] = new Dictionary<string, AreaDateSubtotals>();
                    if (!dashboardData.ChartData.AreaDateMap[dateKey].ContainsKey(item.Area))
                        dashboardData.ChartData.AreaDateMap[dateKey][item.Area] = new AreaDateSubtotals();
            
                    var areaSubtotal = dashboardData.ChartData.AreaDateMap[dateKey][item.Area];
                    areaSubtotal.Obj += item.ProductionObjetive;
                    areaSubtotal.Real += item.ProductionReal;
                }
            }
    
            // Finalize KPIs
            dashboardData.Kpis.Diff = dashboardData.Kpis.TotalReal - dashboardData.Kpis.TotalObj;
            dashboardData.Kpis.Ach = dashboardData.Kpis.TotalObj > 0 ? (float)dashboardData.Kpis.TotalReal / dashboardData.Kpis.TotalObj : 0;

            // Finalize ChartData
            dashboardData.ChartData.Dates = dateSet.OrderBy(d => d).ToList();
            dashboardData.ChartData.ActiveAreas = areaSet.ToList();
    
            // 4. Hierarchy
            var supervisorGroups = flatData.Where(i => !string.IsNullOrEmpty(i.Supervisor)).GroupBy(i => i.Supervisor);
            foreach (var superGroup in supervisorGroups)
            {
                var superTotalObj = superGroup.Sum(i => i.ProductionObjetive);
                var superTotalReal = superGroup.Sum(i => i.ProductionReal);
                var superNode = new SupervisorAchievementNode
                {
                    Name = superGroup.Key,
                    Obj = superTotalObj,
                    Real = superTotalReal,
                    Ach = superTotalObj > 0 ? (float)superTotalReal / superTotalObj : 0,
                    Leaders = new List<LeaderAchievementNode>()
                };

                var leaderGroups = superGroup.Where(i => !string.IsNullOrEmpty(i.Leader)).GroupBy(i => i.Leader);
                foreach (var leaderGroup in leaderGroups)
                {
                    var leaderTotalObj = leaderGroup.Sum(i => i.ProductionObjetive);
                    var leaderTotalReal = leaderGroup.Sum(i => i.ProductionReal);
                    superNode.Leaders.Add(new LeaderAchievementNode
                    {
                        Name = leaderGroup.Key,
                        Obj = leaderTotalObj,
                        Real = leaderTotalReal,
                        Ach = leaderTotalObj > 0 ? (float)leaderTotalReal / leaderTotalObj : 0
                    });
                }
                superNode.Leaders = superNode.Leaders.OrderByDescending(l => l.Ach).ToList();
                dashboardData.Hierarchy.Add(superNode);
            }
            dashboardData.Hierarchy = dashboardData.Hierarchy.OrderByDescending(s => s.Ach).ToList();

            // 5. Parts
            var partGroups = flatData.Where(i => i.ProductionObjetive > 0 && !string.IsNullOrEmpty(i.PartNumberName)).GroupBy(i => i.PartNumberName);
            foreach(var partGroup in partGroups)
            {
                var partTotalObj = partGroup.Sum(i => i.ProductionObjetive);
                var partTotalReal = partGroup.Sum(i => i.ProductionReal);
                dashboardData.Parts.Add(new PartAchievement
                {
                    Name = partGroup.Key,
                    Obj = partTotalObj,
                    Real = partTotalReal,
                    Ach = partTotalObj > 0 ? (float)partTotalReal / partTotalObj : 0
                });
            }
            dashboardData.Parts = dashboardData.Parts.OrderByDescending(p => p.Ach).ToList();

            return dashboardData;
        }
    }
}

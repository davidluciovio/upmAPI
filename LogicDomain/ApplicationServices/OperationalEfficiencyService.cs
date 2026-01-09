using Entity.AplicationDtos._02_OperationalEfficiencyDtos;
using Entity.Models._05_Temporal;
using LogicData.Context;
using Microsoft.EntityFrameworkCore;

namespace LogicDomain.ApplicationServices
{
    public class OperationalEfficiencyService
    {
        private readonly TemporalContext _temporalContext;

        public OperationalEfficiencyService(TemporalContext temporalContext)
        {
            _temporalContext = temporalContext;
        }

        public async Task<OperationalEfficiencyResponseDto.ProductionResponse> GetGroupedProductionAsync(OperationalEfficiencyRequestDto request)
        {
            // 1. Obtención de datos AsNoTracking para alto rendimiento
            var query = _temporalContext.OperationalEfficiencies.AsNoTracking().Where(x => x.Active == true);

            if (request.StartDate.HasValue)
            {
                query = query.Where(x => x.ProductionDate >= request.StartDate.Value);
            }

            if (request.EndDate.HasValue)
            {
                query = query.Where(x => x.ProductionDate <= request.EndDate.Value);
            }

            if (!string.IsNullOrEmpty(request.Leader))
            {
                query = query.Where(x => x.Leader == request.Leader);
            }

            if (!string.IsNullOrEmpty(request.PartNumberName))
            {
                query = query.Where(x => x.PartNumberName == request.PartNumberName);
            }

            if (!string.IsNullOrEmpty(request.Area))
            {
                query = query.Where(x => x.Area == request.Area);
            }

            if (!string.IsNullOrEmpty(request.Supervisor))
            {
                query = query.Where(x => x.Supervisor == request.Supervisor);
            }

            if (!string.IsNullOrEmpty(request.Shift))
            {
                query = query.Where(x => x.Shift == request.Shift);
            }

            var flatData = await query.Where(p => p.OperativityPercent != 0).ToListAsync();

            // 2. Agrupación Multinivel: Lider > Parte > Area > Supervisor
            var grouped = flatData
                .GroupBy(l => l.Leader)
                .ToDictionary(
                    leaderGrp => leaderGrp.Key ?? "SIN LIDER",
                    leaderGrp => leaderGrp
                        .GroupBy(p => p.PartNumberName)
                        .ToDictionary(
                            partGrp => partGrp.Key ?? "SIN PARTE",
                            partGrp => partGrp
                                .GroupBy(a => a.Area)
                                .ToDictionary(
                                    areaGrp => areaGrp.Key ?? "SIN AREA",
                                    areaGrp => areaGrp
                                        .GroupBy(s => s.Supervisor)
                                        .ToDictionary(
                                            superGrp => superGrp.Key ?? "SIN SUPERVISOR",
                                            superGrp => superGrp.Select(item => MapToDetailDto(item)).ToList()
                                        )
                                )
                        )
                );

            return new OperationalEfficiencyResponseDto.ProductionResponse(grouped);
        }

        // CORREGIDO: El tipo de retorno debe ser ProductionDetailDto
        private static OperationalEfficiencyResponseDto.ProductionDetailDto MapToDetailDto(OperationalEfficiency item)
        {
            return new OperationalEfficiencyResponseDto.ProductionDetailDto(
                ProductionDate: item.ProductionDate,
                Shift: item.Shift.ToString(),
                Active: item.Active, // Asumiendo que item.Active ya es bool por el .Where anterior
                Metrics: new OperationalEfficiencyResponseDto.MetricsDto(
                    Hp: item.HP,
                    Neck: item.Neck,
                    RealTime: item.RealTime,
                    ProductionReal: item.PriductionReal,
                    TotalTime: item.TotalTime,
                    RealWorkingTime: item.RealWorkingTime,
                    OperativityPercent: item.OperativityPercent,
                    DowntimePercent: item.DowntimePercent,
                    TotalDowntime: item.TotalDowntime,
                    NoReportedTime: item.NoReportedTime
                )
            );
        }

        public async Task<OperationalEfficiencyDashboardDto> GetProcessedEfficiencyAsync(OperationalEfficiencyRequestDto request)
        {
            // 1. Fetch data
            var query = _temporalContext.OperationalEfficiencies.AsNoTracking().Where(x => x.Active == true);

            if (request.StartDate.HasValue)
            {
                query = query.Where(x => x.ProductionDate >= request.StartDate.Value.Date);
            }
            if (request.EndDate.HasValue)
            {
                query = query.Where(x => x.ProductionDate <= request.EndDate.Value.Date);
            }
            if (!string.IsNullOrEmpty(request.Leader))
            {
                query = query.Where(x => x.Leader == request.Leader);
            }
            if (!string.IsNullOrEmpty(request.PartNumberName))
            {
                query = query.Where(x => x.PartNumberName == request.PartNumberName);
            }
            if (!string.IsNullOrEmpty(request.Area))
            {
                query = query.Where(x => x.Area == request.Area);
            }
            if (!string.IsNullOrEmpty(request.Supervisor))
            {
                query = query.Where(x => x.Supervisor == request.Supervisor);
            }
            if (!string.IsNullOrEmpty(request.Shift))
            {
                query = query.Where(x => x.Shift == request.Shift);
            }

            var flatData = await query.ToListAsync();

            // 2. Initialize response object and accumulators
            var dashboardData = new OperationalEfficiencyDashboardDto
            {
                Stats = new StatsPayload
                {
                    Kpis = new DashboardKpis(),
                    DateMap = new Dictionary<string, DateMapAccumulator>(),
                    AreaDateMap = new Dictionary<string, Dictionary<string, DateMapAccumulator>>(),
                    LeaderDateMap = new Dictionary<string, Dictionary<string, DateMapAccumulator>>(),
                    SupervisorDateMap = new Dictionary<string, Dictionary<string, DateMapAccumulator>>(),
                    ShiftMap = new Dictionary<string, DateMapAccumulator>(),
                    LeaderDownMap = new Dictionary<string, float>()
                },
                Dates = new List<string>(),
                Hierarchy = new List<SupervisorHierarchyNode>(),
                Parts = new List<PartKpi>()
            };

            var dateSet = new HashSet<string>();

            // 3. Process data
            foreach (var item in flatData)
            {
                var dateKey = item.ProductionDate.ToString("yyyy-MM-dd");
                dateSet.Add(dateKey);

                // Update global KPIs
                dashboardData.Stats.Kpis.RealWorkingTime += item.RealWorkingTime;
                dashboardData.Stats.Kpis.TotalTime += item.TotalTime;

                // Helper function to update accumulators
                void UpdateAccumulator(DateMapAccumulator acc)
                {
                    acc.Work += item.RealWorkingTime;
                    acc.Total += item.TotalTime;
                    acc.TotalRecs++;
                    if (item.OperativityPercent >= 0.85)
                    {
                        acc.SuccessRecs++;
                    }
                    acc.OperSum += item.OperativityPercent;
                    acc.Count++;
                }

                // dateMap
                if (!dashboardData.Stats.DateMap.ContainsKey(dateKey))
                    dashboardData.Stats.DateMap[dateKey] = new DateMapAccumulator();
                UpdateAccumulator(dashboardData.Stats.DateMap[dateKey]);

                // areaDateMap
                if (!string.IsNullOrEmpty(item.Area))
                {
                    if (!dashboardData.Stats.AreaDateMap.ContainsKey(dateKey))
                        dashboardData.Stats.AreaDateMap[dateKey] = new Dictionary<string, DateMapAccumulator>();
                    if (!dashboardData.Stats.AreaDateMap[dateKey].ContainsKey(item.Area))
                        dashboardData.Stats.AreaDateMap[dateKey][item.Area] = new DateMapAccumulator();
                    UpdateAccumulator(dashboardData.Stats.AreaDateMap[dateKey][item.Area]);
                }
        
                // leaderDateMap
                if (!string.IsNullOrEmpty(item.Leader))
                {
                    if (!dashboardData.Stats.LeaderDateMap.ContainsKey(dateKey))
                        dashboardData.Stats.LeaderDateMap[dateKey] = new Dictionary<string, DateMapAccumulator>();
                    if (!dashboardData.Stats.LeaderDateMap[dateKey].ContainsKey(item.Leader))
                        dashboardData.Stats.LeaderDateMap[dateKey][item.Leader] = new DateMapAccumulator();
                    UpdateAccumulator(dashboardData.Stats.LeaderDateMap[dateKey][item.Leader]);
                }

                // supervisorDateMap
                if (!string.IsNullOrEmpty(item.Supervisor))
                {
                    if (!dashboardData.Stats.SupervisorDateMap.ContainsKey(dateKey))
                        dashboardData.Stats.SupervisorDateMap[dateKey] = new Dictionary<string, DateMapAccumulator>();
                    if (!dashboardData.Stats.SupervisorDateMap[dateKey].ContainsKey(item.Supervisor))
                        dashboardData.Stats.SupervisorDateMap[dateKey][item.Supervisor] = new DateMapAccumulator();
                    UpdateAccumulator(dashboardData.Stats.SupervisorDateMap[dateKey][item.Supervisor]);
                }
        
                // shiftMap
                if (!string.IsNullOrEmpty(item.Shift))
                {
                    if (!dashboardData.Stats.ShiftMap.ContainsKey(item.Shift))
                        dashboardData.Stats.ShiftMap[item.Shift] = new DateMapAccumulator();
                    UpdateAccumulator(dashboardData.Stats.ShiftMap[item.Shift]);
                }
        
                // leaderDownMap
                if (!string.IsNullOrEmpty(item.Leader))
                {
                    if (!dashboardData.Stats.LeaderDownMap.ContainsKey(item.Leader))
                        dashboardData.Stats.LeaderDownMap[item.Leader] = 0;
                    dashboardData.Stats.LeaderDownMap[item.Leader] += item.TotalDowntime;
                }
            }

            dashboardData.Dates = dateSet.OrderBy(d => d).ToList();

            // 4. Calculate Hierarchy
            var supervisorGroups = flatData.Where(i => !string.IsNullOrEmpty(i.Supervisor)).GroupBy(i => i.Supervisor);
            foreach (var superGroup in supervisorGroups)
            {
                var superNode = new SupervisorHierarchyNode
                {
                    Name = superGroup.Key,
                    Operativity = superGroup.Average(i => i.OperativityPercent),
                    Leaders = new List<LeaderHierarchyNode>()
                };

                var leaderGroups = superGroup.Where(i => !string.IsNullOrEmpty(i.Leader)).GroupBy(i => i.Leader);
                foreach (var leaderGroup in leaderGroups)
                {
                    superNode.Leaders.Add(new LeaderHierarchyNode
                    {
                        Name = leaderGroup.Key,
                        Operativity = leaderGroup.Average(i => i.OperativityPercent)
                    });
                }
                superNode.Leaders = superNode.Leaders.OrderByDescending(l => l.Operativity).ToList();
                dashboardData.Hierarchy.Add(superNode);
            }
            dashboardData.Hierarchy = dashboardData.Hierarchy.OrderByDescending(s => s.Operativity).ToList();
    
            // 5. Calculate Parts KPI
            var partGroups = flatData.Where(i => !string.IsNullOrEmpty(i.PartNumberName)).GroupBy(i => i.PartNumberName);
            foreach(var partGroup in partGroups)
            {
                dashboardData.Parts.Add(new PartKpi 
                {
                    Name = partGroup.Key,
                    Operativity = partGroup.Average(i => i.OperativityPercent)
                });
            }
            dashboardData.Parts = dashboardData.Parts.OrderByDescending(p => p.Operativity).ToList();

            return dashboardData;
        }
    }
}
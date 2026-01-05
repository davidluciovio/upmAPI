using Entity.AplicationDtos._03_IntegratedOperativity;
using Entity.Models._05_Temporal;
using LogicData.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace LogicDomain.ApplicationServices
{
    public class IntegratedOperativityService
    {
        private readonly TemporalContext _temporalContext;
        private readonly DataContext _dataContext;

        public IntegratedOperativityService(TemporalContext temporalContext, DataContext dataContext)
        {
            _temporalContext = temporalContext;
            _dataContext = dataContext;
        }

        private class CombinedOperativityData
        {
            public DateTime ProductionDate { get; set; }
            public string Area { get; set; }
            public string Supervisor { get; set; }
            public string Leader { get; set; }
            public string Shift { get; set; }
            public string PartNumberName { get; set; }
            public float WorkingTime { get; set; }
            public float ProductionObjetive { get; set; }
            public float ProductionReal { get; set; }
        }

        public async Task<IntegratedOperativityResponseDto> FilterDataCombined(IntegratedOperativityRequestDto requestDto)
        {
            var achievementsQuery = _temporalContext.ProductionAchievements
                .Where(pa => pa.ProductionDate >= requestDto.GlobalRange.Start && pa.ProductionDate <= requestDto.GlobalRange.End);

            var efficienciesQuery = _temporalContext.OperationalEfficiencies
                .Where(oe => oe.ProductionDate >= requestDto.GlobalRange.Start && oe.ProductionDate <= requestDto.GlobalRange.End);

            if (!string.IsNullOrEmpty(requestDto.Hierarchy.AreaName))
            {
                achievementsQuery = achievementsQuery.Where(pa => pa.Area == requestDto.Hierarchy.AreaName);
                efficienciesQuery = efficienciesQuery.Where(oe => oe.Area == requestDto.Hierarchy.AreaName);
            }

            if (!string.IsNullOrEmpty(requestDto.Hierarchy.SupervisorName))
            {
                achievementsQuery = achievementsQuery.Where(pa => pa.Supervisor == requestDto.Hierarchy.SupervisorName);
                efficienciesQuery = efficienciesQuery.Where(oe => oe.Supervisor == requestDto.Hierarchy.SupervisorName);
            }

            if (!string.IsNullOrEmpty(requestDto.Hierarchy.LeaderName))
            {
                achievementsQuery = achievementsQuery.Where(pa => pa.Leader == requestDto.Hierarchy.LeaderName);
                efficienciesQuery = efficienciesQuery.Where(oe => oe.Leader == requestDto.Hierarchy.LeaderName);
            }

            if (!string.IsNullOrEmpty(requestDto.PartData.PartNumber))
            {
                achievementsQuery = achievementsQuery.Where(pa => pa.PartNumberName == requestDto.PartData.PartNumber);
                efficienciesQuery = efficienciesQuery.Where(oe => oe.PartNumberName == requestDto.PartData.PartNumber);
            }
            
            var combinedQuery = from pa in achievementsQuery
                                join oe in efficienciesQuery on
                                    new { pa.ProductionDate, pa.Area, pa.Supervisor, pa.Leader, pa.Shift, pa.PartNumberName }
                                    equals
                                    new { oe.ProductionDate, oe.Area, oe.Supervisor, oe.Leader, oe.Shift, oe.PartNumberName }
                                select new CombinedOperativityData
                                {
                                    ProductionDate = pa.ProductionDate,
                                    Area = pa.Area,
                                    Supervisor = pa.Supervisor,
                                    Leader = pa.Leader,
                                    Shift = pa.Shift,
                                    PartNumberName = pa.PartNumberName,
                                    ProductionObjetive = pa.ProductionObjetive,
                                    ProductionReal = pa.ProductionReal,
                                    WorkingTime = oe.RealWorkingTime
                                };

            if (requestDto.Operativity.MinAchievement.HasValue && requestDto.Operativity.MinAchievement > 0)
            {
                combinedQuery = combinedQuery.Where(pa => pa.ProductionObjetive > 0 && (pa.ProductionReal * 100 / pa.ProductionObjetive) >= requestDto.Operativity.MinAchievement.Value);
            }

            var resultData = await combinedQuery.ToListAsync();

            return MapToIntegratedResponseFromCombined(resultData, requestDto.GlobalRange.Start, requestDto.GlobalRange.End);
        }

        private IntegratedOperativityResponseDto MapToIntegratedResponseFromCombined(List<CombinedOperativityData> source, DateTime startDate, DateTime endDate)
        {
            var partNumberNames = source.Select(s => s.PartNumberName).Distinct().ToList();
            var partNumberDescriptions = _dataContext.ProductionPartNumbers
                                                .Where(p => partNumberNames.Contains(p.PartNumberName))
                                                .ToDictionary(p => p.PartNumberName, p => p.PartNumberDescription);
            var response = new IntegratedOperativityResponseDto
            {
                StartDateTime = startDate,
                EndDateTime = endDate,
                ShiftDataList = source
                    .GroupBy(x => x.Shift)
                    .Select(shiftGroup => new IntegratedOperativityResponseDto.ShiftData
                    {
                        ShiftName = shiftGroup.Key,
                        Summary = CalculateSummaryFromCombined(shiftGroup),
                        AreaDataList = shiftGroup
                            .GroupBy(x => x.Area)
                            .Select(areaGroup => new IntegratedOperativityResponseDto.AreaData
                            {
                                AreaName = areaGroup.Key,
                                Summary = CalculateSummaryFromCombined(areaGroup),
                                OperativityDataList = CalculateDailyOperativityFromCombined(areaGroup),
                                SupervisorDataList = areaGroup
                                    .GroupBy(x => x.Supervisor)
                                    .Select(supGroup => new IntegratedOperativityResponseDto.SupervisorData
                                    {
                                        SupervisorName = supGroup.Key,
                                        Summary = CalculateSummaryFromCombined(supGroup),
                                        OperativityDataList = CalculateDailyOperativityFromCombined(supGroup),
                                        LeaderDataList = supGroup
                                            .GroupBy(x => x.Leader)
                                            .Select(leadGroup => new IntegratedOperativityResponseDto.LeaderData
                                            {
                                                LeaderName = leadGroup.Key,
                                                Summary = CalculateSummaryFromCombined(leadGroup),
                                                OperativityDataList = CalculateDailyOperativityFromCombined(leadGroup),
                                                PartNumberDataList = leadGroup
                                                    .GroupBy(x => x.PartNumberName)
                                                    .Select(pnGroup => new IntegratedOperativityResponseDto.PartNumberData
                                                    {
                                                        PartNumberName = pnGroup.Key,
                                                        PartNumberDescription = partNumberDescriptions.GetValueOrDefault(pnGroup.Key, "Description not found"),
                                                        Summary = CalculateSummaryFromCombined(pnGroup),
                                                        OperativityDataList = CalculateDailyOperativityFromCombined(pnGroup)
                                                    }).ToList()
                                            }).ToList()
                                    }).ToList()
                            }).ToList()
                    }).ToList()
            };
            return response;
        }

        private IntegratedOperativityResponseDto.SummaryData CalculateSummaryFromCombined(IEnumerable<CombinedOperativityData> items)
        {
            var totalReal = items.Sum(x => x.ProductionReal);
            var totalObjetive = items.Sum(x => x.ProductionObjetive);
            var totalHours = items.Sum(x => x.WorkingTime);

            return new IntegratedOperativityResponseDto.SummaryData
            {
                TotalWorkingTime = TimeSpan.FromHours(totalHours),
                TotalRealProduction = (int)totalReal,
                TotalObjetiveProduction = (int)totalObjetive,
                AverageAchievement = totalObjetive > 0 ? (totalReal / totalObjetive) * 100 : 0
            };
        }

        private List<IntegratedOperativityResponseDto.OperativityData> CalculateDailyOperativityFromCombined(IEnumerable<CombinedOperativityData> items)
        {
            return items
                .GroupBy(x => x.ProductionDate.Date)
                .Select(dayGroup =>
                {
                    var real = dayGroup.Sum(x => x.ProductionReal);
                    var objetive = dayGroup.Sum(x => x.ProductionObjetive);
                    var hours = dayGroup.Sum(x => x.WorkingTime);

                    return new IntegratedOperativityResponseDto.OperativityData
                    {
                        Date = DateOnly.FromDateTime(dayGroup.Key),
                        WorkingTime = TimeSpan.FromHours(hours),
                        RealProduction = (int)real,
                        ObjetiveProduction = (int)objetive,
                        Achievement = objetive > 0 ? (real / objetive) * 100 : 0
                    };
                })
                .OrderBy(x => x.Date)
                .ToList();
        }
    }
}
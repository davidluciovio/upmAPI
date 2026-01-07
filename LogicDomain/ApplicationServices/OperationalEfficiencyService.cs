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
    }
}
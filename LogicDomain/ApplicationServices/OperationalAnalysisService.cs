using Entity.AplicationDtos.OperationalAnalysis;
using LogicData.Context;
using Microsoft.EntityFrameworkCore;

namespace LogicDomain.ApplicationServices
{
    public class OperationalAnalysisService
    {
        private readonly TemporalContext _temporalContext;
        public OperationalAnalysisService(TemporalContext temporalContext)
        {
            _temporalContext = temporalContext;
        }

        public async Task<OperationalAnalysisFiltersDataDto> GetFiltersData()
        { 
            var leaders = await _temporalContext.OperationalEfficiencies
                .Select(x => x.Leader)
                .Distinct()
                .ToListAsync();
            var partNumbers = await _temporalContext.OperationalEfficiencies
                .Select(x => x.PartNumberName)
                .Distinct()
                .ToListAsync();
            var areas = await _temporalContext.OperationalEfficiencies
                .Select(x => x.Area)
                .Distinct()
                .ToListAsync();
            var supervisors = await _temporalContext.OperationalEfficiencies
                .Select(x => x.Supervisor)
                .Distinct()
                .ToListAsync();
            var shifts = await _temporalContext.OperationalEfficiencies
                .Select(x => x.Shift)
                .Distinct()
                .ToListAsync();

            var result = new OperationalAnalysisFiltersDataDto
            {
                Leaders = leaders,
                PartNumbers = partNumbers,
                Areas = areas,
                Supervisors = supervisors,
                Shifts = shifts
            };

            return result;
        }

        public async Task<OperationalAnalysisResponseDto> GetOperationalAnalysisData(OperationalAnalysisRequestDto request)
        {
            var query = _temporalContext.OperationalEfficiencies.AsQueryable();

            if (request.Leaders.Any())
            {
                query = query.Where(x => request.Leaders.Contains(x.Leader));
            }

            if (request.PartNumbers.Any())
            {
                query = query.Where(x => request.PartNumbers.Contains(x.PartNumberName));
            }

            if (request.Areas.Any())
            {
                query = query.Where(x => request.Areas.Contains(x.Area));
            }

            if (request.Supervisors.Any())
            {
                query = query.Where(x => request.Supervisors.Contains(x.Supervisor));
            }

            if (request.Shifts.Any())
            {
                query = query.Where(x => request.Shifts.Contains(x.Shift));
            }

            var groupedData = await query
                .GroupBy(x => new { x.Leader, x.PartNumberName, x.Area, x.Supervisor, x.Shift })
                .Select(g => new OperationalAnalysisItemDto
                {
                    Leader = g.Key.Leader,
                    PartNumberName = g.Key.PartNumberName,
                    Area = g.Key.Area,
                    Supervisor = g.Key.Supervisor,
                    Shift = g.Key.Shift,
                    AverageOperativityPercent = g.Average(x => x.OperativityPercent),
                    TotalDowntime = g.Sum(x => x.TotalDowntime)
                })
                .ToListAsync();

            return new OperationalAnalysisResponseDto
            {
                AnalysisResults = groupedData
            };
        }
    }
}

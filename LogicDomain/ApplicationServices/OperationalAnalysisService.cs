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

        public async Task<OperationalAnlysisFiltersDataDto> GetFiltersData()
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

            var result = new OperationalAnlysisFiltersDataDto
            {
                Leaders = leaders,
                PartNumbers = partNumbers,
                Areas = areas,
                Supervisors = supervisors,
                Shifts = shifts
            };

            return result;
        }
    }
}

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

        public async Task<OperationalAnalysisRequestDto> GetFiltersData()
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

            var result = new OperationalAnalysisRequestDto
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
            // 1. Construimos la query base (esto no ejecuta nada todavía)
            var query = _temporalContext.OperationalEfficiencies
                .Where(data => data.ProductionDate >= request.StartDate && data.ProductionDate <= request.EndDate)
                .AsQueryable();

            // Filtros dinámicos
            if (request.Leaders != null && request.Leaders.Any())
                query = query.Where(x => request.Leaders.Contains(x.Leader));

            if (request.PartNumbers != null && request.PartNumbers.Any())
                query = query.Where(x => request.PartNumbers.Contains(x.PartNumberName));

            if (request.Areas != null && request.Areas.Any())
                query = query.Where(x => request.Areas.Contains(x.Area));

            if (request.Supervisors != null && request.Supervisors.Any())
                query = query.Where(x => request.Supervisors.Contains(x.Supervisor));

            if (request.Shifts != null && request.Shifts.Any())
                query = query.Where(x => request.Shifts.Contains(x.Shift));

            // 2. KPI Cards
            var kpiCardsData = await query
                .GroupBy(g => g.Area)
                .Select(data => new OperationalAnalysisResponseDto.KPICardsData
                {
                    Area = data.Key,
                    Operativity = data.Average(x => x.OperativityPercent)
                })
                .ToListAsync();

            // 3. Tabla Supervisores (Nota: Agregué AsSplitQuery si traes muchos datos anidados, opcional)
            var tableSupervisorsData = await query
                .GroupBy(g => new { g.Supervisor, g.Area, g.Leader })
                .Select(data => new
                {
                    // Proyección intermedia para evitar problemas de traducción compleja
                    data.Key.Supervisor,
                    data.Key.Area,
                    data.Key.Leader,
                    Operativity = data.Average(x => x.OperativityPercent)
                })
                .ToListAsync();

            // Agrupamos en memoria para dar formato de jerarquía (Supervisor -> Leaders)
            var formattedSupervisorsData = tableSupervisorsData
                .GroupBy(x => new { x.Supervisor, x.Area })
                .Select(g => new OperationalAnalysisResponseDto.SupervisorOperativityData
                {
                    Supervisor = g.Key.Supervisor,
                    Area = g.Key.Area,
                    Operativity = g.Average(x => x.Operativity), // Promedio del supervisor global
                    Leaders = g.Select(l => new OperationalAnalysisResponseDto.SupervisorOperativityData.LeaderOperativityData
                    {
                        Leader = l.Leader,
                        Operativity = l.Operativity
                    }).ToList()
                }).ToList();

            // 4. Tabla Part Numbers
            var tablePartNumberData = await query
                .GroupBy(g => new { g.PartNumberName, g.Area, g.Supervisor, g.Leader })
                .Select(data => new OperationalAnalysisResponseDto.PartNumberOperativityData
                {
                    PartNumber = data.Key.PartNumberName,
                    Area = data.Key.Area,
                    Supervisor = data.Key.Supervisor,
                    Leader = data.Key.Leader,
                    Operativity = data.Average(x => x.OperativityPercent)
                })
                .ToListAsync();

            // 5. Trend Chart (Días) - OPTIMIZADO
            // Problema original: Intentabas iterar 'days' (lista local) dentro de una query SQL. EF Core suele fallar o hacerlo muy lento.
            // Solución: Traemos los datos crudos agrupados por fecha y rellenamos los ceros en memoria (C#).

            // Paso A: Traer datos existentes de la DB
            var rawTrendData = await query
                .GroupBy(g => new { g.Area, g.ProductionDate })
                .Select(g => new
                {
                    Area = g.Key.Area,
                    Date = g.Key.ProductionDate,
                    AvgOperativity = g.Average(x => x.OperativityPercent)
                })
                .ToListAsync();

            // Paso B: Generar rango de días completo
            var allDays = new List<DateTime>();
            for (var date = request.StartDate; date <= request.EndDate; date = date.AddDays(1))
            {
                allDays.Add(date); // Asumiendo que ProductionDate en DB no tiene horas (Time 00:00:00)
            }

            // Paso C: Cruzar datos en memoria (Cross Join)
            var distinctAreas = rawTrendData.Select(x => x.Area).Distinct().ToList();

            var areaOperativityDayTrendData = distinctAreas.Select(area => new OperationalAnalysisResponseDto.AreaOperativityDayTrend
            {
                Area = area,
                DayOperativities = allDays.Select(day => new OperationalAnalysisResponseDto.AreaOperativityDayTrend.DayOperativity
                {
                    Day = day,
                    // Buscamos en memoria si hay dato para ese día/área, si no ponemos 0
                    Operativity = rawTrendData
                        .FirstOrDefault(r => r.Area == area && r.Date.Date == day.Date) // .Date para asegurar
                        ?.AvgOperativity ?? 0
                }).ToList()
            }).ToList();


            // 6. Armar respuesta
            return new OperationalAnalysisResponseDto
            {
                Cards = kpiCardsData,
                Supervisors = formattedSupervisorsData,
                PartNumbers = tablePartNumberData,
                AreaOperativityDayTrends = areaOperativityDayTrendData
            };
        }
    }
}

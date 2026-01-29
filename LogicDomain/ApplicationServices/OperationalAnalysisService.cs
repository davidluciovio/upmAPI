using Entity.AplicationDtos.OperationalAnalysis;
using LogicData.Context;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Runtime.CompilerServices;

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

            var currentYear = DateTime.Now.Year;
            var startOfAnnualRange = new DateTime(currentYear - 1, 12, 1); // 1 de Diciembre del año anterior
            var endOfAnnualRange = new DateTime(currentYear, 12, 31);      // 31 de Diciembre del año actual
            // ---------------------------------------------------------
            // 1. Construcción de Query Base y Filtros
            // ---------------------------------------------------------
            var query = _temporalContext.OperationalEfficiencies
                .Where(data => data.ProductionDate >= request.StartDate && data.ProductionDate <= request.EndDate)
                .AsQueryable();

            // Rango de Días (para gráficas diarias)
            var allDays = new List<DateTime>();
            for (var date = request.StartDate; date <= request.EndDate; date = date.AddDays(1))
            {
                allDays.Add(date);
            }

            // Rango de Meses (para la NUEVA gráfica anual)
            // Calculamos el primer día del mes de inicio y el primer día del mes de fin
            var startMonth = new DateTime(request.StartDate.Year, request.StartDate.Month, 1);
            var endMonth = new DateTime(request.EndDate.Year, request.EndDate.Month, 1);
            var allMonths = new List<DateTime>();

            for (var date = startOfAnnualRange; date <= endOfAnnualRange; date = date.AddMonths(1))
            {
                allMonths.Add(date);
            }

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

            // ---------------------------------------------------------
            // 2. KPI Cards
            // ---------------------------------------------------------
            var kpiCardsData = await query
                .GroupBy(g => g.Area)
                .Select(data => new OperationalAnalysisResponseDto.KPICardsData
                {
                    Area = data.Key,
                    Operativity = data.Average(x => x.OperativityPercent)
                })
                .OrderBy(data => data.Area)
                .ToListAsync();

            // ---------------------------------------------------------
            // 3. Tabla Supervisores
            // ---------------------------------------------------------
            var tableSupervisorsData = await query
                .GroupBy(g => new { g.Supervisor, g.Area, g.Leader })
                .Select(data => new
                {
                    data.Key.Supervisor,
                    data.Key.Area,
                    data.Key.Leader,
                    Operativity = data.Average(x => x.OperativityPercent)
                })
                .ToListAsync();

            var formattedSupervisorsData = tableSupervisorsData
                .GroupBy(x => new { x.Supervisor, x.Area })
                .Select(g => new OperationalAnalysisResponseDto.SupervisorOperativityData
                {
                    Supervisor = g.Key.Supervisor,
                    Area = g.Key.Area,
                    Operativity = g.Average(x => x.Operativity),
                    Leaders = g.Select(l => new OperationalAnalysisResponseDto.SupervisorOperativityData.LeaderOperativityData
                    {
                        Leader = l.Leader,
                        Operativity = l.Operativity
                    }).ToList()
                }).ToList();

            // ---------------------------------------------------------
            // PREPARACIÓN DE DATOS CRUDOS (Optimización)
            // ---------------------------------------------------------

            // A. Datos Diarios (PartNumber)
            var rawPartNumberTrendData = await query
                .GroupBy(g => new { g.PartNumberName, g.ProductionDate })
                .Select(g => new
                {
                    PartNumber = g.Key.PartNumberName,
                    Date = g.Key.ProductionDate,
                    AvgOperativity = g.Average(x => x.OperativityPercent)
                })
                .ToListAsync();

            // B. Datos Diarios (Supervisor)
            var rawSupervisorTrendData = await query
                .GroupBy(g => new { g.Supervisor, g.Leader, g.ProductionDate })
                .Select(g => new
                {
                    Supervisor = g.Key.Supervisor,
                    Leader = g.Key.Leader,
                    Date = g.Key.ProductionDate,
                    AvgOperativity = g.Average(x => x.OperativityPercent)
                })
                .ToListAsync();

            // C. Datos Diarios (Area)
            var rawAreaTrendData = await query
                .GroupBy(g => new { g.Area, g.ProductionDate })
                .Select(g => new
                {
                    Area = g.Key.Area,
                    Date = g.Key.ProductionDate,
                    AvgOperativity = g.Average(x => x.OperativityPercent)
                })
                .ToListAsync();

            // ---------------------------------------------------------
            // 4. Tabla Part Numbers
            // ---------------------------------------------------------
            var partNumberHeaders = await query
                .GroupBy(g => new { g.PartNumberName, g.Area})
                .Select(data => new
                {
                    data.Key.PartNumberName,
                    data.Key.Area,
                    data.FirstOrDefault()!.Supervisor,
                    data.FirstOrDefault()!.Leader,
                    AvgOperativity = data.Average(x => x.OperativityPercent)
                })
                .ToListAsync();

            var trendsLookup = rawPartNumberTrendData.ToLookup(x => x.PartNumber);

            var tablePartNumberData = partNumberHeaders
                .Select(header => new OperationalAnalysisResponseDto.PartNumberOperativityData
                {
                    PartNumber = header.PartNumberName,
                    Area = header.Area,
                    Supervisor = header.Supervisor,
                    Leader = header.Leader,
                    Operativity = header.AvgOperativity,
                    DayOperativities = trendsLookup[header.PartNumberName]
                        .Select(r => new OperationalAnalysisResponseDto.DayOperativity
                        {
                            Day = r.Date,
                            Operativity = r.AvgOperativity
                        })
                        .ToList()
                })
                .ToList();

            // ---------------------------------------------------------
            // 5. Trend Charts (Diarios - Relleno de ceros)
            // ---------------------------------------------------------

            // A. Area Trends (Diario)
            var distinctAreas = rawAreaTrendData.Select(x => x.Area).Distinct().ToList();

            var areaOperativityDayTrendData = distinctAreas.Select(area => new OperationalAnalysisResponseDto.AreaOperativityDayTrend
            {
                Area = area,
                DayOperativities = allDays.Select(day => new OperationalAnalysisResponseDto.DayOperativity
                {
                    Day = day,
                    Operativity = rawAreaTrendData
                        .FirstOrDefault(r => r.Area == area && r.Date.Date == day.Date)
                        ?.AvgOperativity ?? 0
                }).ToList()
            }).ToList();

            // B. Supervisor Heatmaps (Diario)
            var supervisors = rawSupervisorTrendData.Select(x => x.Supervisor).Distinct().ToList();

            var supervisorOperativityDayHeatMap = supervisors.Select(supervisor => new OperationalAnalysisResponseDto.SupervisorOperativityDayHeatMap
            {
                Supervisor = supervisor,
                DayOperativities = allDays.Select(day => new OperationalAnalysisResponseDto.DayOperativity
                {
                    Day = day,
                    Operativity = rawSupervisorTrendData
                        .FirstOrDefault(r => r.Supervisor == supervisor && r.Date.Date == day.Date)
                        ?.AvgOperativity ?? 0,
                }).ToList(),
                Leaders = rawSupervisorTrendData
                    .Where(x => x.Supervisor == supervisor)
                    .Select(x => x.Leader)
                    .Distinct()
                    .Select(leader => new OperationalAnalysisResponseDto.SupervisorOperativityDayHeatMap.LeaderOperativityData
                    {
                        Leader = leader,
                        DayOperativities = allDays.Select(day => new OperationalAnalysisResponseDto.DayOperativity
                        {
                            Day = day,
                            Operativity = rawSupervisorTrendData
                                .FirstOrDefault(r => r.Supervisor == supervisor && r.Leader == leader && r.Date.Date == day.Date)
                                ?.AvgOperativity ?? 0,
                        }).ToList()
                    }).ToList()
            }).ToList();

            // ---------------------------------------------------------
            // 6. NUEVA SECCIÓN: Tendencias Mensuales (Anuales)
            // ---------------------------------------------------------

            // Paso A: Traer datos agrupados por Año y Mes desde DB
            // 1. Calculamos las fechas dinámicamente
            

            // 2. Usamos estas fechas en el Where en lugar de las del 'request'
            var rawMonthlyData = await _temporalContext.OperationalEfficiencies
                .Where(data => data.ProductionDate >= startOfAnnualRange && data.ProductionDate <= endOfAnnualRange)
                .GroupBy(x => new { x.Area, x.ProductionDate.Year, x.ProductionDate.Month })
                .Select(g => new
                {
                    Area = g.Key.Area,
                    Year = g.Key.Year,
                    Month = g.Key.Month,
                    AvgOperativity = g.Average(x => x.OperativityPercent)
                })
                .ToListAsync();

            // Paso B: Cruzar en memoria con la lista completa de meses (allMonths)
            // Usamos 'distinctAreas' que ya obtuvimos en el paso 5
            var annualAreaTrends = distinctAreas.Select(area => new OperationalAnalysisResponseDto.AnnualAreaTrend
            {
                Area = area,
                Months = allMonths.Select(m => new OperationalAnalysisResponseDto.MonthOperativity
                {
                    Year = m.Year,
                    Month = m.Month,
                    MonthName = System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(m.Month), // Ej: "Ene", "Feb" (según idioma del servidor)
                    Operativity = rawMonthlyData
                        .FirstOrDefault(r => r.Area == area && r.Year == m.Year && r.Month == m.Month)
                        ?.AvgOperativity ?? 0
                }).ToList()
            }).ToList();

            // ---------------------------------------------------------
            // 7. Retorno Final
            // ---------------------------------------------------------
            return new OperationalAnalysisResponseDto
            {
                Cards = kpiCardsData,
                Supervisors = formattedSupervisorsData,
                PartNumbers = tablePartNumberData,
                AreaOperativityDayTrends = areaOperativityDayTrendData,
                SupervisorOperativityDayHeatMaps = supervisorOperativityDayHeatMap,
                AnnualAreaTrends = annualAreaTrends // <--- Nuevo campo
            };
        }

        public async IAsyncEnumerable<string> GetOperationalAnalysisStream([EnumeratorCancellation] CancellationToken cancellationToken)
        {
            var startInfo = new ProcessStartInfo
            {
                FileName = @"\\upmap11\c$\UPM\ProductionData\XSLXtoCSV.exe",
                Arguments = "",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using var process = new Process { StartInfo = startInfo };

            process.Start();

            // Leemos la salida línea por línea en lugar de esperar al final
            while (!process.StandardOutput.EndOfStream)
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    process.Kill();
                    yield break;
                }

                string? line = await process.StandardOutput.ReadLineAsync();
                if (line != null)
                {
                    yield return line; // Enviamos la línea al cliente inmediatamente
                }
            }

            // También capturamos errores si los hay
            if (!process.StandardError.EndOfStream)
            {
                string error = await process.StandardError.ReadToEndAsync();
                if (!string.IsNullOrEmpty(error)) yield return $"[ERROR]: {error}";
            }

            await process.WaitForExitAsync(cancellationToken);
        }
    }
}
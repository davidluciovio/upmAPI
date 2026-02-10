using Entity.Dtos.AplicationDtos.OperationalAnalysis;
using LogicData.Context;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Reflection.PortableExecutable;
using System.Runtime.CompilerServices;
using System.Security;
using static Entity.Dtos.AplicationDtos.OperationalAnalysis.OperationalAnalysisResponseDto;

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
            var managments = await _temporalContext.OperationalEfficiencies
                .Select(x => x.Managment)
                .Distinct()
                .ToListAsync();
            var partNumbers = await _temporalContext.OperationalEfficiencies
                .Select(x => x.PartNumberName)
                .Distinct()
                .ToListAsync();
            var shifts = await _temporalContext.OperationalEfficiencies
                .Select(x => x.Shift)
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
            var jefes = await _temporalContext.OperationalEfficiencies
                .Select(x => x.Jefe)
                .Distinct()
                .ToListAsync();

            var result = new OperationalAnalysisRequestDto
            {
                Leaders = leaders,
                Managments = managments,
                Areas = areas,
                Supervisors = supervisors,
                Jefes = jefes,
                PartNumbers = partNumbers,
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

            if (request.Managments != null && request.Managments.Any())
                query = query.Where(x => request.Managments.Contains(x.Managment));

            if (request.Areas != null && request.Areas.Any())
                query = query.Where(x => request.Areas.Contains(x.Area));

            if (request.Supervisors != null && request.Supervisors.Any())
                query = query.Where(x => request.Supervisors.Contains(x.Supervisor));

            if (request.Jefes != null && request.Jefes.Any())
                query = query.Where(x => request.Jefes.Contains(x.Jefe));

            if (request.PartNumbers != null && request.PartNumbers.Any())
                query = query.Where(x => request.PartNumbers.Contains(x.PartNumberName));

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
            var tableRawData = await query
                .GroupBy(g => new
                {
                    g.Managment,
                    g.Area,
                    g.Jefe,
                    g.Supervisor,
                    g.Leader
                })
                .Select(data => new
                {
                    data.Key.Managment,
                    data.Key.Area,
                    data.Key.Jefe,
                    data.Key.Supervisor,
                    data.Key.Leader,
                    // Calculamos el promedio base por líder
                    Operativity = data.Average(x => x.OperativityPercent)
                })
                .ToListAsync();

            var formattedManagmentData = tableRawData
                // Nivel 1: Agrupar por Gerencia y Area
                .GroupBy(x => new { x.Managment, x.Area })
                .Select(gMgmt => new ManagmentOperativityData
                {
                    Managment = gMgmt.Key.Managment,
                    Area = gMgmt.Key.Area,
                    // Promedio de toda la Gerencia
                    Operativity = gMgmt.Average(x => x.Operativity),

                    Jefes = gMgmt
                        // Nivel 2: Agrupar por Jefe
                        .GroupBy(x => x.Jefe)
                        .Select(gJefe => new ManagmentOperativityData.JefeOperativityData
                        {
                            Jefe = gJefe.Key,
                            // Promedio de toda la Jefatura
                            Operativity = gJefe.Average(x => x.Operativity),

                            Supervisors = gJefe
                                // Nivel 3: Agrupar por Supervisor
                                .GroupBy(x => x.Supervisor)
                                .Select(gSup => new ManagmentOperativityData.JefeOperativityData.SupervisorOperativityData
                                {
                                    Supervisor = gSup.Key,
                                    // Promedio de todo el Supervisor
                                    Operativity = gSup.Average(x => x.Operativity),

                                    Leaders = gSup
                                        // Nivel 4: Lista de Líderes (hojas del árbol)
                                        .Select(l => new ManagmentOperativityData.JefeOperativityData.SupervisorOperativityData.LeaderOperativityData
                                        {
                                            Leader = l.Leader,
                                            Operativity = l.Operativity
                                        }).ToList()
                                }).ToList()
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
                .GroupBy(g => new { g.PartNumberName, g.Area })
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

            //var supervisorOperativityDayHeatMap = supervisors.Select(supervisor => new OperationalAnalysisResponseDto.SupervisorOperativityDayHeatMap
            //{
            //    Supervisor = supervisor,
            //    DayOperativities = allDays.Select(day => new OperationalAnalysisResponseDto.DayOperativity
            //    {
            //        Day = day,
            //        Operativity = rawSupervisorTrendData
            //            .FirstOrDefault(r => r.Supervisor == supervisor && r.Date.Date == day.Date)
            //            ?.AvgOperativity ?? 0,
            //    }).ToList(),
            //    Leaders = rawSupervisorTrendData
            //        .Where(x => x.Supervisor == supervisor)
            //        .Select(x => x.Leader)
            //        .Distinct()
            //        .Select(leader => new OperationalAnalysisResponseDto.SupervisorOperativityDayHeatMap.LeaderOperativityData
            //        {
            //            Leader = leader,
            //            DayOperativities = allDays.Select(day => new OperationalAnalysisResponseDto.DayOperativity
            //            {
            //                Day = day,
            //                Operativity = rawSupervisorTrendData
            //                    .FirstOrDefault(r => r.Supervisor == supervisor && r.Leader == leader && r.Date.Date == day.Date)
            //                    ?.AvgOperativity ?? 0,
            //            }).ToList()
            //        }).ToList()
            //}).ToList();

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
                Managments = formattedManagmentData,
                PartNumbers = tablePartNumberData,
                AreaOperativityDayTrends = areaOperativityDayTrendData,
                //SupervisorOperativityDayHeatMaps = supervisorOperativityDayHeatMap,
                AnnualAreaTrends = annualAreaTrends // <--- Nuevo campo
            };
        }

        public async IAsyncEnumerable<string> GetOperationalAnalysisStream([EnumeratorCancellation] CancellationToken cancellationToken)
        {
            string user = "UPM.COM.MX\\Administrator";
            string pass = "N3g1T0r01107";

            // Rutas
            string rutaExeDir = @"\\upmap11\c$\UPM\ProductionData";
            string servidorDatos = @"\\upms001";
            string servidorApp = @"\\upmap11";
            string servidorPCP = @"\\upms002";

            // COMANDO ROBUSTO (IPC$ + PUSHD):
            // Autentica en ambos servidores sin letras y luego monta la unidad con pushd.
            string command = $"/C \"net use \"{servidorDatos}\\IPC$\" {pass} /USER:{user} /Y && " +
                             $"net use \"{servidorApp}\\IPC$\" {pass} /USER:{user} /Y && " +
                             $"net use \"{servidorPCP}\\IPC$\" {pass} /USER:{user} /Y && " +
                             $"pushd \"{rutaExeDir}\" && XSLXtoCSV.exe && popd && " +
                             $"net use * /delete /y\"";

            var startInfo = new ProcessStartInfo
            {
                FileName = "cmd.exe",
                Arguments = command,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using var process = new Process { StartInfo = startInfo };

            // --- CORRECCIÓN DEL TRY/CATCH ---
            // 1. Declaramos variable fuera
            string? errorFatal = null;

            try
            {
                process.Start();
            }
            catch (Exception ex)
            {
                // 2. Asignamos el error a la variable (sin yield aquí)
                errorFatal = ex.Message;
            }

            // 3. Hacemos el yield fuera del bloque catch
            if (errorFatal != null)
            {
                yield return $"[FATAL START]: {errorFatal}";
                yield break;
            }
            // --------------------------------

            // Bucle de lectura
            while (!process.StandardOutput.EndOfStream)
            {
                if (cancellationToken.IsCancellationRequested) { process.Kill(); yield break; }

                string? line = await process.StandardOutput.ReadLineAsync(cancellationToken);

                // Filtros de limpieza para no ver mensajes de "net use"
                if (!string.IsNullOrWhiteSpace(line) &&
                    !line.Contains("Comando completado") &&
                    !line.Contains("Se eliminaron"))
                {
                    yield return line;
                }
            }

            string error = await process.StandardError.ReadToEndAsync(cancellationToken);
            // Ignoramos el error 85 (recurso en uso) si aparece
            if (!string.IsNullOrWhiteSpace(error) && !error.Contains("85"))
            {
                yield return $"[LOG ERROR]: {error}";
            }

            await process.WaitForExitAsync(cancellationToken);
        }

        public async Task<PartOperativityData> GetDayOperativity(string partNumber, DateTime startDate, DateTime endDate)
        {
            var dayOperativity = await _temporalContext.OperationalEfficiencies
                .Where(x => x.PartNumberName == partNumber && x.ProductionDate >= startDate && x.ProductionDate <= endDate)
                .GroupBy(x => new { x.Area, x.Supervisor, x.Leader, x.Shift, x.ProductionDate })
                .Select(g => new PartOperativityData
                {
                    PartNumber = partNumber,
                    Area = g.Key.Area,
                    Supervisor = g.Key.Supervisor,
                    Leader = g.Key.Leader,
                    Shift = g.Key.Shift,
                    DayOperativities = g.Select(x => new DayOperativity
                    {
                        Day = g.Key.ProductionDate,
                        Operativity = g.Average(x => x.OperativityPercent)
                    }).ToList()
                }).ToListAsync();


            return new PartOperativityData
            {
                PartNumber = partNumber,
                Area = dayOperativity.FirstOrDefault().Area,
                Supervisor = dayOperativity.FirstOrDefault().Supervisor,
                Leader = dayOperativity.FirstOrDefault().Leader,
                Shift = dayOperativity.FirstOrDefault().Shift,
                Operativity = dayOperativity.SelectMany(x => x.DayOperativities).ToList().Average(x => x.Operativity),
                DayOperativities = dayOperativity.SelectMany(x => x.DayOperativities).OrderBy(x => x.Day).ToList()
            };
        }
    }
}
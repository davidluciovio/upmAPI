using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using upmData.Context;
using upmDomain.DomainDowntime;
using upmDomain.DomainPartNumber;
using upmDomain.DomainPartNumberConfiguration;
using upmDomain.DomainProductionReport;
using upmDomain.Interfaces;
using upmDomain.Lider;
using upmDomain.LineDomain;
using upmDomain.Shift;

namespace upmDomain.ProductionReport
{
    public class ProductionReportService : IService<ProductionReportDto>
    {
        private readonly UpmwebContext _context;
        private WorkShiftService _shiftService;
        private LiderService _liderService;
        public ProductionReportService(UpmwebContext context, WorkShiftService shiftService, LiderService liderService) 
        {
            _context = context;
            _liderService = liderService;
            _shiftService = shiftService;
        }

        public Task<List<ProductionReportDto>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<List<ProductionReportDto>> GetProductionReportsAsync(DateTime startDatetime, DateTime endDatetime, Guid lineId, int modelId)
        {
            // 1. OBTENER IDs DE CONFIGURACIÓN RELEVANTES (Sin cambios)
            var partNumberConfigIdsQuery = _context.PartNumberConfigurations
                .Where(pc => pc.Active && pc.LineId == lineId && pc.ModelId == modelId)
                .Select(pc => pc.Id);

            // 1.5. GENERAR LISTA COMPLETA DE HORAS PARA EL RANGO
            var allHoursInRange = new List<DateTime>();
            // Truncamos la fecha de inicio a la hora en punto para un bucle limpio
            var currentHour = new DateTime(startDatetime.Year, startDatetime.Month, startDatetime.Day, startDatetime.Hour, 0, 0);
            while (currentHour < endDatetime)
            {
                allHoursInRange.Add(currentHour);
                currentHour = currentHour.AddHours(1);
            }

            // 2. OBTENER DATOS DE PRODUCCIÓN AGRUPADOS (Sin cambios, ya estaba bien)
            var hourlyProductionData = await _context.ProductionRegisters
                .Where(pr => pr.Active &&
                             pr.CreateDate >= startDatetime &&
                             pr.CreateDate < endDatetime &&
                             partNumberConfigIdsQuery.Contains(pr.PartNumberConfigurationId))
                .GroupBy(pr => new {
                    Hour = new DateTime(pr.CreateDate.Year, pr.CreateDate.Month, pr.CreateDate.Day, pr.CreateDate.Hour, 0, 0),
                    pr.PartNumberConfigurationId
                })
                .Select(g => new
                {
                    g.Key.Hour,
                    g.Key.PartNumberConfigurationId,
                    TotalProduction = g.Sum(pr => pr.Quantity)
                })
                .ToListAsync();

            // 3. OBTENER DATOS DE PAROS AGRUPADOS (Ajuste en la clave de agrupación)
            var hourlyDowntimeData = await _context.DowntimeRegisters
                .Where(dr => dr.Active &&
                             dr.StartTime >= startDatetime &&
                             dr.EndTime < endDatetime &&
                             dr.EndTime != null &&
                             partNumberConfigIdsQuery.Contains(dr.PartNumberConfigurationId))
                .GroupBy(dr => new {
                    // Usamos el mismo método de truncado que en producción para consistencia
                    Hour = new DateTime(dr.CreateDate.Year, dr.CreateDate.Month, dr.CreateDate.Day, dr.CreateDate.Hour, 0, 0),
                    dr.PartNumberConfigurationId,
                    dr.DowntimeId
                })
                .Select(g => new
                {
                    g.Key.Hour,
                    g.Key.PartNumberConfigurationId,
                    g.Key.DowntimeId,
                    TotalMinutes = g.Sum(dr => EF.Functions.DateDiffMinute(dr.StartTime, dr.EndTime.Value))
                })
                .ToListAsync();

            // 4. OBTENER DETALLES DE LOS PAROS (Sin cambios)
            var downtimeIds = hourlyDowntimeData.Select(d => d.DowntimeId).Distinct();
            var downtimesDictionary = await _context.Downtimes
                .Where(d => downtimeIds.Contains(d.Id))
                .ToDictionaryAsync(d => d.Id, d => DowntimeMapper.Map(d));

            // 5. OBTENER LA ESTRUCTURA PRINCIPAL DEL REPORTE (Sin cambios)
            var partNumberConfigurations = await _context.PartNumberConfigurations
                .Include(pc => pc.PartNumber)
                .Where(pc => pc.Active && pc.LineId == lineId && pc.ModelId == modelId)
                .ToListAsync();

            // 6. ENSAMBLAR EL REPORTE FINAL (LÓGICA PRINCIPAL MODIFICADA)

            // Convertimos los datos en diccionarios para búsquedas O(1) súper rápidas
            var productionLookup = hourlyProductionData
                .ToDictionary(p => (p.PartNumberConfigurationId, p.Hour), p => p.TotalProduction);

            var downtimesLookup = hourlyDowntimeData
                .ToLookup(d => (d.PartNumberConfigurationId, d.Hour), d => d);

            var finalReport = new List<ProductionReportDto>();

            foreach (var pc in partNumberConfigurations)
            {
                var reportDto = new ProductionReportDto
                {
                    EndDatetime = endDatetime,
                    StartDatetime = startDatetime,
                    PartNumber = PartNumberMapper.Map(pc.PartNumber),
                    // Iteramos sobre nuestra lista completa de horas, no sobre los datos de producción
                    TimeProductions = allHoursInRange.Select(hour =>
                    {
                        // Buscamos la producción para esta hora. Si no existe, es 0.
                        productionLookup.TryGetValue((pc.Id, hour), out int totalProduction);

                        // Buscamos los paros para esta hora. El lookup devolverá una lista vacía si no hay.
                        var downtimesForHour = downtimesLookup[(pc.Id, hour)];

                        return new TimeProduction
                        {
                            Time = hour,
                            Production = totalProduction, // Asignamos el valor encontrado o 0
                            Downtimes = downtimesForHour.Select(downtimeGroup => new TimeDowntime
                            {
                                Minutes = TimeSpan.FromMinutes(downtimeGroup.TotalMinutes),
                                Downtime = downtimesDictionary.GetValueOrDefault(downtimeGroup.DowntimeId)
                            }).ToList(),
                            Plan = (60 - downtimesForHour.Sum(downtimeGroup => downtimeGroup.TotalMinutes)) / pc.PartNumber.NetoTime
                        };
                    }).ToList()
                };
                finalReport.Add(reportDto);
            }

            return finalReport;
        }

        public async Task<List<ProductionReportListDto>> GetProductionReportListAsync(List<Guid> liderIds, List<Guid> lineIds, List<int> modelIds, List<Guid> shiftIds, List<DateTime> dates)
        {
            var listProductionReport = new List<ProductionReportListDto>();


            var maxDate = dates.Max();
            var minDate = dates.Min();

            var productionRegisters = _context.ProductionRegisters
                .Where(pr => 
                pr.CreateDate >= minDate &&
                pr.CreateDate < maxDate)
                .GroupBy(pr => new
                {
                    pr.PartNumberConfiguration
                });

            return listProductionReport;
        }
    }
}

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
            // 1. OBTENER LOS IDs DE CONFIGURACIÓN RELEVANTES
            // Consulta base que usaremos en los demás queries. No se ejecuta aún.
            var partNumberConfigIdsQuery = _context.PartNumberConfigurations
                .Where(pc => pc.Active && pc.LineId == lineId && pc.ModelId == modelId)
                .Select(pc => pc.Id);

            // 2. OBTENER TODOS LOS DATOS DE PRODUCCIÓN AGRUPADOS DE UNA SOLA VEZ
            // La base de datos hará el Where, GroupBy y Sum, y nos devolverá solo el agregado.
            var hourlyProductionData = await _context.ProductionRegisters
                .Where(pr => pr.Active &&
                             pr.CreateDate >= startDatetime &&
                             pr.CreateDate < endDatetime &&
                             partNumberConfigIdsQuery.Contains(pr.PartNumberConfigurationId))
                .GroupBy(pr => new { Hour = new DateTime(pr.CreateDate.Year, pr.CreateDate.Month, pr.CreateDate.Day, pr.CreateDate.Hour, 0, 0), pr.PartNumberConfigurationId })
                .Select(g => new
                {
                    g.Key.Hour,
                    g.Key.PartNumberConfigurationId,
                    TotalProduction = g.Sum(pr => pr.Quantity)
                })
                .ToListAsync();

            // 3. OBTENER TODOS LOS DATOS DE PAROS AGRUPADOS DE UNA SOLA VEZ
            var hourlyDowntimeData = await _context.DowntimeRegisters
                .Where(dr => dr.Active &&
                             dr.StartTime >= startDatetime &&
                             dr.EndTime < endDatetime &&
                             dr.EndTime != null &&
                             partNumberConfigIdsQuery.Contains(dr.PartNumberConfigurationId))
                .GroupBy(dr => new { dr.CreateDate.Hour, dr.PartNumberConfigurationId, dr.DowntimeId })
                .Select(g => new
                {
                    g.Key.Hour,
                    g.Key.PartNumberConfigurationId,
                    g.Key.DowntimeId,
                    TotalMinutes = g.Sum(dr => EF.Functions.DateDiffMinute(dr.StartTime, dr.EndTime.Value))
                })
                .ToListAsync();

            // 4. OBTENER LOS NOMBRES/DETALLES DE LOS PAROS NECESARIOS (EVITANDO .Find())
            // Obtenemos los IDs únicos de los paros que encontramos y los buscamos todos a la vez.
            var downtimeIds = hourlyDowntimeData.Select(d => d.DowntimeId).Distinct();
            var downtimesDictionary = await _context.Downtimes
                .Where(d => downtimeIds.Contains(d.Id))
                .ToDictionaryAsync(d => d.Id, d => DowntimeMapper.Map(d));

            // 5. OBTENER LA ESTRUCTURA PRINCIPAL DEL REPORTE
            var partNumberConfigurations = await _context.PartNumberConfigurations
                .Include(pc => pc.PartNumber) // Incluir datos relacionados para evitar más queries
                .Where(pc => pc.Active && pc.LineId == lineId && pc.ModelId == modelId)
                .ToListAsync();

            // 6. ENSAMBLAR EL REPORTE FINAL EN MEMORIA (¡SÚPER RÁPIDO!)

            // Creamos un "lookup" para acceder a los paros de forma eficiente
            var downtimesLookup = hourlyDowntimeData.ToLookup(
                key => (key.Hour, key.PartNumberConfigurationId),
                val => val);

            var finalReport = new List<ProductionReportDto>();

            foreach (var pc in partNumberConfigurations)
            {
                var reportDto = new ProductionReportDto
                {
                    EndDatetime = endDatetime,
                    StartDatetime = startDatetime,
                    PartNumber = PartNumberMapper.Map(pc.PartNumber),
                    TimeProductions = hourlyProductionData
                        .Where(prod => prod.PartNumberConfigurationId == pc.Id)
                        .Select(prodGroup => new TimeProduction
                        {
                            Time = prodGroup.Hour,
                            Production = prodGroup.TotalProduction,
                            Downtimes = downtimesLookup[(prodGroup.Hour.Hour, pc.Id)]
                                .Select(downtimeGroup => new TimeDowntime
                                {
                                    Minutes = TimeSpan.FromMinutes(downtimeGroup.TotalMinutes),
                                    // Buscamos en nuestro diccionario en memoria, no en la BD
                                    Downtime = downtimesDictionary.GetValueOrDefault(downtimeGroup.DowntimeId)
                                })
                                .ToList()
                        })
                        .ToList()
                };
                finalReport.Add(reportDto);
            }

            return finalReport;
        }
    }
}

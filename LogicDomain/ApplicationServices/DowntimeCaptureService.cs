using Entity.Dtos.AplicationDtos.DowntimeCapture;
using LogicData.Context;
using Microsoft.EntityFrameworkCore;

namespace LogicDomain.ApplicationServices
{
    public class DowntimeCaptureService
    {
        private readonly AssyProductionContext _contextAssy;
        private readonly DataContext _contextData;
        public DowntimeCaptureService(AssyProductionContext contextAssy, DataContext contextData)
        {
            _contextAssy = contextAssy;
            _contextData = contextData;
        }

        public async Task<DowntimeCaptureResponseDto> GetDowntimeCaptureData(DowntimeCaptureRequestDto request)
        {
            // 1. Definir intervalos
            var hourlyIntervals = new List<(DateTime Start, DateTime End)>();
            for (DateTime start = request.StartDatetime; start < request.EndDatetime; start = start.AddHours(1))
            {
                hourlyIntervals.Add((start, start.AddHours(1)));
            }

            // 2. Obtener Línea y Lookups base (O(1) access)
            var line = await _contextData.ProductionLines
                .AsNoTracking()
                .FirstOrDefaultAsync(l => l.LineDescription == request.LineDescription);

            if (line == null) throw new Exception("Line not found");

            var downtimeLookup = await _contextData.ProductionDowntimes
                .AsNoTracking()
                .ToDictionaryAsync(d => d.Id, d => d.Programable);

            // 3. Carga masiva de datos de producción filtrados por fecha
            var stationsData = await _contextAssy.ProductionStations
                .Where(ps => ps.LineId == line.Id)
                .Include(ps => ps.PrductionRegisters.Where(pr => pr.CreateDate >= request.StartDatetime && pr.CreateDate < request.EndDatetime))
                .Include(ps => ps.DowntimeRegisters.Where(dr => dr.CreateDate >= request.StartDatetime && dr.CreateDate < request.EndDatetime))
                .AsNoTracking()
                .ToListAsync();

            // 4. Batch Fetch de nombres de Partes y Modelos
            var partIds = stationsData.Select(s => s.PartNumberId).Distinct().ToList();
            var modelIds = stationsData.Select(s => s.ModelId).Distinct().ToList();

            var partLookup = await _contextData.ProductionPartNumbers
                .Where(p => partIds.Contains(p.Id))
                .ToDictionaryAsync(p => p.Id);

            var modelLookup = await _contextData.ProductionModels
                .Where(m => modelIds.Contains(m.Id))
                .ToDictionaryAsync(m => m.Id);

            // 5. Construcción del DTO de respuesta
            var responseDowntimeCaptureResponse = new DowntimeCaptureResponseDto
            {
                LineId = line.Id,
                LineDescription = line.LineDescription,
                partNumberDataProductions = stationsData.Select(ps =>
                {
                    var partInfo = partLookup.GetValueOrDefault(ps.PartNumberId);
                    var modelInfo = modelLookup.GetValueOrDefault(ps.ModelId);

                    // Indexar registros por hora para evitar bucles anidados costosos
                    var prodByHour = ps.PrductionRegisters.ToLookup(pr => pr.CreateDate.Hour);
                    var downByHour = ps.DowntimeRegisters.ToLookup(dr => dr.CreateDate.Hour);

                    return new DowntimeCaptureResponseDto.PartNumberDataProduction
                    {
                        PartNumberId = ps.PartNumberId,
                        PartNumberName = partInfo?.PartNumberName ?? "N/A",
                        PartNumberDescription = partInfo?.PartNumberDescription ?? "N/A",
                        ModelId = ps.ModelId,
                        ModelName = modelInfo?.ModelDescription ?? "N/A",
                        ObjetiveTime = (float)ps.ObjetiveTime,
                        HPTime = (float)ps.NetoTime,
                        hourlyProductionDatas = hourlyIntervals.Select(interval =>
                        {
                            var prInInterval = prodByHour[interval.Start.Hour].ToList();
                            var drInInterval = downByHour[interval.Start.Hour].ToList();

                            // Cálculos de Downtime
                            var downtimeP = (float)drInInterval
                                .Where(dr => downtimeLookup.GetValueOrDefault(dr.DataProductionDowntimeId))
                                .Sum(dr => (dr.EndDowntimeDatetime - dr.StartDowntimeDatetime).TotalMinutes);

                            var downtimeNP = (float)drInInterval
                                .Where(dr => !downtimeLookup.GetValueOrDefault(dr.DataProductionDowntimeId))
                                .Sum(dr => (dr.EndDowntimeDatetime - dr.StartDowntimeDatetime).TotalMinutes);

                            float producedQty = prInInterval.Sum(pr => (float)pr.Quantity);

                            // Lógica de tiempo de trabajo (Mínimo 0 para evitar negativos)
                            float totalWorkingTime = prInInterval.Count > 1
                                ? (float)(prInInterval.Max(pr => pr.CreateDate) - prInInterval.Min(pr => pr.CreateDate)).TotalMinutes
                                : 0f;

                            float minutesPzas = producedQty > 0 ? totalWorkingTime / producedQty : 0f;
                            float objQty = ps.NetoTime > 0 ? (totalWorkingTime / (float)ps.NetoTime) : 0f;
                            float efficiency = (minutesPzas > 0) ? ((float)ps.NetoTime / minutesPzas) : 0f;

                            return new DowntimeCaptureResponseDto.PartNumberDataProduction.HourlyProductionData
                            {
                                StartProductionDate = interval.Start,
                                EndProductionDate = interval.End,
                                DowntimeP = downtimeP,
                                DowntimeNP = downtimeNP,
                                TotalDowntime = downtimeP + downtimeNP,
                                TotalWorkingTime = totalWorkingTime,
                                MinutesPzas = minutesPzas,
                                ProducedQuantity = producedQty,
                                ObjetiveQuantity = objQty,
                                Efectivity = efficiency
                            };
                        }).ToList()
                    };
                }).ToList()
            };

            return new DowntimeCaptureResponseDto
            {
                LineDescription = responseDowntimeCaptureResponse.LineDescription,
                LineId = responseDowntimeCaptureResponse.LineId,
                partNumberDataProductions = responseDowntimeCaptureResponse.partNumberDataProductions
            };
        }
    }
}

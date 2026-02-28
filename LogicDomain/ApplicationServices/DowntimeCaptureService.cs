using Entity.Dtos.AplicationDtos.DowntimeCapture;
using LogicData.Context;
using Microsoft.EntityFrameworkCore;
using LogicData.Models.AssyProduction; // Added for CompleteRackRegister
using Entity.Models.AssyProduction;
using Entity.Models.DataUPM;
using Entity.Dtos.AssyProduction; // Added for LineOperatorsRegister and DowntimeRegister

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
        
        public async Task RegisterCompleteRack(CompleteRackRegisterDto dto)
        {
            var completeRackRegister = new CompleteRackRegister
            {
                Id = Guid.NewGuid(),
                Active = true,
                CreateDate = DateTime.UtcNow,
                CreateBy = "System", // Placeholder for now
                UpdateDate = DateTime.UtcNow,
                UpdateBy = "System", // Placeholder for now
                NoRack = dto.NoRack,
                Serie = dto.Serie,
                Destination = dto.Destination,
                ProductionStationId = dto.ProductionStationId
            };

            _contextAssy.CompleteRackRegisters.Add(completeRackRegister);
            await _contextAssy.SaveChangesAsync();
        }

        public async Task RegisterLineOperators(LineOperatorsRegisterRequestDto dto)
        {
            var lineOperatorsRegister = new LineOperatorsRegister
            {
                Id = Guid.NewGuid(),
                Active = true,
                CreateDate = DateTime.UtcNow,
                CreateBy = dto.CreateBy, // Placeholder for now
                UpdateDate = DateTime.UtcNow,
                UpdateBy = "", // Placeholder for now
                LineId = dto.LineId,
                OperatorCode = dto.OperatorCode,
                OperatorName = dto.OperatorName,
                StartDatetime = dto.StartDatetime,
                EndDatetime = dto.EndDatetime,
                Complete = false
            };

            _contextAssy.LineOperatorsRegisters.Add(lineOperatorsRegister);
            await _contextAssy.SaveChangesAsync();
        }

        public async Task RegisterDowntime(DowntimeRegisterRequestDto dto)
        {
            // 1. Validaciones de Lógica de Negocio
            if (dto.StartDowntimeDatetime >= dto.EndDowntimeDatetime)
            {
                throw new ArgumentException("La fecha de inicio debe ser anterior a la fecha de fin.");
            }

            if (dto.StartDowntimeDatetime > DateTime.UtcNow)
            {
                throw new ArgumentException("No se puede registrar un tiempo de inactividad en el futuro.");
            }

            // 2. Validaciones de Integridad (Opcional pero recomendado)
            // Verificamos que los IDs relacionados existan en la base de datos
            var stationExists = await _contextAssy.ProductionStations
                .AnyAsync(s => s.Id == dto.ProductionStationId);

            if (!stationExists)
            {
                throw new KeyNotFoundException($"La estación de producción con ID {dto.ProductionStationId} no existe.");
            }

            // 3. Mapeo y Creación
            var downtimeRegister = new DowntimeRegister
            {
                Id = Guid.NewGuid(),
                Active = true,
                CreateDate = DateTime.UtcNow,
                CreateBy = dto.CreateBy,
                UpdateDate = DateTime.UtcNow,
                UpdateBy = dto.UpdateBy,
                StartDowntimeDatetime = dto.StartDowntimeDatetime,
                EndDowntimeDatetime = dto.EndDowntimeDatetime,
                DataProductionDowntimeId = dto.DataProductionDowntimeId,
                ProductionStationId = dto.ProductionStationId
            };

            try
            {
                _contextAssy.DowntimeRegisters.Add(downtimeRegister);
                await _contextAssy.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                // Captura errores de base de datos (ej. violaciones de FK o triggers)
                throw new Exception("Error al persistir el registro de inactividad.", ex);
            }
        }


        public async Task<List<LineOperatorsRegisterResponseDto>> GetLineOperatorsRegisters(LineOperatorsRegisterRequestDto request)
        {
            return _contextAssy.LineOperatorsRegisters
                .Where(or => or.StartDatetime >= request.StartDatetime && or.EndDatetime <= request.EndDatetime && or.LineId == request.LineId && or.Complete == false)
                .Select(or => new LineOperatorsRegisterResponseDto
                {
                    Id = or.Id,
                    OperatorCode = or.OperatorCode,
                    OperatorName = or.OperatorName,
                    StartDatetime = or.StartDatetime,
                    EndDatetime = or.EndDatetime
                }).ToList();
        }

        public async Task<List<DataActiveEmployees>> GetActiveEmployees(string request)
        {
            return await _contextData.ActiveEmployees
                .Select(e => new DataActiveEmployees
                {
                    CB_APE_MAT = e.CB_APE_MAT,
                    CB_APE_PAT = e.CB_APE_PAT,
                    CB_CODIGO = e.CB_CODIGO,
                    CB_NOMBRES = e.CB_NOMBRES,
                    PRETTYNAME = e.CB_CODIGO.ToString() + " - "  + e.PRETTYNAME,
                    IM_BLOB = e.IM_BLOB
                })
                .Where(e => e.PRETTYNAME.Contains(request))
                .ToListAsync();
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

            // New: Fetch ProductionDowntime descriptions
            var downtimeDescriptionLookup = await _contextData.ProductionDowntimes
                .AsNoTracking()
                .ToDictionaryAsync(d => d.Id, d => d.DowntimeDescription);

            // New: Fetch LineOperatorsRegister for the entire line within the time frame
            var lineOperators = await _contextAssy.LineOperatorsRegisters
                .Where(lo => lo.LineId == line.Id && lo.StartDatetime < request.EndDatetime && lo.EndDatetime > request.StartDatetime)
                .AsNoTracking()
                .ToListAsync();

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

                    // New: Filter operators relevant to this station's line (all operators fetched are for this line)
                    // The request asked for operators based on ProductionStationId, but LineOperatorsRegister only has LineId.
                    // So we provide all operators for the line. If a more specific filter is needed, the model needs to be updated.
                    var stationOperators = lineOperators
                        .Where(lo => lo.StartDatetime < request.EndDatetime && lo.EndDatetime > request.StartDatetime && lo.LineId == line.Id) // Additional check for clarity, though already filtered
                        .Select(lo => new DowntimeCaptureResponseDto.PartNumberDataProduction.OperatorsDto
                        {
                            LineOperatorId = lo.Id,
                            OperatorCode = lo.OperatorCode,
                            OperatorName = lo.OperatorName,
                            StartDatetime = lo.StartDatetime,
                            EndDatetime = lo.EndDatetime
                        }).ToList();

                    // New: Map DowntimeRegisters for this station
                    var stationDowntimeRegisters = ps.DowntimeRegisters
                        .Select(dr => new DowntimeCaptureResponseDto.PartNumberDataProduction.DowntimeRegisterData
                        {
                            Id = dr.Id,
                            StartDowntimeDatetime = dr.StartDowntimeDatetime,
                            EndDowntimeDatetime = dr.EndDowntimeDatetime,
                            DataProductionDowntimeId = dr.DataProductionDowntimeId,
                            ProductionStationId = dr.ProductionStationId,
                            DowntimeReason = downtimeDescriptionLookup.GetValueOrDefault(dr.DataProductionDowntimeId, "Unknown")
                        }).ToList();

                    return new DowntimeCaptureResponseDto.PartNumberDataProduction
                    {
                        ProductionStationId = ps.Id,
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
                                ? (float)(prInInterval.Max(pr => pr.CreateDate) - prInInterval.Min(pr => pr.CreateDate)).TotalMinutes - downtimeNP
                                : 0f ;

                            if(totalWorkingTime < 0) totalWorkingTime = 0;

                            float minutesPzas = producedQty > 0 ? totalWorkingTime / producedQty : 0f;
                            float objQty = ps.NetoTime > 0 && totalWorkingTime > 0 ? (totalWorkingTime / (float)ps.NetoTime) : 0f;
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
                                ObjetiveQuantity = (int)objQty,
                                Efectivity = efficiency,
                                LastDatetimeProduction = prInInterval.Any() ? prInInterval.Max(pr => pr.CreateDate) : interval.Start,
                            };
                        }).ToList(),
                        Operators = stationOperators, // Populate new Operators list
                        DowntimeRegisters = stationDowntimeRegisters // Populate new DowntimeRegisters list
                    };
                }).ToList()
            };

            var partNumberProductionsWithOutput = responseDowntimeCaptureResponse.partNumberDataProductions
                .Where(p => p.hourlyProductionDatas.Any(h => h.StartProductionDate <= DateTime.Now))
                .ToList();

            partNumberProductionsWithOutput.ForEach(p =>
            {
                p.hourlyProductionDatas = p.hourlyProductionDatas.Where(h => h.StartProductionDate <= DateTime.Now).ToList();
            });

            return new DowntimeCaptureResponseDto
            {
                LineDescription = responseDowntimeCaptureResponse.LineDescription,
                LineId = responseDowntimeCaptureResponse.LineId,
                partNumberDataProductions = partNumberProductionsWithOutput
            };
        }

        public async Task<ProductionStationResponseDto> GetProductionStationbyPartNumber(string partnumber)
        {

        }


        public async Task PutLineOperatorRegisters(Guid id, LineOperatorsRegisterRequestDto request)
        {
            var existingRegister = await _contextAssy.LineOperatorsRegisters.FindAsync(id);

            if (existingRegister == null) throw new Exception("Line Operator Register not found");

            existingRegister.OperatorCode = request.OperatorCode;
            existingRegister.OperatorName = request.OperatorName;
            existingRegister.StartDatetime = request.StartDatetime;
            existingRegister.EndDatetime = request.EndDatetime;
            existingRegister.UpdateDate = DateTime.UtcNow;
            existingRegister.UpdateBy = request.UpdateBy; // Placeholder for now
            existingRegister.UpdateDate = DateTime.UtcNow;
            existingRegister.UpdateBy = request.UpdateBy; // Placeholder for now
            existingRegister.Complete = request.Complete;

            _contextAssy.LineOperatorsRegisters.Update(existingRegister);
            await _contextAssy.SaveChangesAsync();
        }
    
    }
}

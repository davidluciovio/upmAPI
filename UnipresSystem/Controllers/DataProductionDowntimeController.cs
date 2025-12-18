using Entity.ModelDtos._02_DataProduction.DataProductionDowntime;
using Entity.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace UnipresSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DataProductionDowntimeController : ControllerBase
    {
        private readonly IService<DataProductionDowntimeResponseDto, DataProductionDowntimeRequestDto> _dataProductionDowntimeService;

        public DataProductionDowntimeController(IService<DataProductionDowntimeResponseDto, DataProductionDowntimeRequestDto> dataProductionDowntimeService)
        {
            _dataProductionDowntimeService = dataProductionDowntimeService;
        }

        [HttpGet("v1/get-all")]
        public async Task<IActionResult> GetAllDowntimes()
        {
            try
            {
                var downtimes = await _dataProductionDowntimeService.GetAlls();
                return Ok(downtimes);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message} - {ex.InnerException?.Message}");
            }
        }

        [HttpGet("v1/get-id/{id}")]
        public async Task<IActionResult> GetDowntimeById(Guid id)
        {
            try
            {
                var downtime = await _dataProductionDowntimeService.GetById(id);
                if (downtime == null)
                {
                    return NotFound();
                }
                return Ok(downtime);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message} - {ex.InnerException?.Message}");
            }
        }

        [HttpPost("v1/create")]
        public async Task<IActionResult> CreateDowntime([FromBody] DataProductionDowntimeRequestDto createDto)
        {
            try
            {
                if (createDto == null)
                {
                    return BadRequest("The submitted object is null.");
                }

                var newDowntime = await _dataProductionDowntimeService.Create(createDto);

                return CreatedAtAction(nameof(GetDowntimeById), new { id = newDowntime.Id }, newDowntime);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message} - {ex.InnerException?.Message}");
            }
        }

        [HttpPost("v1/update/{id}")]
        public async Task<IActionResult> UpdateDowntime(Guid id, [FromBody] DataProductionDowntimeRequestDto updateDto)
        {
            try
            {
                var updatedDowntime = await _dataProductionDowntimeService.Update(id, updateDto);
                return Ok(updatedDowntime);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message} - {ex.InnerException?.Message}");
            }
        }
    }
}

using Entity.Dtos._00_DataUPM;
using Entity.Interfaces;
using LogicDomain._00_DataUPM;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace UnipresSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DataSecurityUserController : ControllerBase
    {
        private readonly IServiceCrud<DataSecurityUserDto, DataSecurityUserCreateDto, DataSecurityUserUpdateDto> _dataSecurityUserService;

        public DataSecurityUserController(IServiceCrud<DataSecurityUserDto, DataSecurityUserCreateDto, DataSecurityUserUpdateDto> dataSecurityUserService)
        {
            _dataSecurityUserService = dataSecurityUserService;
        }

        [HttpGet("v1/get-all")]
        public async Task<IActionResult> GetAllUsers()
        {
            try
            {
                var users = await _dataSecurityUserService.GetAlls();
                return Ok(users);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message} - {ex.InnerException?.Message}");
            }
        }

        [HttpGet("v1/get-id/{id}")]
        public async Task<IActionResult> GetUserById(Guid id)
        {
            try
            {
                var user = await _dataSecurityUserService.GetById(id);
                if (user == null)
                {
                    return NotFound();
                }
                return Ok(user);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message} - {ex.InnerException?.Message}");
            }
        }

        [HttpPost("v1/create")]
        public async Task<IActionResult> CreateUser([FromBody] DataSecurityUserCreateDto createDto)
        {
            try
            {
                if (createDto == null)
                {
                    return BadRequest("The submitted object is null.");
                }

                var newUser = await _dataSecurityUserService.Create(createDto);

                return CreatedAtAction(nameof(GetUserById), new { id = newUser.Id }, newUser);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("v1/update/{id}")]
        public async Task<IActionResult> UpdateUser(Guid id, [FromBody] DataSecurityUserUpdateDto updateDto)
        {
            try
            {
                var updatedUser = await _dataSecurityUserService.Update(id, updateDto);

                if (updatedUser == null)
                {
                    return NotFound($"User with id: {id} not found.");
                }

                return Ok(updatedUser);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message} - {ex.InnerException?.Message}");
            }
        }
    }
}

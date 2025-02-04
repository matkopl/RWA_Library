using BL.Models;
using BL.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class LogController : ControllerBase
    {
        private readonly ILogRepository _logRepository;

        public LogController(ILogRepository logRepository)
        {
            _logRepository = logRepository;
        }

        // GET: api/<LogController>
        [HttpGet("/api/Log/get/{n}")]
        public IActionResult Get(int n = 10)
        {
            try
            {
                if (_logRepository.GetLogCount() < n)
                {
                    return BadRequest($"Number of logs is less than {n}");
                }

                return Ok(_logRepository.GetByAmount(n));
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        // GET api/<LogController>/5
        [HttpGet("/api/Log/Count")]
        public IActionResult Count() => Ok(_logRepository.GetLogCount());
    }
}

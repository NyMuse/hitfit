using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using hitfit.app.Models;
using hitfit.app.Models.Dictionaries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace hitfit.app.Controllers.Api
{
    [Authorize]
    [Produces("application/json")]
    [Route("api/Dictionaries")]
    public class DictionariesController : Controller
    {
        private readonly HitFitDbContext _context;

        public DictionariesController(HitFitDbContext context)
        {
            _context = context;
        }

        [HttpGet("measurementtypes")]
        public IEnumerable<MeasurementType> GetMeasurementTypes()
        {
            return _context.MeasurementTypes;
        }

        [HttpGet("measurementtypes/{id}")]
        public async Task<IActionResult> GetMeasurementType(int id)
        {
            var measurementType = await _context.MeasurementTypes.SingleOrDefaultAsync(m => m.Id == id);

            return Ok(measurementType);
        }

        [HttpGet("measurementtypes/{key}")]
        public async Task<IActionResult> GetMeasurementType(string key)
        {
            var measurementType = await _context.MeasurementTypes.SingleOrDefaultAsync(m => m.Key.Equals(key));

            return Ok(measurementType);
        }

        [HttpPost("measurementtypes")]
        public async Task<IActionResult> PostMeasurementType([FromBody] MeasurementType type)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.MeasurementTypes.Add(type);

            await _context.SaveChangesAsync();

            return CreatedAtAction("GetMeasurementType", new { id = type.Id });
        }





        [HttpGet("programtypes")]
        public IEnumerable<ProgramType> GetProgramTypes()
        {
            return _context.ProgramTypes;
        }

        [HttpGet("programtypes/{id}")]
        public async Task<IActionResult> GetProgramType(int id)
        {
            var type = await _context.ProgramTypes.SingleOrDefaultAsync(m => m.Id == id);

            return Ok(type);
        }

        [HttpGet("programtypes/{key}")]
        public async Task<IActionResult> GetProgramType(string key)
        {
            var type = await _context.ProgramTypes.SingleOrDefaultAsync(m => m.Key.Equals(key));

            return Ok(type);
        }

        [HttpPost("programtypes")]
        public async Task<IActionResult> PostProgramType([FromBody] ProgramType type)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.ProgramTypes.Add(type);

            await _context.SaveChangesAsync();

            return CreatedAtAction("GetProgramType", new { id = type.Id });
        }

        [HttpGet("reporttypes")]
        public IEnumerable<ReportType> GetReportTypes()
        {
            return _context.ReportTypes;
        }

        [HttpGet("reporttypes/{id}")]
        public async Task<IActionResult> GetReportType(int id)
        {
            var type = await _context.ReportTypes.SingleOrDefaultAsync(m => m.Id == id);

            return Ok(type);
        }

        [HttpGet("reporttypes/{key}")]
        public async Task<IActionResult> GetReportType(string key)
        {
            var type = await _context.ReportTypes.SingleOrDefaultAsync(m => m.Key.Equals(key));

            return Ok(type);
        }

        [HttpPost("reporttypes")]
        public async Task<IActionResult> PostReportType([FromBody] ReportType type)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.ReportTypes.Add(type);

            await _context.SaveChangesAsync();

            return CreatedAtAction("GetReportType", new { id = type.Id });
        }
    }
}
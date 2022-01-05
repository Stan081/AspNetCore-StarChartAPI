using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using StarChart.Data;

namespace StarChart.Controllers
{
    [ApiController]
    [Route("")]
    public class CelestialObjectController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public CelestialObjectController(ApplicationDbContext context)
        {
            _context = context;

        }

        [HttpGet("{id}")]
        public IActionResult GetByID(int Id)
        {
            var records = _context.CelestialObjects;

            var record = records.Find(Id);
            if (record == null) return NotFound();

            var result = records.Where(x => x.OrbitedObjectId == Id).ToList();
            if (result != null)
            {
                record.Satellites.Insert(1, record);
            }
            return Ok(record);

        }

        [HttpGet("{name}")]
        public IActionResult GetByID(string Name)
        {
            var output = _context.CelestialObjects;

            var records = output.Where(x => x.Name == Name);
            if (records == null) return NotFound();

            foreach (var record in records)
            {
                var check = output.Where(x => x.OrbitedObjectId == record.Id);
                record.Satellites.AddRange(check);
            }
            return Ok(records);

        }
    }
}

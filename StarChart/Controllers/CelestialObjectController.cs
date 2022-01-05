using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using StarChart.Data;
using StarChart.Models;

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

        [HttpGet("{id:int}")]
        public IActionResult GetById(int Id)
        {
            var records = _context.CelestialObjects;

            var record = records.Find(Id);
            if (record == null)
                return NotFound();

            var result = records.Where(x => x.OrbitedObjectId == Id).ToList();
            if (result != null)
            {
                record.Satellites = result;
            }
            return Ok(record);

        }

        [HttpGet("{name}")]
        public IActionResult GetByName(string Name)
        {
            var output = _context.CelestialObjects;

            var records = output.Where(x => x.Name == Name);

            if (!records.Any())
                return NotFound();

            foreach (var record in records)
            {
                var check = output.Where(x => x.OrbitedObjectId == record.Id).ToList();
                record.Satellites = check;
            }
            return Ok(records);

        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var records = _context.CelestialObjects.ToList();
            foreach (var record in records)
            {
                var check = records.Where(x => x.OrbitedObjectId == record.Id).ToList();
                record.Satellites = check;
            }
            return Ok(records);

        }

        [HttpPost]
        public IActionResult Create([FromBody] CelestialObject obj)
        {
            _context.Add(obj);
            _context.SaveChanges();

            return CreatedAtRoute(nameof(GetById),
                new { id = obj.Id },
                obj);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int Id, CelestialObject obj)
        {
            if (Id != obj.Id) return BadRequest();
            var record = _context.CelestialObjects.Find(Id);
            if (record == null) return NotFound();

            record.Name = obj.Name;
            record.OrbitalPeriod = obj.OrbitalPeriod;
            record.OrbitedObjectId = obj.OrbitedObjectId;

            _context.CelestialObjects.Update(record);
            _context.SaveChanges();

            return NoContent();

        }

        [HttpPatch("{id}")]
        public IActionResult RenameObject(int Id, CelestialObject obj)
        {
            if (Id != obj.Id) return BadRequest();
            var record = _context.CelestialObjects.Find(Id);
            if (record == null) return NotFound();

            record.Name = obj.Name;

            _context.CelestialObjects.Update(record);
            _context.SaveChanges();

            return NoContent();

        }


        [HttpDelete("{id}")]
        public IActionResult Delete(int Id)
        {
            var records = _context.CelestialObjects.Where(x => x.Id == Id || x.OrbitedObjectId == Id).ToList();
            if (records == null) return NotFound();

            foreach (var record in records)
            {
                _context.Remove(record);
            }
           
            _context.SaveChanges();

            return NoContent();

        }


    }
}

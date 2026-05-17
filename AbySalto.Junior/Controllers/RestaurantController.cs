using AbySalto.Junior.Infrastructure.Database;
using AbySalto.Junior.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AbySalto.Junior.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RestaurantController : Controller
    {
        private readonly IApplicationDbContext _context;

        public RestaurantController(IApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> DohvatiSveNarudzbe()
        {
            var narudzbe = await _context.Narudzbe
                .Include(n => n.Stavke)
                .ToListAsync();

            return Ok(narudzbe);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> DohvatiNarudzbu(int id)
        {
            var narudzba = await _context.Narudzbe
                .Include(n => n.Stavke)
                .FirstOrDefaultAsync(n => n.Id == id);
            if (narudzba == null)
                return NotFound($"Narudzba s ID {id} nije pronadena.");
            return Ok(narudzba);
        }

        [HttpPost]
        public async Task<IActionResult> KreirajNarudzbu([FromBody] Models.Narudzba novaNarudzba)
        {
            if (novaNarudzba == null)
                return BadRequest("Narudzba ne moze biti prazna!");

            novaNarudzba.DatumNarudzbe = DateTime.Now;
            novaNarudzba.Status = StatusNarudzbe.NaCekanju;

            _context.Narudzbe.Add(novaNarudzba);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(DohvatiNarudzbu), new { id = novaNarudzba.Id }, novaNarudzba);
        }

        [HttpPut("{id}/status")]
        public async Task<IActionResult> UpdateStatus(int id, [FromBody] StatusNarudzbe status)
        {
            var narudzba = await _context.Narudzbe
                .FirstOrDefaultAsync(n => n.Id == id);

            if (narudzba == null)
                return NotFound($"Narudzba s ID {id} nije pronadena.");

            narudzba.Status = status;
            await _context.SaveChangesAsync();

            return Ok(new { poruka = "Status uspjesno azuriran.", noviStatus = status.ToString() });
        }

        [HttpGet("{id}/ukupno")]
        public async Task<IActionResult> GetUkupno(int id)
        {
            var narudzba = await _context.Narudzbe
                .Include(n => n.Stavke)
                .FirstOrDefaultAsync(n => n.Id == id);

            if (narudzba == null)
                return NotFound($"Narudzba s ID {id} nije pronadena.");

            return Ok(new
            {
                narudzba.Id,
                narudzba.ImeKupca,
                narudzba.UkupnaCijena,
                narudzba.Valuta
            });
        }
    }
}

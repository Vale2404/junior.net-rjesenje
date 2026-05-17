using AbySalto.Junior.Infrastructure.Database;
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
    }
}

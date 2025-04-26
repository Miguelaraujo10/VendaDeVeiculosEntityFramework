using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjetoCompletoLocadora.Data;
using ProjetoCompletoLocadora.Models;

namespace ProjetoCompletoLocadora.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FabricantesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public FabricantesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Fabricantes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Fabricante>>> GetFabricantes()
        {
            if (_context.Fabricantes == null)
            {
                return NotFound();
            }
            return await _context.Fabricantes.ToListAsync();
        }

        // GET: api/Fabricantes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Fabricante>> GetFabricante(int id)
        {
            if (_context.Fabricantes == null)
            {
                return NotFound();
            }

            var fabricante = await _context.Fabricantes
                .Include(f => f.Veiculos)  // JOIN com Veiculos
                .FirstOrDefaultAsync(f => f.Id == id);

            if (fabricante == null)
            {
                return NotFound();
            }

            return fabricante;
        }

        // Filtro 1: Buscar fabricantes com veículos
        [HttpGet("filtro1")]
        public async Task<ActionResult<IEnumerable<Fabricante>>> GetFabricantesComVeiculos()
        {
            var fabricantes = await _context.Fabricantes
                .Include(f => f.Veiculos)  // JOIN com Veiculos
                .ToListAsync();

            if (fabricantes == null)
            {
                return NotFound();
            }

            return fabricantes;
        }

        // Filtro 2: Buscar fabricantes por nome
        [HttpGet("filtro2")]
        public async Task<ActionResult<IEnumerable<Fabricante>>> GetFabricantesPorNome(string nome)
        {
            var fabricantes = await _context.Fabricantes
                .Where(f => f.Nome.Contains(nome))  // Filtro por nome
                .Include(f => f.Veiculos)  // JOIN com Veiculos
                .ToListAsync();

            if (fabricantes == null)
            {
                return NotFound();
            }

            return fabricantes;
        }

        // Filtro 3: Buscar fabricantes com um determinado número de veículos
        [HttpGet("filtro3")]
        public async Task<ActionResult<IEnumerable<Fabricante>>> GetFabricantesComVeiculosMaiorQue(int numeroVeiculos)
        {
            var fabricantes = await _context.Fabricantes
                .Where(f => f.Veiculos.Count() >= numeroVeiculos)  // Filtro de quantidade de veículos
                .Include(f => f.Veiculos)  // JOIN com Veiculos
                .ToListAsync();

            if (fabricantes == null)
            {
                return NotFound();
            }

            return fabricantes;
        }

        // Filtro 4: Buscar fabricantes e seus veículos com detalhes de categoria
        [HttpGet("filtro4")]
        public async Task<ActionResult<IEnumerable<Fabricante>>> GetFabricantesComVeiculosECategorias()
        {
            var fabricantes = await _context.Fabricantes
                .Include(f => f.Veiculos)  // JOIN com Veiculos
                .ThenInclude(v => v.Categoria)  // JOIN com Categoria
                .ToListAsync();

            if (fabricantes == null)
            {
                return NotFound();
            }

            return fabricantes;
        }

        // Filtro 5: Buscar fabricantes com veículos e aluguel
        [HttpGet("filtro5")]
        public async Task<ActionResult<IEnumerable<Fabricante>>> GetFabricantesComVeiculosEAlugueis()
        {
            var fabricantes = await _context.Fabricantes
                .Include(f => f.Veiculos)  // JOIN com Veiculos
                .ThenInclude(v => v.Alugueis)  // JOIN com Alugueis
                .ToListAsync();

            if (fabricantes == null)
            {
                return NotFound();
            }

            return fabricantes;
        }

        // PUT: api/Fabricantes/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutFabricante(int id, Fabricante fabricante)
        {
            if (id != fabricante.Id)
            {
                return BadRequest();
            }

            _context.Entry(fabricante).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FabricanteExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Fabricantes
        [HttpPost]
        public async Task<ActionResult<Fabricante>> PostFabricante(Fabricante fabricante)
        {
            if (_context.Fabricantes == null)
            {
                return Problem("Entity set 'AppDbContext.Fabricantes'  is null.");
            }
            _context.Fabricantes.Add(fabricante);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetFabricante", new { id = fabricante.Id }, fabricante);
        }

        // DELETE: api/Fabricantes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFabricante(int id)
        {
            if (_context.Fabricantes == null)
            {
                return NotFound();
            }
            var fabricante = await _context.Fabricantes.FindAsync(id);
            if (fabricante == null)
            {
                return NotFound();
            }

            _context.Fabricantes.Remove(fabricante);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool FabricanteExists(int id)
        {
            return (_context.Fabricantes?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}

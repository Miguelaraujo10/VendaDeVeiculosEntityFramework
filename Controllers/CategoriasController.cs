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
    public class CategoriasController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CategoriasController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Categorias
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Categoria>>> GetCategorias()
        {
            if (_context.Categorias == null)
            {
                return NotFound();
            }
            return await _context.Categorias
                .Include(c => c.Veiculos) // INNER JOIN com Veiculos
                .ToListAsync();
        }

        // GET: api/Categorias/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Categoria>> GetCategoria(int id)
        {
            if (_context.Categorias == null)
            {
                return NotFound();
            }

            var categoria = await _context.Categorias
                .Include(c => c.Veiculos) // INNER JOIN com Veiculos
                .FirstOrDefaultAsync(c => c.Id == id);

            if (categoria == null)
            {
                return NotFound();
            }

            return categoria;
        }

        // Filtro 1: Buscar categorias que possuem veículos
        [HttpGet("filtro1")]
        public async Task<ActionResult<IEnumerable<Categoria>>> GetCategoriasComVeiculos()
        {
            var categorias = await _context.Categorias
                .Where(c => c.Veiculos.Any()) // Filtra categorias que possuem veículos
                .Include(c => c.Veiculos) // LEFT JOIN com Veiculos
                .ToListAsync();

            if (categorias == null)
            {
                return NotFound();
            }

            return categorias;
        }

        // Filtro 2: Buscar todas as categorias por nome
        [HttpGet("filtro2")]
        public async Task<ActionResult<IEnumerable<Categoria>>> GetCategoriasPorNome(string nome)
        {
            var categorias = await _context.Categorias
                .Where(c => c.Nome.Contains(nome)) // Filtra categorias pelo nome
                .Include(c => c.Veiculos) // INNER JOIN com Veiculos
                .ToListAsync();

            if (categorias == null)
            {
                return NotFound();
            }

            return categorias;
        }

        // Filtro 3: Buscar categorias com a quantidade de veículos
        [HttpGet("filtro3")]
        public async Task<ActionResult<IEnumerable<object>>> GetCategoriasComQuantidadeDeVeiculos()
        {
            var categorias = await _context.Categorias
                .Select(c => new
                {
                    c.Id,
                    c.Nome,
                    VeiculosCount = c.Veiculos.Count() // Contagem de veículos na categoria
                })
                .ToListAsync();

            if (categorias == null)
            {
                return NotFound();
            }

            return categorias;
        }

        // Filtro 4: Buscar categorias e seus respectivos veículos e fabricantes
        [HttpGet("filtro4")]
        public async Task<ActionResult<IEnumerable<Categoria>>> GetCategoriasComVeiculosEFabricantes()
        {
            var categorias = await _context.Categorias
                .Include(c => c.Veiculos) // INNER JOIN com Veiculos
                .ThenInclude(v => v.Fabricante) // LEFT JOIN com Fabricante
                .ToListAsync();

            if (categorias == null)
            {
                return NotFound();
            }

            return categorias;
        }

        // Filtro 5: Buscar categorias sem veículos
        [HttpGet("filtro5")]
        public async Task<ActionResult<IEnumerable<Categoria>>> GetCategoriasSemVeiculos()
        {
            var categorias = await _context.Categorias
                .Where(c => !c.Veiculos.Any()) // Filtra categorias sem veículos
                .ToListAsync();

            if (categorias == null)
            {
                return NotFound();
            }

            return categorias;
        }

        // PUT: api/Categorias/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCategoria(int id, Categoria categoria)
        {
            if (id != categoria.Id)
            {
                return BadRequest();
            }

            _context.Entry(categoria).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CategoriaExists(id))
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

        // POST: api/Categorias
        [HttpPost]
        public async Task<ActionResult<Categoria>> PostCategoria(Categoria categoria)
        {
            if (_context.Categorias == null)
            {
                return Problem("Entity set 'AppDbContext.Categorias'  is null.");
            }
            _context.Categorias.Add(categoria);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCategoria", new { id = categoria.Id }, categoria);
        }

        // DELETE: api/Categorias/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategoria(int id)
        {
            if (_context.Categorias == null)
            {
                return NotFound();
            }
            var categoria = await _context.Categorias.FindAsync(id);
            if (categoria == null)
            {
                return NotFound();
            }

            _context.Categorias.Remove(categoria);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CategoriaExists(int id)
        {
            return (_context.Categorias?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
